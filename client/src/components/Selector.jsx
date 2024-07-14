import { MenuItem, Select } from "@mui/material";
import React from "react";

const Selector = ({ options, value, changeHandler }) => {
    return (
        <Select variant="outlined" value={value} onChange={(e) => changeHandler(e.target.value)} style={{ minWidth: "120px" }}>
            {options.map((option) => (
                <MenuItem key={option.value} value={option.value}>
                    {option.name}
                </MenuItem>
            ))}
        </Select>
    );
};

export default Selector;
