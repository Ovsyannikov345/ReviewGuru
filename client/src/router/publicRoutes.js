import { CATALOGUE_ROUTE, MEDIA_ROUTE, REGISTER_ROUTE, VERIFICATION_ROUTE } from "../utils/consts";
import RegistrationPage from "./../pages/RegistrationPage";
import CataloguePage from "./../pages/CataloguePage";
import ProductPage from "./../pages/ProductPage";
import EmailVerificationPage from "../pages/EmailVerificationPage";

export const publicRoutes = [
    {
        path: CATALOGUE_ROUTE,
        Component: CataloguePage,
    },
    {
        path: MEDIA_ROUTE,
        Component: ProductPage,
    },
    {
        path: VERIFICATION_ROUTE,
        Component: EmailVerificationPage,
    },
];
