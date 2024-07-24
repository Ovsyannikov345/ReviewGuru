import React, { useState } from "react";
import { TextField, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Button } from "@mui/material";
import Autocomplete, { createFilterOptions } from "@mui/material/Autocomplete";

const filter = createFilterOptions();

const AuthorAutocomplete = ({ authors, addAuthor, addNewAuthor, displayError }) => {
    const [authorCreationToggle, setAuthorCreationToggle] = useState(false);

    const [authorInputValue, setAuthorInputValue] = useState("");

    const [newAuthor, setNewAuthor] = useState({
        firstName: "",
        lastName: "",
    });

    const handleClose = () => {
        setNewAuthor({
            firstName: "",
            lastName: "",
        });

        setAuthorCreationToggle(false);
    };

    const handleSubmit = (event) => {
        event.preventDefault();

        if (!newAuthor.firstName || !newAuthor.lastName) {
            displayError("Fill the author information");
            return;
        }

        addNewAuthor(newAuthor);
        handleClose();
    };

    return (
        <>
            <Autocomplete
                value={null}
                onChange={(event, newValue) => {
                    if (typeof newValue === "string") {
                        setTimeout(() => {
                            let authorData = newValue.split(" ");

                            if (authorData.length === 1) {
                                authorData.push("");
                            }

                            setAuthorCreationToggle(true);
                            setNewAuthor({
                                firstName: authorData[0],
                                lastName: authorData[1],
                            });
                        });
                    } else if (newValue && newValue.inputValue) {
                        let authorData = newValue.inputValue.split(" ");

                        if (authorData.length === 1) {
                            authorData.push("");
                        }

                        setAuthorCreationToggle(true);
                        setNewAuthor({
                            firstName: authorData[0],
                            lastName: authorData[1],
                        });
                    } else {
                        addAuthor(newValue);
                    }

                    setAuthorInputValue("");
                }}
                inputValue={authorInputValue}
                onInputChange={(event, newInputValue) => {
                    setAuthorInputValue(newInputValue);
                }}
                filterOptions={(options, params) => {
                    const filtered = filter(options, params);

                    if (params.inputValue !== "") {
                        filtered.push({
                            inputValue: params.inputValue,
                            firstName: `Add "${params.inputValue}"`,
                        });
                    }

                    return filtered;
                }}
                id="free-solo-dialog"
                options={authors}
                getOptionLabel={(option) => {
                    if (typeof option === "string") {
                        return option;
                    }
                    if (option.inputValue) {
                        return option.inputValue;
                    }
                    return option.firstName + " " + option.lastName;
                }}
                selectOnFocus
                clearOnBlur
                handleHomeEndKeys
                renderOption={(props, option) => {
                    const { key, ...optionProps } = props;
                    return (
                        <li key={option.firstName + option.lastName} {...optionProps}>
                            {option.firstName + " " + (option.lastName ?? "")}
                        </li>
                    );
                }}
                sx={{ width: 300 }}
                freeSolo
                renderInput={(params) => <TextField {...params} label="Select authors" />}
                groupBy={(a) => a.groupLetter}
                style={{ marginTop: "5px" }}
            />
            <Dialog open={authorCreationToggle} onClose={handleClose}>
                <form onSubmit={handleSubmit}>
                    <DialogTitle>Add a new author</DialogTitle>
                    <DialogContent>
                        <DialogContentText>Did you miss any author in our list? Please, add it!</DialogContentText>
                        <TextField
                            autoFocus
                            margin="dense"
                            id="name"
                            value={newAuthor.firstName}
                            onChange={(event) =>
                                setNewAuthor({
                                    ...newAuthor,
                                    firstName: event.target.value,
                                })
                            }
                            label="First name"
                            type="text"
                            variant="standard"
                            style={{marginRight: "10px"}}
                        />
                        <TextField
                            margin="dense"
                            id="name"
                            value={newAuthor.lastName}
                            onChange={(event) =>
                                setNewAuthor({
                                    ...newAuthor,
                                    lastName: event.target.value,
                                })
                            }
                            label="Last name"
                            type="text"
                            variant="standard"
                        />
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={handleClose}>Cancel</Button>
                        <Button type="submit">Add</Button>
                    </DialogActions>
                </form>
            </Dialog>
        </>
    );
};

export default AuthorAutocomplete;
