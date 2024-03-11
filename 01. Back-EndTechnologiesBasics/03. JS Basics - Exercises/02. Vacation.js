function calculateTotalPrice(groupSize, groupType, dayOfWeek) {
    var pricePerPerson;

    switch (groupType.toLowerCase()) {
        case 'students':
            switch (dayOfWeek.toLowerCase()) {
                case 'friday':
                    pricePerPerson = 8.45;
                    break;
                case 'saturday':
                    pricePerPerson = 9.80;
                    break;
                case 'sunday':
                    pricePerPerson = 10.46;
                    break;
            }
            break;
        case 'business':
            switch (dayOfWeek.toLowerCase()) {
                case 'friday':
                    pricePerPerson = 10.90;
                    break;
                case 'saturday':
                    pricePerPerson = 15.60;
                    break;
                case 'sunday':
                    pricePerPerson = 16.00;
                    break;
            }
            break;
        case 'regular':
            switch (dayOfWeek.toLowerCase()) {
                case 'friday':
                    pricePerPerson = 15.00;
                    break;
                case 'saturday':
                    pricePerPerson = 20.00;
                    break;
                case 'sunday':
                    pricePerPerson = 22.50;
                    break;
            }
            break;
    }

    var totalPrice = groupSize * pricePerPerson;

    if (groupType.toLowerCase() === 'students' && groupSize >= 30) {
        totalPrice *= 0.85;
    } else if (groupType.toLowerCase() === 'business' && groupSize >= 100) {
        totalPrice -= 10 * pricePerPerson;
    } else if (groupType.toLowerCase() === 'regular' && groupSize >= 10 && groupSize <= 20) {
        totalPrice *= 0.95;
    }

    console.log(`Total price: ${totalPrice.toFixed(2)}`);
}