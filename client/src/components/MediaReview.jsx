import { Grid, Rating, Typography } from "@mui/material";
import React from "react";

const MediaReview = ({ review }) => {
    return (
        <Grid container gap={"10px"} style={{ backgroundColor: "#d3e2ff", borderRadius: "15px", padding: "10px 15px 10px 15px" }}>
            <Grid container justifyContent={"space-between"}>
                <Rating value={parseInt(review.rating)} readOnly />
                <Typography>{new Date(review.dateOfCreation).toDateString()}</Typography>
            </Grid>
            <Typography>{review.userReview}</Typography>
        </Grid>
    );
};

export default MediaReview;
