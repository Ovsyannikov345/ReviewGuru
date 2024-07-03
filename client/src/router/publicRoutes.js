import { CATALOGUE_ROUTE, LOGIN_ROUTE, PRODUCT_ROUTE, REGISTER_ROUTE } from "../utils/consts";
import LoginPage from "./../pages/LoginPage";
import RegistrationPage from "./../pages/RegistrationPage";
import CataloguePage from "./../pages/CataloguePage";
import ProductPage from "./../pages/ProductPage";

export const publicRoutes = [
    {
        path: LOGIN_ROUTE,
        Component: LoginPage,
    },
    {
        path: REGISTER_ROUTE,
        Component: RegistrationPage,
    },
    {
        path: CATALOGUE_ROUTE,
        Component: CataloguePage,
    },
    {
        path: PRODUCT_ROUTE,
        Component: ProductPage,
    },
];