import { host } from "../api";

const updateTokens = async (callbackFunction, argument) => {
    const refreshToken = localStorage.getItem("refreshToken");

    try {
        const response = await host.post("/Token/refresh", { refreshToken: refreshToken });

        localStorage.setItem("accessToken", response.data.accessToken);
        localStorage.setItem("refreshToken", response.data.refreshToken);

        if (argument === undefined) {
            return await callbackFunction();
        }

        return await callbackFunction(argument);
    } catch (error) {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
    }
};

export default updateTokens;
