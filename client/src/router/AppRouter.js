import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { publicRoutes } from "./publicRoutes";
import { authorizedRoutes } from "./authorizedRoutes";
import { CATALOGUE_ROUTE, LOGIN_ROUTE, REGISTER_ROUTE } from "../utils/consts";
import LoginPage from "../pages/LoginPage";
import RegistrationPage from "../pages/RegistrationPage";

const AppRouter = ({ accessToken, refreshToken, setAccessToken, setRefreshToken }) => {
    if (!accessToken) {
        return (
            <Routes>
                {publicRoutes.map(({ path, Component }) => (
                    <Route key={path} path={path} element={<Component />} exact />
                ))}
                <Route
                    key={LOGIN_ROUTE}
                    path={LOGIN_ROUTE}
                    element={<LoginPage setAccessToken={setAccessToken} setRefreshToken={setRefreshToken} />}
                    exact
                />
                <Route
                    key={REGISTER_ROUTE}
                    path={REGISTER_ROUTE}
                    element={<RegistrationPage setAccessToken={setAccessToken} setRefreshToken={setRefreshToken} />}
                    exact
                />
                <Route path="*" element={<Navigate to={CATALOGUE_ROUTE} />} />
            </Routes>
        );
    }

    return (
        <Routes>
            {publicRoutes.map(({ path, Component }) => (
                <Route
                    key={path}
                    path={path}
                    element={
                        <Component
                            accessToken={accessToken}
                            refreshToken={refreshToken}
                            setAccessToken={setAccessToken}
                            setRefreshToken={setRefreshToken}
                        />
                    }
                    exact
                />
            ))}
            {authorizedRoutes.map(({ path, Component }) => (
                <Route
                    key={path}
                    path={path}
                    element={
                        <Component
                            accessToken={accessToken}
                            refreshToken={refreshToken}
                            setAccessToken={setAccessToken}
                            setRefreshToken={setRefreshToken}
                        />
                    }
                    exact
                />
            ))}
            <Route path="*" element={<Navigate to={CATALOGUE_ROUTE} />} />
        </Routes>
    );
};

export default AppRouter;
