import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { publicRoutes } from "./publicRoutes";
import { authorizedRoutes } from "./authorizedRoutes";
import { CATALOGUE_ROUTE, LOGIN_ROUTE, REGISTER_ROUTE } from "../utils/consts";
import LoginPage from "../pages/LoginPage";
import RegistrationPage from "../pages/RegistrationPage";

const AppRouter = ({ accessToken, setAccessToken, setRefreshToken }) => {
    if (!accessToken) {
        return (
            <Routes>
                {publicRoutes.map(({ path, Component }) => (
                    <Route key={path} path={path} element={<Component />} exact />
                ))}
                <Route
                    key={LOGIN_ROUTE}
                    path={LOGIN_ROUTE}
                    element={<LoginPage setAcessToken={setAccessToken} setRefreshToken={setRefreshToken} />}
                    exact
                />
                <Route
                    key={REGISTER_ROUTE}
                    path={REGISTER_ROUTE}
                    element={<RegistrationPage setAcessToken={setAccessToken} setRefreshToken={setRefreshToken} />}
                    exact
                />
                <Route path="*" element={<Navigate to={CATALOGUE_ROUTE} />} />
            </Routes>
        );
    }

    return (
        <Routes>
            {publicRoutes.map(({ path, Component }) => (
                <Route key={path} path={path} element={<Component />} exact />
            ))}
            {authorizedRoutes.map(({ path, Component }) => (
                <Route key={path} path={path} element={<Component />} exact />
            ))}
            <Route path="*" element={<Navigate to={CATALOGUE_ROUTE} />} />
        </Routes>
    );
};

export default AppRouter;
