import { Button, Grid, Typography } from "@mui/material";
import React from "react";
import moment from "moment";
import { useNavigate } from "react-router-dom";

const CatalogueItem = ({ mediaInfo }) => {
    const navigate = useNavigate();

    return (
        <Grid container wrap="nowrap" bgcolor={"#d3e2ff"} borderRadius={"15px"} padding={"10px 15px 10px 15px"}>
            <Grid container flexDirection={"column"}>
                <Typography variant="h5">{mediaInfo.name}</Typography>
                <Typography variant="h6">
                    {mediaInfo.mediaType}, {moment(mediaInfo.yearOfCreating).format("YYYY")}
                </Typography>
                <Typography>
                    {mediaInfo.authors.length > 0 ? (
                        mediaInfo.authors.map((author) => [author.firstName, author.lastName].join(" ")).join(", ")
                    ) : (
                        <i>No authors specified</i>
                    )}
                </Typography>
            </Grid>
            <Grid item>
                <Grid container flexDirection={"column"} justifyContent={"space-between"} minWidth={"140px"} height={"100%"}>
                    <Button variant="outlined">В избранное</Button>
                    <Button variant="outlined" onClick={() => navigate(`/media/${mediaInfo.mediaId}`)}>
                        Отзывы
                    </Button>
                </Grid>
            </Grid>
        </Grid>
    );
};

export default CatalogueItem;
