export default function useApiRequest(accessToken, refreshToken, setAccessToken, setRefreshToken) {
    const updateTokens = async () => {
        const response = await fetch(`${process.env.REACT_APP_SERVER_URL}/Token/refresh`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json;charset=utf-8",
            },
            body: JSON.stringify({ refreshToken }),
        });

        if (!response.ok) {
            throw new Error("Session expired");
        }

        const tokens = await response.json();

        return tokens;
    };

    const reattemptWithNewToken = async (endpointPath, method, body, queryString, newAccessToken) => {
        let response;

        try {
            response = await fetch(`${process.env.REACT_APP_SERVER_URL}/${endpointPath}?${queryString}`, {
                method: method.toUpperCase(),
                headers: {
                    "Content-Type": "application/json;charset=utf-8",
                    Authorization: `Bearer ${newAccessToken}`,
                },
                body: method.toUpperCase() === "GET" ? null : JSON.stringify(body),
            });
        } catch {
            return { ok: false, error: "Service is currently unavailable" };
        }

        if (response.ok) {
            if (response.status === 204 || response.headers.get("content-length") === "0") {
                return { ok: true, data: null };
            }

            const responseData = await response.json();

            return { ok: true, data: responseData };
        }

        let responseData = await response.json();

        return { ok: false, error: responseData.message ?? "Unexpected error" };
    };

    const sendRequest = async (endpointPath, method, body, queryParams) => {
        let queryString = "";

        if (queryParams != null) {
            const params = new URLSearchParams();

            for (const [key, value] of Object.entries(queryParams)) {
                params.append(key, value);
            }

            queryString = params.toString();
        }

        let response;

        try {
            response = await fetch(`${process.env.REACT_APP_SERVER_URL}/${endpointPath}?${queryString}`, {
                method: method.toUpperCase(),
                headers: {
                    "Content-Type": "application/json;charset=utf-8",
                    Authorization: `Bearer ${accessToken}`,
                },
                body: method.toUpperCase() === "GET" ? null : JSON.stringify(body),
            });
        } catch {
            return { ok: false, error: "Service is currently unavailable" };
        }

        if (response.ok) {
            if (response.status === 204 || response.headers.get("content-length") === "0") {
                return { ok: true, data: null };
            }

            const responseData = await response.json();

            return { ok: true, data: responseData };
        }

        if (response.status === 401) {
            if (!accessToken) {
                return { ok: false, error: "You have to be logged in" };
            }

            let tokens;

            try {
                tokens = await updateTokens();
            } catch (err) {
                setAccessToken(null);
                setRefreshToken(null);

                return { ok: false, error: "Session expired" };
            }

            setAccessToken(tokens.accessToken);
            setRefreshToken(tokens.refreshToken);

            return await reattemptWithNewToken(endpointPath, method, body, queryString, tokens.accessToken);
        }

        let responseData = await response.json();

        return { ok: false, error: responseData.message ?? "Unexpected error" };
    };

    return sendRequest;
}
