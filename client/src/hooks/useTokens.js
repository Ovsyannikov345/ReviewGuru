import { useState } from "react";

export default function useTokens() {
    const getAccessToken = () => {
        return localStorage.getItem("accessToken");
    };

    const [accessToken, setAccessToken] = useState(getAccessToken());

    const saveAccessToken = (token) => {
        if (!token) {
            localStorage.removeItem("accessToken");
            setAccessToken(null);
            return;
        }

        localStorage.setItem("accessToken", token);
        setAccessToken(token);
    };

    const getRefreshToken = () => {
        return localStorage.getItem("refreshToken");
    };

    const [refreshToken, setRefreshToken] = useState(getRefreshToken());

    const saveRefreshToken = (token) => {
        if (!token) {
            localStorage.removeItem("refreshToken");
            setRefreshToken(null);
            return;
        }

        localStorage.setItem("refreshToken", token);
        setRefreshToken(token);
    };

    return {
        setAccessToken: saveAccessToken,
        setRefreshToken: saveRefreshToken,
        accessToken,
        refreshToken,
    };
}
