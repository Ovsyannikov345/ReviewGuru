import axios from "axios";

const host = axios.create({
    baseURL: process.env.REACT_APP_SERVER_URL,
    headers: {
        common: {
            Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
        },
    },
});

const authHost = axios.create({
    baseURL: process.env.REACT_APP_SERVER_URL,
});

export { host, authHost };
