import React, { useEffect, useState } from "react";
import {
    FormControl,
    Grid,
    Typography,
    Link,
    Rating,
    TextField,
    Button,
    Autocomplete,
} from "@mui/material";
import useApiRequest from "./../hooks/useApiRequest";
import useSnackbar from "./../hooks/useSnackbar";
import { useNavigate } from "react-router-dom";

const ReviewCreationPage = ({ accessToken, refreshToken, setAccessToken, setRefreshToken }) => {
    const navigate = useNavigate();

    const sendRequest = useApiRequest(accessToken, refreshToken, setAccessToken, setRefreshToken);

    const { displayError, displaySuccess, ErrorSnackbar, SuccessSnackbar } = useSnackbar();

    const [mediaCatalogue, setMediaCatalogue] = useState([]);

    const [review, setReview] = useState({
        rating: null,
        userReview: "",
        mediaToCreateDTO: null,
    });

    const [mediaInputValue, setMediaInputValue] = useState("");

    useEffect(() => {
        const fetchMediaCatalogue = async () => {
            const response = await sendRequest("media", "get", {}, {});

            if (!response.ok) {
                displayError(response.error);
                return;
            }

            setMediaCatalogue(response.data.media);
        };

        fetchMediaCatalogue();
    }, []);

    const createReview = async () => {
        console.log(review);

        if (review.rating == null || review.userReview.trim().length === 0 || review.mediaToCreateDTO == null) {
            displayError("Fill the review information");
            return;
        }

        const response = await sendRequest("review/CreateReview", "post", review, {});

        if (!response.ok) {
            displayError(response.error);
            return;
        }

        displaySuccess("Review created");
        navigate(-1);
    };

    return (
        <>
            <Grid container flexDirection={"column"} alignItems={"center"} gap={"20px"} mt={"20px"} pb={"20px"}>
                <Grid container justifyContent={"center"}>
                    <Typography variant="h4">Create new review</Typography>
                </Grid>
                <Grid container item flexDirection={"column"} alignItems={"flex-start"} xs={6} rowGap={"20px"}>
                    <FormControl fullWidth>
                        <Autocomplete
                            value={review.mediaToCreateDTO}
                            onChange={(event, newValue) => {
                                setReview({ ...review, mediaToCreateDTO: newValue });
                            }}
                            inputValue={mediaInputValue}
                            onInputChange={(event, newInputValue) => {
                                setMediaInputValue(newInputValue);
                            }}
                            options={mediaCatalogue}
                            getOptionLabel={(m) => m.name}
                            fullWidth
                            renderInput={(params) => <TextField {...params} label="Select media from catalogue" />}
                        />

                        <Link
                            variant="body2"
                            display={"block"}
                            onClick={(e) => {
                                e.preventDefault();
                            }}
                            sx={{ cursor: "pointer", userSelect: "none", textDecoration: "none", mt: "5px" }}
                        >
                            Your media is not in the list?
                        </Link>
                    </FormControl>
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
            <SuccessSnackbar />
        </>
    );
};

export default ReviewCreationPage;
