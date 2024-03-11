function validatePassword(password) {
    const isLengthValid = password.length >= 6 && password.length <= 10;
    const isAlphanumeric = /^[a-zA-Z0-9]+$/.test(password);
    const hasAtLeastTwoDigits = (password.match(/\d/g) || []).length >= 2;

    if (!isLengthValid) {
        console.log("Password must be between 6 and 10 characters");
    }

    if (!isAlphanumeric) {
        console.log("Password must consist only of letters and digits");
    }

    if (!hasAtLeastTwoDigits) {
        console.log("Password must have at least 2 digits");
    }

    if (isLengthValid && isAlphanumeric && hasAtLeastTwoDigits) {
        console.log("Password is valid");
    }
}