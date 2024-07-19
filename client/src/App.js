import { BrowserRouter } from "react-router-dom";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterMoment } from "@mui/x-date-pickers/AdapterMoment";
import NavBar from "./components/headers/NavBar";
import AppRouter from "./router/AppRouter";
import useTokens from "./hooks/useTokens";

function App() {
    const { accessToken, refreshToken, setAccessToken, setRefreshToken } = useTokens();

    return (
        <LocalizationProvider dateAdapter={AdapterMoment} adapterLocale="en-us">
            <BrowserRouter>
                <NavBar
                    accessToken={accessToken}
                    refreshToken={refreshToken}
                    setAccessToken={setAccessToken}
                    setRefreshToken={setRefreshToken}
                />
                <AppRouter
                    accessToken={accessToken}
                    refreshToken={refreshToken}
                    setAccessToken={setAccessToken}
                    setRefreshToken={setRefreshToken}
                />
            </BrowserRouter>
        </LocalizationProvider>
    );
}

export default App;
