import moment from "moment";

const validateUser = ({ login, password, passwordDuplicate, email, dateOfBirth }) => {
    const errors = {};

    if (!login) {
        errors.login = "Required field";
    } else if (!/^[A-Za-z0-9]+$/i.test(login)) {
        errors.login = "Login should only contain latin letters and numbers";
    } else if (login.length > 50) {
        errors.firstName = "Login should be shorter than 50 symbols";
    }

    if (!password) {
        errors.password = "Required field";
    } else if (password.length < 5 || password.length > 20) {
        errors.password = "Password should be from 5 to 20 symbols";
    }

    if (password && passwordDuplicate !== password) {
        errors.passwordDuplicate = "Passwords don't match";
    }

    if (!email) {
        errors.email = "Required field";
    } else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(email)) {
        errors.email = "Invalid email format";
    } else if (email.length > 200) {
        errors.email = "Email should be shorter than 200 symbols";
    }

    if (dateOfBirth && (dateOfBirth.isAfter(moment()) || dateOfBirth.isBefore(moment().add(-100, "year")))) {
        errors.dateOfBirth = "Invalid date";
    }

    return errors;
};

export default validateUser;
