import React, { useEffect, useMemo, useState } from "react";
import {
    FormControl,
    Grid,
    Typography,
    Link,
    Rating,
    TextField,
    Button,
    Select,
    InputLabel,
    MenuItem,
    Chip,
    Autocomplete,
} from "@mui/material";
import useApiRequest from "./../hooks/useApiRequest";
import useSnackbar from "./../hooks/useSnackbar";
import { useNavigate } from "react-router-dom";
import AuthorAutocomplete from "../components/AuthorAutocomplete";
import NavigateBack from "./../components/buttons/NavigateBack";

const ReviewCreationPage = ({ accessToken, refreshToken, setAccessToken, setRefreshToken }) => {
    const navigate = useNavigate();

    const sendRequest = useApiRequest(accessToken, refreshToken, setAccessToken, setRefreshToken);

    const { displayError, ErrorSnackbar } = useSnackbar();

    const [mediaCatalogue, setMediaCatalogue] = useState([]);

    const [review, setReview] = useState({
        rating: null,
        userReview: "",
        mediaToCreateDTO: null,
    });

    const [mediaInputValue, setMediaInputValue] = useState("");

    const [newMediaMode, setNewMediaMode] = useState(false);

    const [newMedia, setNewMedia] = useState({
        mediaType: "",
        name: "",
        yearOfCreating: "",
        authors: [],
    });

    const [authors, setAuthors] = useState([]);

    const sortedMedia = useMemo(() => {
        return mediaCatalogue.sort((a, b) => {
            if (a.mediaType > b.mediaType) {
                return 1;
            }

            if (a.mediaType < b.mediaType) {
                return -1;
            }

            if (a.name > b.name) {
                return 1;
            }

            return -1;
        });
    }, [mediaCatalogue]);

    const availableAuthors = useMemo(() => {
        const availableAuthors = authors.filter((author) => !newMedia.authors.includes(author));

        availableAuthors.forEach((author) => {
            author.groupLetter = author.firstName[0];
        });

        availableAuthors.sort((a, b) => a.groupLetter.localeCompare(b.groupLetter));

        return availableAuthors;
    }, [authors, newMedia.authors]);

    useEffect(() => {
        const fetchMediaCatalogue = async () => {
            const response = await sendRequest("media", "get", {}, { pageSize: 2000000000 });

            if (!response.ok) {
                displayError(response.error);
                return;
            }

            setMediaCatalogue(response.data.media);
        };

        const fetchAuthors = async () => {
            const response = await sendRequest("authors", "get", {}, {});

            if (!response.ok) {
                displayError(response.error);
                return;
            }

            setAuthors(response.data);
        };

        const fetchData = async () => {
            await fetchMediaCatalogue();
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

    const createReview = async () => {
        if (review.rating == null || review.userReview.trim().length === 0) {
            displayError("Fill the review information");
            return;
        }

        if (!newMediaMode) {
            if (review.mediaToCreateDTO == null) {
                displayError("Fill the review information");
                return;
            }

            const response = await sendRequest("review/CreateReview", "post", review, {});

            if (!response.ok) {
                displayError(response.error);
                return;
            }

            navigate(-1);
            return;
        }

        if (newMedia.mediaType === "") {
            displayError("Select media type");
            return;
        }

        if (!newMedia.name) {
            displayError("Fill media name");
            return;
        }

        if (
            newMedia.yearOfCreating &&
            (Number.isNaN(parseInt(newMedia.yearOfCreating)) ||
                newMedia.yearOfCreating < 1 ||
                newMedia.yearOfCreating > new Date().getFullYear())
        ) {
            displayError("Invalid media creation year");
            return;
        }

        if (newMedia.mediaType === "Movie") {
            const response = await sendRequest(
                "OMDb/CreateReview",
                "post",
                {
                    rating: review.rating,
                    userReview: review.userReview,
                    mediaName: newMedia.name,
                    yearOfMediaCreation: newMedia.yearOfCreating ? newMedia.yearOfCreating : null,
                },
                {}
            );

            if (response.ok) {
                navigate(-1);
                return;
            }

            if (response.status === 400) {
                displayError(response.error);
                return;
            }
        }

        if (!newMedia.yearOfCreating) {
            displayError("Enter media creation year");
            return;
        }

        const reviewData = {
            ...review,
            mediaToCreateDTO: {
                ...newMedia,
                yearOfCreating: `${newMedia.yearOfCreating}-01-01`,
                authorsToCreateDTO: newMedia.authors.map((author) => ({
                    authorId: author.authorId,
                    firstName: author.firstName,
                    lastName: author.lastName,
                })),
            },
        };

        const response = await sendRequest("review/CreateReview", "post", reviewData, {});

        if (!response.ok) {
            displayError(response.error);
            return;
        }

        navigate(-1);
    };

    return (
        <>
            <Grid container flexDirection={"column"} alignItems={"center"} gap={"20px"} mt={"20px"} pb={"20px"}>
                <Grid container justifyContent={"center"}>
                    <Typography variant="h4">Create new review</Typography>
                </Grid>
                <Grid container item xs={6} mt={"-60px"} ml={"-10px"}>
                    <NavigateBack to={-1} label={"Back"} />
                </Grid>
                <Grid container item flexDirection={"column"} alignItems={"flex-start"} xs={6} rowGap={"20px"}>
                    {!newMediaMode ? (
                        <FormControl fullWidth>
                            <Typography variant="h6">Media</Typography>
                            <Link
                                variant="body2"
                                display={"block"}
                                onClick={(e) => {
                                    e.preventDefault();
                                    setNewMediaMode(true);
                                }}
                                sx={{ cursor: "pointer", userSelect: "none", textDecoration: "none", mt: "5px", mb: "5px" }}
                            >
                                Your media is not in the list?
                            </Link>
                            <Autocomplete
                                value={review.mediaToCreateDTO}
                                onChange={(event, newValue) => {
                                    setReview({ ...review, mediaToCreateDTO: newValue });
                                }}
                                inputValue={mediaInputValue}
                                onInputChange={(event, newInputValue) => {
                                    setMediaInputValue(newInputValue);
                                }}
                                options={sortedMedia}
                                getOptionLabel={(m) => m.name}
                                fullWidth
                                renderInput={(params) => <TextField {...params} label="Select media from catalogue" />}
                                groupBy={(m) => m.mediaType}
                                style={{ marginTop: "5px" }}
                            />
                        </FormControl>
                    ) : (
                        <Grid container gap={"15px"}>
                            <Grid container>
                                <Typography variant="h6">New media</Typography>
                            </Grid>
                            <Link
                                variant="body2"
                                display={"block"}
                                onClick={(e) => {
                                    e.preventDefault();
                                    setNewMediaMode(false);
                                }}
                                sx={{ cursor: "pointer", userSelect: "none", textDecoration: "none", mt: "-10px", mb: "-5px" }}
                            >
                                Select media from catalogue
                            </Link>
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
                    )}
                    <Grid container flexDirection={"column"}>
                        <Typography variant="h6">Your grade</Typography>
                        <Rating
                            id="grade"
                            name="grade"
                            sx={{
                                fontSize: "40px",
                                ml: "-5px",
                                mt: "5px",
                            }}
                            value={review.rating}
                            onChange={(e, newValue) => {
                                if (newValue != null) {
                                    setReview({ ...review, rating: newValue });
                                }
                            }}
                        />
                    </Grid>
                    <Grid container flexDirection={"column"}>
                        <Typography variant="h6">Your opinion</Typography>
                        <TextField
                            multiline
                            minRows={3}
                            placeholder="Write your review here"
                            autoComplete="off"
                            style={{ marginTop: "5px" }}
                            value={review.userReview}
                            onChange={(e) => setReview({ ...review, userReview: e.target.value })}
                        />
                        <Typography
                            variant="subtitle2"
                            alignSelf={"flex-end"}
                            color={review.userReview.length > 5000 ? "error" : "inherit"}
                        >
                            {review.userReview.length}/5000
                        </Typography>
                    </Grid>
                    <Button variant="contained" fullWidth style={{ height: "40px", fontSize: "16px" }} onClick={createReview}>
                        Create
                    </Button>
                </Grid>
            </Grid>
            <ErrorSnackbar />
        </>
    );
};

export default ReviewCreationPage;
