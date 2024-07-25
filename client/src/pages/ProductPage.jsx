import React, { useEffect, useState } from "react";
import { Chip, Grid, Typography } from "@mui/material";
import { useParams } from "react-router";
import useSnackbar from "../hooks/useSnackbar";
import useApiRequest from "../hooks/useApiRequest";
import MediaReview from "../components/MediaReview";
import NavigateBack from "../components/buttons/NavigateBack";

const ProductPage = ({ accessToken, refreshToken, setAccessToken, setRefreshToken }) => {
    const { id } = useParams();

    const { displayError, ErrorSnackbar } = useSnackbar();

    const sendRequest = useApiRequest(accessToken, refreshToken, setAccessToken, setRefreshToken);

    const [media, setMedia] = useState({
        mediaType: "",
        name: "",
        yearOfCreating: "",
        authors: [],
        reviews: [],
    });

    useEffect(() => {
        const fetchMedia = async () => {
            const response = await sendRequest(`media/${id}`, "get", {}, {});

            if (!response.ok) {
                displayError(response.error);
                return;
            }

            setMedia(response.data);
        };

        const fetchData = async () => {
            await fetchMedia();
        };

        fetchData();
    }, []);

    return (
        <>
            <Grid container flexDirection={"column"} alignItems={"center"} gap={"20px"} mt={"20px"} pb={"20px"}>
                <Grid container item xs={6} gap={2} ml={"-220px"}>
                    <NavigateBack to={-1} label={"Back"} />
                    <Typography variant="h4">Media info</Typography>
                </Grid>
                <Grid container item xs={6} flexDirection={"column"} gap={"15px"}>
                    <Grid item flexDirection={"column"}>
                        <Typography variant="subtitle1">Media name</Typography>
                        <Typography variant="h5" style={{ borderBottom: "1px solid black", paddingBottom: "3px" }}>
                            {media.name}
                        </Typography>
                    </Grid>
                    <Grid container gap={"50px"}>
                        <Grid item flexDirection={"column"} minWidth={"200px"}>
                            <Typography variant="subtitle1">Media type</Typography>
                            <Typography variant="h5" style={{ borderBottom: "1px solid black", paddingBottom: "3px" }}>
                                {media.mediaType}
                            </Typography>
                        </Grid>
                        <Grid item flexDirection={"column"}>
                            <Typography variant="subtitle1">Creation year</Typography>
                            <Typography variant="h5" style={{ borderBottom: "1px solid black", paddingBottom: "3px" }}>
                                {media.yearOfCreating ? new Date(media.yearOfCreating).getFullYear() : ""}
                            </Typography>
                        </Grid>
                    </Grid>
                    <Grid container flexDirection={"column"} gap={"5px"}>
                        <Typography variant="subtitle1">Authors</Typography>
                        <Grid container gap={"10px"} ml={"-3px"}>
                            {media.authors.map((author) => (
                                <Chip key={author.authorId} label={author.firstName + " " + author.lastName} />
                            ))}
                        </Grid>
                    </Grid>
                </Grid>
                <Grid container item xs={6} gap={"15px"}>
                    <Typography variant="h4">Reviews</Typography>
                </Grid>
                {media.reviews.length > 0 ? (
                    <Grid container item xs={6} flexDirection={"column"} gap={"15px"}>
                        {media.reviews.map((review) => (
                            <MediaReview key={review.reviewId} review={review} />
                        ))}
                    </Grid>
                ) : (
                    <Grid container item xs={6}>
                        <Typography>No reviews yet</Typography>
                    </Grid>
                )}
            </Grid>
            <ErrorSnackbar />
        </>
    );
};

export default ProductPage;
