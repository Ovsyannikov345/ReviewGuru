import React, { useEffect, useMemo, useState } from "react";
import { FormControl, Grid, Typography, TextField, Button, Select, InputLabel, MenuItem, Chip } from "@mui/material";
import useApiRequest from "./../hooks/useApiRequest";
import useSnackbar from "./../hooks/useSnackbar";
import { useNavigate } from "react-router-dom";
import AuthorAutocomplete from "../components/AuthorAutocomplete";
import NavigateBack from "./../components/buttons/NavigateBack";

const MediaCreationPage = ({ accessToken, refreshToken, setAccessToken, setRefreshToken }) => {
    const navigate = useNavigate();

    const sendRequest = useApiRequest(accessToken, refreshToken, setAccessToken, setRefreshToken);

    const { displayError, ErrorSnackbar } = useSnackbar();

    const [newMedia, setNewMedia] = useState({
        mediaType: "",
        name: "",
        yearOfCreating: "",
        authors: [],
    });

    const [authors, setAuthors] = useState([]);

    const availableAuthors = useMemo(() => {
        const availableAuthors = authors.filter((author) => !newMedia.authors.includes(author));

        availableAuthors.forEach((author) => {
            author.groupLetter = author.firstName[0];
        });

        availableAuthors.sort((a, b) => a.groupLetter.localeCompare(b.groupLetter));

        return availableAuthors;
    }, [authors, newMedia.authors]);

    useEffect(() => {
        const fetchAuthors = async () => {
            const response = await sendRequest("authors", "get", {}, {});

            if (!response.ok) {
                displayError(response.error);
                return;
            }

            setAuthors(response.data);
        };

        const fetchData = async () => {
            await fetchAuthors();
        };

        fetchData();
    }, []);

    const addAuthorToNewMedia = (author) => {
        setNewMedia({ ...newMedia, authors: [...newMedia.authors, author] });
    };

    const addNewAuthorToNewMedia = (newAuthor) => {
        if (authors.some((author) => author.firstName === newAuthor.firstName && author.lastName === newAuthor.lastName)) {
            displayError("Author already exists");
            return;
        }

        setNewMedia({
            ...newMedia,
            authors: [...newMedia.authors, newAuthor],
        });
    };

    const removeAuthorFromNewMedia = (firstName, lastName) => {
        setNewMedia({
            ...newMedia,
            authors: newMedia.authors.filter((author) => !(author.firstName === firstName && author.lastName === lastName)),
        });
    };

    const createMedia = async () => {
        // TODO implement
        // if (review.rating == null || review.userReview.trim().length === 0) {
        //     displayError("Fill the review information");
        //     return;
        // }
        // if (!newMediaMode) {
        //     if (review.mediaToCreateDTO == null) {
        //         displayError("Fill the review information");
        //         return;
        //     }
        //     const response = await sendRequest("review/CreateReview", "post", review, {});
        //     if (!response.ok) {
        //         displayError(response.error);
        //         return;
        //     }
        //     navigate(-1);
        //     return;
        // }
        // if (newMedia.mediaType === "") {
        //     displayError("Select media type");
        //     return;
        // }
        // if (!newMedia.name) {
        //     displayError("Fill media name");
        //     return;
        // }
        // if (newMedia.yearOfCreating && (newMedia.yearOfCreating < 1 || newMedia.yearOfCreating > new Date().getFullYear())) {
        //     displayError("Invalid media creation year");
        //     return;
        // }
        // if (newMedia.mediaType === "Movie") {
        //     const response = await sendRequest(
        //         "OMDb/CreateReview",
        //         "post",
        //         {
        //             rating: review.rating,
        //             userReview: review.userReview,
        //             mediaName: newMedia.name,
        //             yearOfMediaCreation: newMedia.yearOfCreating ? newMedia.yearOfCreating : null,
        //         },
        //         {}
        //     );
        //     if (response.ok) {
        //         navigate(-1);
        //         return;
        //     }
        //     if (response.status === 400) {
        //         displayError(response.error);
        //         return;
        //     }
        // }
        // if (!newMedia.yearOfCreating) {
        //     displayError("Enter media creation year");
        //     return;
        // }
        // const reviewData = {
        //     ...review,
        //     mediaToCreateDTO: {
        //         ...newMedia,
        //         yearOfCreating: `${newMedia.yearOfCreating}-01-01`,
        //         authorsToCreateDTO: newMedia.authors.map((author) => ({
        //             authorId: author.authorId,
        //             firstName: author.firstName,
        //             lastName: author.lastName,
        //         })),
        //     },
        // };
        // const response = await sendRequest("review/CreateReview", "post", reviewData, {});
        // if (!response.ok) {
        //     displayError(response.error);
        //     return;
        // }
        // navigate(-1);
    };

    return (
        <>
            <Grid container flexDirection={"column"} alignItems={"center"} gap={"20px"} mt={"20px"} pb={"20px"}>
                <Grid container justifyContent={"center"}>
                    <Typography variant="h4">Create new media</Typography>
                </Grid>
                <Grid container item xs={6} mt={"-60px"} ml={"-10px"}>
                    <NavigateBack to={-1} label={"Back"} />
                </Grid>
                <Grid container item flexDirection={"column"} alignItems={"flex-start"} xs={6} rowGap={"20px"}>
                    <Grid container gap={"15px"}>
                        <TextField
                            label="Media name"
                            fullWidth
                            autoComplete="off"
                            value={newMedia.name}
                            onChange={(e) => setNewMedia({ ...newMedia, name: e.target.value })}
                        ></TextField>
                        <Grid container gap={"20px"}>
                            <FormControl style={{ width: "200px" }}>
                                <InputLabel id="media-type-label">Media type</InputLabel>
                                <Select
                                    labelId="media-type-label"
                                    label="Media type"
                                    value={newMedia.mediaType}
                                    onChange={(e) => setNewMedia({ ...newMedia, mediaType: e.target.value })}
                                >
                                    {["Movie", "Music", "Book"].map((type) => (
                                        <MenuItem key={type} value={type}>
                                            {type}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                            <TextField
                                label="Media creation year"
                                type="number"
                                autoComplete="off"
                                style={{ width: "200px" }}
                                value={newMedia.yearOfCreating}
                                onChange={(e) => setNewMedia({ ...newMedia, yearOfCreating: e.target.value })}
                            ></TextField>
                        </Grid>
                        <Grid container flexDirection={"column"} gap={"10px"}>
                            <Typography variant="h6">Authors</Typography>
                            {newMedia.authors.length > 0 && (
                                <Grid container gap={"10px"}>
                                    {newMedia.authors.map((author) => (
                                        <Chip
                                            key={author.firstName + author.lastName}
                                            label={author.firstName + " " + author.lastName}
                                            onDelete={() => removeAuthorFromNewMedia(author.firstName, author.lastName)}
                                        />
                                    ))}
                                </Grid>
                            )}
                            <AuthorAutocomplete
                                authors={availableAuthors}
                                addAuthor={addAuthorToNewMedia}
                                addNewAuthor={addNewAuthorToNewMedia}
                                displayError={displayError}
                            />
                        </Grid>
                    </Grid>
                    <Button variant="contained" fullWidth style={{ height: "40px", fontSize: "16px" }} onClick={createMedia}>
                        Create
                    </Button>
                </Grid>
            </Grid>
            <ErrorSnackbar />
        </>
    );
};

export default MediaCreationPage;
