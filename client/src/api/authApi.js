import { authHost } from ".";

const sendLoginRequest = async (authData) => {
    try {
        const response = await authHost.post("/Auth/login", authData);

        return response;
    } catch (error) {
        if (error.response) {
            if (!error.response.data.statusCode) {
                error.response.data = {
                    statusCode: 500,
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

const sendLogoutRequest = async (refreshToken) => {
    try {
        const response = await authHost.post("/Auth/logout", { refreshToken: refreshToken });

        return response;
    } catch (error) {
        console.log(error.response);

        if (error.response) {
            if (!error.response.data.statusCode) {
                error.response.data = {
                    statusCode: 500,
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

export { sendLoginRequest, sendLogoutRequest };
