import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
    Box,
    AppBar,
    Toolbar,
    Grid,
    Menu,
    IconButton,
    Tooltip,
    MenuItem,
    ListItemIcon,
    Snackbar,
    Alert,
    Button,
} from "@mui/material";
import LogoutIcon from "@mui/icons-material/Logout";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import RateReviewIcon from "@mui/icons-material/RateReview";
import EditIcon from "@mui/icons-material/Edit";
import PasswordIcon from "@mui/icons-material/Password";
import LoginIcon from "@mui/icons-material/Login";
import Logo from "../images/logo-black.png";
import { sendLogoutRequest } from "../api/authApi";
import { CATALOGUE_ROUTE, FAVOURITES_ROUTE } from "../utils/consts";

const NavBar = () => {
    const [error, setError] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [success, setSuccess] = useState(false);
    const [successMessage, setSuccessMessage] = useState("");

    const displayError = (message) => {
        closeSnackbar();
        setErrorMessage(message);
        setError(true);
    };

    const displaySuccess = (message) => {
        closeSnackbar();
        setSuccessMessage(message);
        setSuccess(true);
    };

    const closeSnackbar = (event, reason) => {
        if (reason === "clickaway") {
            return;
        }

        setSuccess(false);
        setError(false);
    };

    const [accessToken, setAccessToken] = useState(localStorage.getItem("accessToken"));

    const [anchorEl, setAnchorEl] = useState(null);

    const navigate = useNavigate();

    useEffect(() => {
        const iframe = document.createElement("iframe");
        iframe.style.display = "none";

        document.body.appendChild(iframe);
        iframe.contentWindow.addEventListener("storage", () => {
            setAccessToken(localStorage.getItem("accessToken"));
        });

        return () => {
            iframe.contentWindow.removeEventListener("storage", () => {
                setAccessToken(localStorage.getItem("accessToken"));
            });
            document.body.removeChild(iframe);
        };
    }, []);

    const logout = async () => {
        const response = await sendLogoutRequest();

        if (!response || response.data.statusCode >= 400) {
            if (response) {
                displayError(response.data.message);
            }

            return;
        }

        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        displaySuccess("Logged out");
        navigate("/catalogue");
    };

    return (
        <>
            <Box>
                <AppBar position="static">
                    <Toolbar style={{ paddingLeft: "100px", paddingRight: "100px" }}>
                        <Grid container alignItems={"center"} justifyContent={"space-between"}>
                            <Grid item mt={"5px"} mb={"5px"}>
                                <Grid container gap={"60px"} alignItems={"center"}>
                                    <img
                                        src={Logo}
                                        alt="Review Guru"
                                        style={{ maxWidth: "200px", height: "auto", borderRadius: "10px" }}
                                    />
                                    <Button
                                        variant="text"
                                        color="inherit"
                                        style={{ fontSize: "18px", borderRadius: "0", borderBottom: "1px solid white" }}
                                        onClick={() => navigate(CATALOGUE_ROUTE)}
                                    >
                                        Catalogue
                                    </Button>
                                </Grid>
                            </Grid>
                            <Grid item>
                                <Grid container gap={"10px"}>
                                    {accessToken ? (
                                        <>
                                            <Tooltip title="Actions">
                                                <IconButton
                                                    size="large"
                                                    color="inherit"
                                                    onClick={(event) => {
                                                        setAnchorEl(event.currentTarget);
                                                    }}
                                                >
                                                    <AccountCircleIcon fontSize="large" />
                                                </IconButton>
                                            </Tooltip>
                                            <Tooltip title="Favorites">
                                                <IconButton
                                                    size="large"
                                                    color="inherit"
                                                    onClick={() => navigate(FAVOURITES_ROUTE)}
                                                >
                                                    <FavoriteBorderIcon fontSize="large" />
                                                </IconButton>
                                            </Tooltip>
                                            <Tooltip title="Log out">
                                                <IconButton size="large" color="inherit" onClick={logout}>
                                                    <LogoutIcon fontSize="large" />
                                                </IconButton>
                                            </Tooltip>
                                        </>
                                    ) : (
                                        <Tooltip title="Log in">
                                            <IconButton
                                                size="large"
                                                color="inherit"
                                                onClick={() => {
                                                    navigate("/login");
                                                }}
                                            >
                                                <LoginIcon fontSize="large" />
                                            </IconButton>
                                        </Tooltip>
                                    )}
                                </Grid>
                            </Grid>
                        </Grid>
                    </Toolbar>
                </AppBar>
            </Box>
            <Menu
                id="menu"
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={() => setAnchorEl(null)}
                transformOrigin={{ horizontal: "right", vertical: "top" }}
                anchorOrigin={{ horizontal: "right", vertical: "bottom" }}
            >
                <MenuItem key={1}>
                    <ListItemIcon>
                        <RateReviewIcon fontSize="small" />
                    </ListItemIcon>
                    My reviews
                </MenuItem>
                <MenuItem key={2}>
                    <ListItemIcon>
                        <EditIcon fontSize="small" />
                    </ListItemIcon>
                    Edit profile
                </MenuItem>
                <MenuItem key={3}>
                    <ListItemIcon>
                        <PasswordIcon fontSize="small" />
                    </ListItemIcon>
                    Change password
                </MenuItem>
            </Menu>
            <Snackbar open={error} autoHideDuration={6000} onClose={closeSnackbar}>
                <Alert onClose={closeSnackbar} severity="error" sx={{ width: "100%" }}>
                    {errorMessage}
                </Alert>
            </Snackbar>
            <Snackbar open={success} autoHideDuration={6000} onClose={closeSnackbar}>
                <Alert onClose={closeSnackbar} severity="success" sx={{ width: "100%" }}>
                    {successMessage}
                </Alert>
            </Snackbar>
        </>
    );
};

export default NavBar;
