import { CREATE_REVIEW_ROUTE, FAVOURITES_ROUTE, MY_REVIEWS_ROUTE } from "../utils/consts";
import MyReviewsPage from "./../pages/MyReviewsPage";
import FavouritesPage from "./../pages/FavouritesPage";
import ReviewCreationPage from "../pages/ReviewCreationPage";

export const authorizedRoutes = [
    {
        path: MY_REVIEWS_ROUTE,
        Component: MyReviewsPage,
    },
    {
        path: FAVOURITES_ROUTE,
        Component: FavouritesPage,
    },
    {
        path: CREATE_REVIEW_ROUTE,
        Component: ReviewCreationPage,
    },
];
