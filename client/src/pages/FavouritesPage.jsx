import React, { useEffect, useMemo, useState } from "react";
import { Button, Grid, Pagination, TextField, Typography } from "@mui/material";
import CatalogueItem from "../components/CatalogueItem";
import { sendFavoritesGetRequest } from "../api/mediaApi";
import { MEDIA_PER_PAGE, MEDIA_TYPES } from "../utils/consts";
import useSnackbar from "./../hooks/useSnackbar";
import Selector from "../components/Selector";
import SearchIcon from "@mui/icons-material/Search";

const FavouritesPage = () => {
    const { displayError, ErrorSnackbar, SuccessSnackbar } = useSnackbar();

    const [mediaQuery, setMediaQuery] = useState({
        page: 1,
        mediaType: MEDIA_TYPES[0].value,
        searchText: "",
    });

    const [userText, setUserText] = useState("");

    const [mediaData, setMediaData] = useState({
        totalMediaCount: 1,
        media: [],
    });

    const pagesCount = useMemo(() => {
        var count = Math.floor(mediaData.totalMediaCount / MEDIA_PER_PAGE);

        if (mediaData.totalMediaCount % MEDIA_PER_PAGE !== 0) {
            count++;
        }

        return count;
    }, [mediaData.totalMediaCount]);

    useEffect(() => {
        const fetchEvents = async () => {
            const response = await sendFavoritesGetRequest(
                mediaQuery.page,
                mediaQuery.mediaType === "All" ? "" : mediaQuery.mediaType,
                mediaQuery.searchText
            );

            if (response?.data?.statusCode >= 400) {
                displayError(response.data.message);
                return;
            }

            setMediaData(response.data);
        };

        fetchEvents();
    }, [mediaQuery]);

    const changePage = (event, value) => {
        setMediaQuery({ ...mediaQuery, page: value });
        window.scrollTo({ top: 0, behavior: "smooth" });
    };

    return (
        <>
            <Grid container flexDirection={"column"} alignItems={"center"} gap={"20px"} mt={"20px"} pb={"20px"}>
                <Grid container xs={6}>
                    <Typography variant="h4">Your favorite media</Typography>
                </Grid>
                <Grid container item alignItems={"center"} columnGap={"15px"} wrap="nowrap" xs={6}>
                    <Grid item>
                        <Selector
                            options={MEDIA_TYPES}
                            value={mediaQuery.mediaType}
                            changeHandler={(value) => setMediaQuery({ ...mediaQuery, mediaType: value })}
                        />
                    </Grid>
                    <Grid container alignItems={"stretch"} wrap="nowrap" columnGap={"10px"}>
                        <TextField
                            fullWidth
                            placeholder="Type the text to search"
                            value={userText}
                            onChange={(e) => setUserText(e.target.value)}
                        />
                        <Button
                            variant="contained"
                            style={{ width: "100px" }}
                            onClick={() => setMediaQuery({ ...mediaQuery, searchText: userText })}
                        >
                            <SearchIcon style={{ fontSize: "30px" }} />
                        </Button>
                    </Grid>
                </Grid>
                {mediaData.media.length > 0 ? (
                    <>
                        <Grid container item xs={6} rowGap={"10px"}>
                            {mediaData.media.map((media) => (
                                <CatalogueItem key={media.mediaId} mediaInfo={media} />
                            ))}
                        </Grid>
                        <Pagination
                            variant="outlined"
                            shape="rounded"
                            color="primary"
                            count={pagesCount}
                            page={mediaQuery.page}
                            onChange={changePage}
                        />
                    </>
                ) : (
                    <Typography>No media found</Typography>
                )}
            </Grid>
            <ErrorSnackbar />
            <SuccessSnackbar />
        </>
    );
};

export default FavouritesPage;
