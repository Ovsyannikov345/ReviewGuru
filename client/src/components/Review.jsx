import { Grid, Rating, Typography } from "@mui/material";
import React from "react";

const Review = ({ review }) => {
    return (
        <Grid container gap={"10px"} style={{ backgroundColor: "#d3e2ff", borderRadius: "15px", padding: "10px 15px 10px 15px" }}>
            <Grid container justifyContent={"space-between"} wrap="nowrap">
                <Typography style={{ maxWidth: "70%" }}>
                    <b>
                        {review.media.name} ({review.media.mediaType}, {new Date(review.media.yearOfCreating).getFullYear()})
                    </b>
                </Typography>
                <Typography style={{ minWidth: "130px" }}>{new Date(review.dateOfCreation).toDateString()}</Typography>
            </Grid>
            <Grid container mt={"-5px"}>
                <Typography>
                    {review.media.authors.length <= 3
                        ? review.media.authors.map((a) => a.firstName + " " + a.lastName).join(", ")
                        : review.media.authors
                              .slice(0, 3)
                              .map((a) => a.firstName + " " + a.lastName)
                              .join(", ") + ` + ${review.media.authors.length - 3} others`}
                </Typography>
            </Grid>
            <Grid container mt={"10px"}>
                <Rating value={parseInt(review.rating)} readOnly style={{ marginLeft: "-5px" }} />
            </Grid>
            <Typography>{review.userReview}</Typography>
        </Grid>
    );
};

export default Review;
