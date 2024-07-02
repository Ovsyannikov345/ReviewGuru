import { FAVOURITES_ROUTE, MY_REVIEWS_ROUTE } from "../utils/consts";
import MyReviewsPage from "./../pages/MyReviewsPage";
import FavouritesPage from "./../pages/FavouritesPage";

export const authorizedRoutes = [
    {
        path: MY_REVIEWS_ROUTE,
        Component: MyReviewsPage,
    },
    {
        path: FAVOURITES_ROUTE,
        Component: FavouritesPage,
    },
];
