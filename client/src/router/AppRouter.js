import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { publicRoutes } from "./publicRoutes";
import { authorizedRoutes } from "./authorizedRoutes";
import { CATALOGUE_ROUTE } from "../utils/consts";

const AppRouter = () => {
    const accessToken = localStorage.getItem("accessToken");

    return (
        <Routes>
            {publicRoutes.map(({ path, Component }) => (
                <Route key={path} path={path} element={<Component />} exact />
            ))}
            {accessToken &&
                authorizedRoutes.map(({ path, Component }) => <Route key={path} path={path} element={<Component />} exact />)}
            <Route path="*" element={<Navigate to={CATALOGUE_ROUTE} />} />
        </Routes>
    );
};

export default AppRouter;
