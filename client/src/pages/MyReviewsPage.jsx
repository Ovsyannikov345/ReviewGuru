import React, { useEffect, useMemo, useState } from "react";
import useSnackbar from "../hooks/useSnackbar";
import useApiRequest from "../hooks/useApiRequest";
import { Grid, Pagination, Rating, TextField, Typography } from "@mui/material";
import NavigateBack from "../components/buttons/NavigateBack";
import Selector from "../components/Selector";
import { MEDIA_TYPES, REVIEWS_PER_PAGE } from "../utils/consts";
import Review from "../components/Review";

const MyReviewsPage = ({ accessToken, refreshToken, setAccessToken, setRefreshToken }) => {
    const { displayError, ErrorSnackbar } = useSnackbar();

    const sendRequest = useApiRequest(accessToken, refreshToken, setAccessToken, setRefreshToken);

    const [reviews, setReviews] = useState([]);

    const [totalReviewsCount, setTotalReviewsCount] = useState(1);

    const [reviewQuery, setReviewQuery] = useState({
        pageNumber: 1,
        pageSize: REVIEWS_PER_PAGE,
        mediaType: MEDIA_TYPES[0].value,
        searchText: "",
        minRating: null,
        maxRating: null,
    });

    const pagesCount = useMemo(() => {
        var count = Math.floor(totalReviewsCount / REVIEWS_PER_PAGE);

        if (totalReviewsCount % REVIEWS_PER_PAGE !== 0) {
            count++;
        }

        return count;
    }, [totalReviewsCount]);

    useEffect(() => {
        const fetchReviews = async () => {
            const { minRating, maxRating, mediaType, ...query } = reviewQuery;

            if (mediaType !== "All") {
                query.mediaType = mediaType;
            }

            if (minRating) {
                query.minRating = minRating;
            }

            if (maxRating) {
                query.maxRating = maxRating;
            }

            const response = await sendRequest("review/MyReviews", "get", {}, query);

            if (!response.ok) {
                displayError(response.error);
                return;
            }

            setReviews(response.data.reviews);
            setTotalReviewsCount(response.data.totalReviewsCount);
        };

        const fetchData = async () => {
            await fetchReviews();
        };

        fetchData();
    }, [reviewQuery]);

    const changePage = (event, value) => {
        setReviewQuery({ ...reviewQuery, pageNumber: value });
        window.scrollTo({ top: 0, behavior: "smooth" });
    };

    return (
        <>
            <Grid container flexDirection={"column"} alignItems={"center"} gap={"20px"} mt={"20px"} pb={"20px"}>
                <Grid container item xs={6} gap={2} ml={"-220px"}>
                    <NavigateBack to={-1} label={"Back"} />
                    <Typography variant="h4">Your reviews</Typography>
                </Grid>
                <Grid container item alignItems={"center"} columnGap={"15px"} wrap="nowrap" xs={6}>
                    <Grid item>
                        <Selector
                            options={MEDIA_TYPES}
                            value={reviewQuery.mediaType}
                            changeHandler={(value) => setReviewQuery({ ...reviewQuery, mediaType: value })}
                        />
                    </Grid>
                    <Grid container alignItems={"stretch"} wrap="nowrap" columnGap={"10px"}>
                        <TextField
                            fullWidth
                            placeholder="Type the text to search"
                            value={reviewQuery.searchText}
                            onChange={(e) => setReviewQuery({ ...reviewQuery, searchText: e.target.value })}
                        />
                    </Grid>
                </Grid>
                <Grid container item xs={6} gap={"50px"}>
                    <Grid item flexDirection="column">
                        <Typography>Min rating</Typography>
                        <Rating
                            size="large"
                            style={{ marginLeft: "-5px", marginTop: "5px" }}
                            value={reviewQuery.minRating}
                            onChange={(event, newValue) => {
                                if (!newValue) {
                                    setReviewQuery({ ...reviewQuery, minRating: null });
                                    return;
                                }

                                if (!reviewQuery.maxRating || reviewQuery.maxRating >= newValue) {
                                    setReviewQuery({ ...reviewQuery, minRating: newValue });
                                    return;
                                }

                                displayError("Min rating should be lower than max rating");
                            }}
                        />
                    </Grid>
                    <Grid item flexDirection="column">
                        <Typography>Max rating</Typography>
                        <Rating
                            size="large"
                            style={{ marginLeft: "-5px", marginTop: "5px" }}
                            value={reviewQuery.maxRating}
                            onChange={(event, newValue) => {
                                if (!newValue) {
                                    setReviewQuery({ ...reviewQuery, maxRating: null });
                                    return;
                                }

                                if (!reviewQuery.minRating || reviewQuery.minRating <= newValue) {
                                    setReviewQuery({ ...reviewQuery, maxRating: newValue });
                                    return;
                                }

                                displayError("Max rating should be higher than min rating");
                            }}
                        />
                    </Grid>
                </Grid>
                <Grid container item xs={6} justifyContent={"center"} rowGap={"10px"}>
                    {reviews.length > 0 ? (
                        <>
                            {reviews.map((review) => (
                                <Review key={review.reviewId} review={review} />
                            ))}
                            <Pagination
                                variant="outlined"
                                shape="rounded"
                                color="primary"
                                count={pagesCount}
                                page={reviewQuery.page}
                                onChange={changePage}
                                style={{ marginTop: "10px" }}
                            />
                        </>
                    ) : (
                        <Typography>No reviews found</Typography>
                    )}
                </Grid>
            </Grid>
            <ErrorSnackbar />
        </>
    );
};

export default MyReviewsPage;
