// Auth routes
export const LOGIN_ROUTE = "/login";
export const REGISTER_ROUTE = "/register";

// Public routes
export const CATALOGUE_ROUTE = "/catalogue";
export const MEDIA_ROUTE = "/media/:id";
export const VERIFICATION_ROUTE = "/verify-email";

// Authorized routes
export const MY_REVIEWS_ROUTE = "/my-reviews";
export const FAVOURITES_ROUTE = "/favourites";
export const CREATE_REVIEW_ROUTE = "/create-review";
export const CREATE_MEDIA_ROUTE = "/create-media";

// Consts
export const MEDIA_PER_PAGE = 10;
export const FAVORITES_PER_PAGE = 10;

export const MEDIA_TYPES = [
    { value: "All", name: "All" },
    { value: "Movie", name: "Movie" },
    { value: "Music", name: "Music" },
    { value: "Book", name: "Book" },
];
