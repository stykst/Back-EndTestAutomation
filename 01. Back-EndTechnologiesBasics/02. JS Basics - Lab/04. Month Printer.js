function printMonthName(monthNumber) {
    var months = [
        "Error!", "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    if (monthNumber >= 1 && monthNumber <= 12) {
        console.log(months[monthNumber]);
    } else {
        console.log("Error!");
    }
}
printMonthName(3)