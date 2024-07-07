import { host } from ".";
import updateTokens from "./../utils/updateTokens";

const sendLoginRequest = async (authData) => {
    try {
        const response = await host.post("/Auth/login", authData);

        return response;
    } catch (error) {
        if (error.response) {
            return error.response;
        } else if (error.request) {
            return { data: { statusCode: 500, message: "Service is currently unavailable" } };
        } else {
            return { data: { statusCode: 400, message: "Error while creating request" } };
        }
    }
};

const sendLogoutRequest = async () => {
    try {
        const response = await host.post(
            "/Auth/logout",
            { refreshToken: localStorage.getItem("refreshToken") },
            { headers: { Authorization: `Bearer ${localStorage.getItem("accessToken")}` } }
        );

        return response;
    } catch (error) {
        if (error.response) {
            if (!error.response.data.statusCode) {
                error.response.data = {
                    statusCode: error.response.status,
                    message: "Service is currently unavailable",
                };
            }

            if (error.response.data.statusCode === 401) {
                return await updateTokens(sendLogoutRequest);
            }

            return error.response;
        } else if (error.request) {
            return { data: { statusCode: 500, message: "Service is currently unavailable" } };
        } else {
            return { data: { statusCode: 400, message: "Error while creating request" } };
        }
    }
};

const sendRegisterRequest = async (userData) => {
    try {
        const response = await host.post("/Auth/register", userData, {
            headers: { Authorization: `Bearer ${localStorage.getItem("accessToken")}` },
        });

        return response;
    } catch (error) {
        if (error.response) {
            if (!error.response.data.statusCode) {
                error.response.data = {
                    statusCode: error.response.status,
                    message: "Service is currently unavailable",
                };
            }

            return error.response;
        } else if (error.request) {
            return { data: { statusCode: 500, message: "Service is currently unavailable" } };
        } else {
            return { data: { statusCode: 400, message: "Error while creating request" } };
        }
    }
};

const sendVerificationRequest = async (verificationToken) => {
    try {
        const response = await host.post("/Auth/verify", { verificationToken: verificationToken });

        return response;
    } catch (error) {
        if (error.response) {
            if (!error.response.data.statusCode) {
                error.response.data = {
                    statusCode: error.response.status,
                    message: "Service is currently unavailable",
                };
            }

            return error.response;
        } else if (error.request) {
            return { data: { statusCode: 500, message: "Service is currently unavailable" } };
        } else {
            return { data: { statusCode: 400, message: "Error while creating request" } };
        }
    }
};

export { sendLoginRequest, sendLogoutRequest, sendRegisterRequest, sendVerificationRequest };
