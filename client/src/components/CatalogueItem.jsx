import { Button, Grid, IconButton, Typography } from "@mui/material";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import FavoriteIcon from "@mui/icons-material/Favorite";
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
                <Grid
                    container
                    flexDirection={"column"}
                    justifyContent={"space-between"}
                    alignItems={"flex-end"}
                    minWidth={"160px"}
                    height={"100%"}
                >
                    <IconButton style={{ marginTop: "-5px", marginRight: "-10px" }}>
                        {!mediaInfo.isFavorite ? (
                            <FavoriteBorderIcon style={{ fontSize: "30px" }} />
                        ) : (
                            <FavoriteIcon style={{ fontSize: "30px" }} color="error" />
                        )}
                    </IconButton>
                    <Button
                        variant="contained"
                        onClick={() => navigate(`/media/${mediaInfo.mediaId}`)}
                        endIcon={<ArrowForwardIcon />}
                    >
                        Reviews
                    </Button>
                </Grid>
            </Grid>
        </Grid>
    );
};

export default CatalogueItem;