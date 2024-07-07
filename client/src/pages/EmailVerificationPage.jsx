import React, { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { Button, CircularProgress, Dialog, Grid, Typography } from "@mui/material";
import MailOutlineIcon from "@mui/icons-material/MailOutline";
import { sendVerificationRequest } from "../api/authApi";
import { CATALOGUE_ROUTE } from "./../utils/consts";

const EmailVerificationPage = () => {
    const navigate = useNavigate();

    const [searchParams, setSearchParams] = useSearchParams();

    const [status, setStatus] = useState({
        isLoading: false,
        isError: false,
        isSuccess: false,
    });

    const verifyEmail = async () => {
        setStatus({
            isLoading: true,
            isError: false,
            isSuccess: false,
        });

        const verificationToken = searchParams.get("token");

        setTimeout(async () => {
            const response = await sendVerificationRequest(verificationToken);

            if (response.data.statusCode >= 400) {
                setStatus({
                    isLoading: false,
                    isError: true,
                    isSuccess: false,
                });

                return;
            }

            setStatus({
                isLoading: false,
                isError: false,
                isSuccess: true,
            });
        }, 3000);
    };

    useEffect(() => {
        verifyEmail();
    }, []);

    return (
        <Dialog open={true} PaperProps={{ style: { padding: "10px 50px 15px 50px", minWidth: "230px", minHeight: "150px" } }}>
            <Grid container flexDirection={"column"} alignItems={"center"}>
                <MailOutlineIcon style={{ width: "60px", height: "60px" }} color="primary" />
                {status.isLoading ? (
                    <>
                        <Typography variant="h5">Verifying your email...</Typography>
                        <CircularProgress style={{ marginTop: "15px" }} />
                    </>
                ) : status.isError ? (
                    <>
                        <Typography variant="h5">Verification failed</Typography>
                        <Button variant="contained" style={{ marginTop: "15px", width: "50%" }} onClick={verifyEmail}>
                            Retry
                        </Button>
                    </>
                ) : status.isSuccess ? (
                    <>
                        <Typography variant="h5">Email is verified</Typography>
                        <Button variant="contained" style={{ marginTop: "15px" }} onClick={() => navigate(CATALOGUE_ROUTE)}>
                            Continue
                        </Button>
                    </>
                ) : (
                    <></>
                )}
            </Grid>
        </Dialog>
    );
};

export default EmailVerificationPage;
