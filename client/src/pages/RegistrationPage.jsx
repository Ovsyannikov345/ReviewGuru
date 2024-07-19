import React, { useState } from "react";
import { TextField, Button, Link, Grid, Typography, CircularProgress } from "@mui/material";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { useNavigate } from "react-router-dom";
import { useFormik } from "formik";
import moment from "moment";
import validateUser from "./../utils/validators/validateUser";
import useApiRequest from "../hooks/useApiRequest";
import useSnackbar from "../hooks/useSnackbar";
import NavigateBack from "./../components/buttons/NavigateBack";

const RegistrationPage = ({ accessToken, refreshToken, setAccessToken, setRefreshToken }) => {
    const navigate = useNavigate();

    const sendRequest = useApiRequest(accessToken, refreshToken, setAccessToken, setRefreshToken);

    const { displayError, ErrorSnackbar } = useSnackbar();

    const [loading, setLoading] = useState(false);

    const registerUser = async (userData) => {
        setLoading(true);

        if (userData.dateOfBirth && !userData.dateOfBirth._isValid) {
            userData.dateOfBirth = null;
        }

        const { passwordDuplicate, ...dataToSend } = userData;

        if (userData.dateOfBirth) {
            dataToSend.dateOfBirth = dataToSend.dateOfBirth.format("YYYY-MM-DDT00:00:00.000") + "Z";
        }

        const response = await sendRequest("auth/register", "post", dataToSend);

        if (!response.ok) {
            displayError(response.error);
            setLoading(false);
            return;
        }

        setAccessToken(response.data.accessToken);
        setRefreshToken(response.data.refreshToken);
        navigate("/catalogue");
    };

    const formik = useFormik({
        initialValues: {
            login: "",
            password: "",
            passwordDuplicate: "",
            email: "",
            dateOfBirth: null,
        },
        validate: validateUser,
        onSubmit: (values) => registerUser(values),
    });

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
                    <NavigateBack to={"/catalogue"} label={"Catalogue"} />
                    <Typography variant="h4" width={"100%"} textAlign={"center"}>
                        Register
                    </Typography>
                    <form onSubmit={formik.handleSubmit} style={{ width: "100%" }}>
                        <TextField
                            id="login"
                            label="Login"
                            type="text"
                            variant="outlined"
                            margin="normal"
                            fullWidth
                            value={formik.values.login}
                            onChange={(e) => {
                                formik.setFieldTouched("login", false);
                                formik.handleChange(e);
                            }}
                            error={formik.touched.login && formik.errors.login !== undefined}
                            helperText={formik.touched.login && formik.errors.login !== undefined ? formik.errors.login : ""}
                        />
                        <TextField
                            id="password"
                            label="Password"
                            type="password"
                            variant="outlined"
                            margin="normal"
                            fullWidth
                            value={formik.values.password}
                            onChange={(e) => {
                                formik.setFieldTouched("password", false);
                                formik.handleChange(e);
                            }}
                            error={formik.touched.password && formik.errors.password !== undefined}
                            helperText={
                                formik.touched.password && formik.errors.password !== undefined ? formik.errors.password : ""
                            }
                        />
                        <TextField
                            id="passwordDuplicate"
                            label="Repeat the password"
                            type="password"
                            variant="outlined"
                            margin="normal"
                            fullWidth
                            value={formik.values.passwordDuplicate}
                            onChange={(e) => {
                                formik.setFieldTouched("passwordDuplicate", false);
                                formik.handleChange(e);
                            }}
                            onBlur={formik.handleBlur}
                            error={formik.touched.passwordDuplicate && formik.errors.passwordDuplicate !== undefined}
                            helperText={
                                formik.touched.passwordDuplicate && formik.errors.passwordDuplicate !== undefined
                                    ? formik.errors.passwordDuplicate
                                    : ""
                            }
                        />
                        <TextField
                            id="email"
                            label="Email"
                            type="text"
                            variant="outlined"
                            margin="normal"
                            fullWidth
                            value={formik.values.email}
                            onChange={(e) => {
                                formik.setFieldTouched("email", false);
                                formik.handleChange(e);
                            }}
                            error={formik.touched.email && formik.errors.email !== undefined}
                            helperText={formik.touched.email && formik.errors.email !== undefined ? formik.errors.email : ""}
                        />
                        <DatePicker
                            sx={{ width: "100%", marginTop: "16px", marginBottom: "8px" }}
                            label="Date of birth"
                            disableFuture
                            minDate={moment().add(-100, "year")}
                            value={formik.values.dateOfBirth}
                            onChange={(newDate) => {
                                formik.setFieldValue("dateOfBirth", moment(newDate));
                                formik.setFieldTouched("dateOfBirth", false);
                            }}
                        />
                        <Link
                            variant="body2"
                            display={"block"}
                            onClick={(e) => {
                                e.preventDefault();
                                navigate("/login");
                            }}
                            sx={{ cursor: "pointer", userSelect: "none", textDecoration: "none" }}
                        >
                            Already with us? &#129303; - Log in!
                        </Link>
                        <Button
                            disabled={loading}
                            type="submit"
                            fullWidth
                            variant="contained"
                            color="primary"
                            style={{ marginTop: "20px" }}
                        >
                            {!loading ? "Register" : <CircularProgress color="primary" size={"30px"} />}
                        </Button>
                    </form>
                </Grid>
            </Grid>
            <ErrorSnackbar />
        </>
    );
};

export default RegistrationPage;
