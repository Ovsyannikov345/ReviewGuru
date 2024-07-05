import { BrowserRouter } from "react-router-dom";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterMoment } from "@mui/x-date-pickers/AdapterMoment";
import NavBar from "./components/NavBar";
import AppRouter from "./router/AppRouter";

function App() {
    return (
        <LocalizationProvider dateAdapter={AdapterMoment} adapterLocale="en-us">
            <BrowserRouter>
                <NavBar />
                <AppRouter />
            </BrowserRouter>
        </LocalizationProvider>
    );
}

export default App;
