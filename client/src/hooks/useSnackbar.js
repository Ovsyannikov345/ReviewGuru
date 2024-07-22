import { useState } from "react";
import Snackbar from "@mui/material/Snackbar";
import Alert from "@mui/material/Alert";

const useSnackbar = () => {
    const [error, setError] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [success, setSuccess] = useState(false);
    const [successMessage, setSuccessMessage] = useState("");

    const displayError = (message) => {
        setErrorMessage(message);
        setError(true);
    };

    const displaySuccess = (message) => {
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

    const ErrorSnackbar = () => (
        <Snackbar open={error} autoHideDuration={3000} onClose={closeSnackbar}>
            <Alert onClose={closeSnackbar} severity="error" sx={{ width: "100%" }}>
                {errorMessage}
            </Alert>
        </Snackbar>
    );

    const SuccessSnackbar = () => (
        <Snackbar open={success} autoHideDuration={3000} onClose={closeSnackbar}>
            <Alert onClose={closeSnackbar} severity="success" sx={{ width: "100%" }}>
                {successMessage}
            </Alert>
        </Snackbar>
    );

    return {
        displayError,
        displaySuccess,
        ErrorSnackbar,
        SuccessSnackbar,
    };
};

export default useSnackbar;
