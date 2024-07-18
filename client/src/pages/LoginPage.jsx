import React, { useState } from "react";
import { TextField, Button, Link, Grid, Typography, CircularProgress } from "@mui/material";
import { useNavigate } from "react-router-dom";
import useApiRequest from "../hooks/useApiRequest";
import useSnackbar from "../hooks/useSnackbar";

const LoginPage = ({ accessToken, refreshToken, setAccessToken, setRefreshToken }) => {
    const navigate = useNavigate();

    const sendRequest = useApiRequest(accessToken, refreshToken, setAccessToken, setRefreshToken);

    const { displayError, ErrorSnackbar } = useSnackbar();

    const [loading, setLoading] = useState(false);

    const [authData, setAuthData] = useState({
        login: "",
        password: "",
    });

    const loginUser = async () => {
        if (!authData.login || !authData.password) {
            displayError("Enter form data");
            return;
        }

        setLoading(true);

        const response = await sendRequest("auth/login", "post", authData);

        if (!response.ok) {
            displayError(response.error);
            setLoading(false);
            return;
        }

        setAccessToken(response.data.accessToken);
        setRefreshToken(response.data.refreshToken);
        navigate("/catalogue");
    };

    return (
        <>
            <Grid
                container
                item
                flexDirection={"column"}
                justifyContent="center"
                alignItems="center"
                position={"absolute"}
                height={"80%"}
            >
                <Grid container item xs={12} sm={6} md={4} xl={3} gap={2} maxWidth={"480px"}>
                    <Typography variant="h4" width={"100%"} textAlign={"center"}>
                        Log in
                    </Typography>
                    <form
                        onSubmit={(e) => {
                            e.preventDefault();
                            loginUser();
                        }}
                        style={{ width: "100%" }}
                    >
                        <TextField
                            id="login"
                            label="Login"
                            type="text"
                            variant="outlined"
                            margin="normal"
                            fullWidth
                            value={authData.login}
                            onChange={(e) => setAuthData({ ...authData, login: e.target.value })}
                        />
                        <TextField
                            id="password"
                            label="Password"
                            type="password"
                            variant="outlined"
                            margin="normal"
                            fullWidth
                            value={authData.password}
                            onChange={(e) => setAuthData({ ...authData, password: e.target.value })}
                        />
                        <Link
                            variant="body2"
                            display={"block"}
                            onClick={(e) => {
                                e.preventDefault();
                                navigate("/register");
                            }}
                            sx={{ cursor: "pointer", userSelect: "none", textDecoration: "none" }}
                        >
                            Not with us yet? &#x1F633; - Register!
                        </Link>
                        <Button
                            disabled={loading}
                            type="submit"
                            fullWidth
                            variant="contained"
                            color="primary"
                            style={{ marginTop: "20px" }}
                        >
                            {!loading ? "Log in" : <CircularProgress color="primary" size={"30px"} />}
                        </Button>
                    </form>
                </Grid>
            </Grid>
            <ErrorSnackbar />
        </>
    );
};

export default LoginPage;
