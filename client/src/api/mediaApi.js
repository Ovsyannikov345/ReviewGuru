import { host } from ".";
import { MEDIA_PER_PAGE } from "../utils/consts";
import updateTokens from "./../utils/updateTokens";

const sendMediaGetRequest = async (pageNumber, mediaType, searchText) => {
    try {
        const response = await host.get(
            `/media?pageNumber=${pageNumber}&pageSize=${MEDIA_PER_PAGE}&mediaType=${mediaType}&searchText=${searchText}`,
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
                return await updateTokens(sendMediaGetRequest, pageNumber, mediaType, searchText);
            }

            return error.response;
        } else if (error.request) {
            return { data: { statusCode: 500, message: "Service is currently unavailable" } };
        } else {
            return { data: { statusCode: 400, message: "Error while creating request" } };
        }
    }
};

const sendFavoritesGetRequest = async (pageNumber, mediaType, searchText) => {
    try {
        const response = await host.get(
            `/user/favorites?pageNumber=${pageNumber}&pageSize=${MEDIA_PER_PAGE}&mediaType=${mediaType}&searchText=${searchText}`,
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
                return await updateTokens(sendFavoritesGetRequest, pageNumber, mediaType, searchText);
            }

            return error.response;
        } else if (error.request) {
            return { data: { statusCode: 500, message: "Service is currently unavailable" } };
        } else {
            return { data: { statusCode: 400, message: "Error while creating request" } };
        }
    }
};

export { sendMediaGetRequest, sendFavoritesGetRequest };
