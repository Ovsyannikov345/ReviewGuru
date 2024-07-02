import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Box, AppBar, Toolbar, Grid, Menu, IconButton, Tooltip, MenuItem, ListItemIcon } from "@mui/material";
import LogoutIcon from "@mui/icons-material/Logout";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import RateReviewIcon from "@mui/icons-material/RateReview";
import EditIcon from "@mui/icons-material/Edit";
import PasswordIcon from "@mui/icons-material/Password";
import LoginIcon from "@mui/icons-material/Login";
import Logo from "../images/logo-black.png";

const NavBar = () => {
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

    return (
        <>
            <Box>
                <AppBar position="static">
                    <Toolbar style={{ paddingLeft: "100px", paddingRight: "100px" }}>
                        <Grid container alignItems={"center"} justifyContent={"space-between"}>
                            <Grid item mt={"5px"} mb={"5px"}>
                                <img
                                    src={Logo}
                                    alt="Review Guru"
                                    style={{ maxWidth: "200px", height: "auto", borderRadius: "10px" }}
                                />
                            </Grid>
                            <Grid item>
                                <Grid container gap={"10px"}>
                                    {accessToken ? (
                                        <>
                                            <Tooltip title="Действия">
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
                                            <Tooltip title="Избранное">
                                                <IconButton size="large" color="inherit">
                                                    <FavoriteBorderIcon fontSize="large" />
                                                </IconButton>
                                            </Tooltip>
                                            <Tooltip title="Выход">
                                                <IconButton size="large" color="inherit">
                                                    <LogoutIcon fontSize="large" />
                                                </IconButton>
                                            </Tooltip>
                                        </>
                                    ) : (
                                        <Tooltip title="Авторизоваться">
                                            <IconButton
                                                size="large"
                                                color="inherit"
                                                onClick={(event) => {
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
                    Мои отзывы
                </MenuItem>
                <MenuItem key={2}>
                    <ListItemIcon>
                        <EditIcon fontSize="small" />
                    </ListItemIcon>
                    Редактировать профиль
                </MenuItem>
                <MenuItem key={3}>
                    <ListItemIcon>
                        <PasswordIcon fontSize="small" />
                    </ListItemIcon>
                    Сменить пароль
                </MenuItem>
            </Menu>
        </>
    );
};

export default NavBar;
