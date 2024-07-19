import { IconButton } from "@mui/material";
import React from "react";
import { useNavigate } from "react-router-dom";
import BackIcon from "@mui/icons-material/ArrowBackIos";

const NavigateBack = ({ to, label }) => {
    const navigate = useNavigate();

    return (
        <IconButton color="primary" onClick={() => navigate(to)}>
            <BackIcon></BackIcon>
            {label}
        </IconButton>
    );
};

export default NavigateBack;