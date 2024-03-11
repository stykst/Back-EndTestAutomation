function calculateTicketPrice(dayType, age) {
    var price;

    switch (dayType.toLowerCase()) {
        case 'weekday':
            if (age >= 0 && age <= 18) {
                price = 12;
            } else if (age > 18 && age <= 64) {
                price = 18;
            } else if (age > 64 && age <= 122) {
                price = 12;
            } else {
                console.log("Error!");
                return;
            }
            break;
        case 'weekend':
            if (age >= 0 && age <= 18) {
                price = 15;
            } else if (age > 18 && age <= 64) {
                price = 20;
            } else if (age > 64 && age <= 122) {
                price = 15;
            } else {
                console.log("Error!");
                return;
            }
            break;
        case 'holiday':
            if (age >= 0 && age <= 18) {
                price = 5;
            } else if (age > 18 && age <= 64) {
                price = 12;
            } else if (age > 64 && age <= 122) {
                price = 10;
            } else {
                console.log("Error!");
                return;
            }
            break;
    }

    console.log(`${price}$`);
}