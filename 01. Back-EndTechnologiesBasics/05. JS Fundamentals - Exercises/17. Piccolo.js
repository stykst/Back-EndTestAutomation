function manageParkingLot(entries) {
    let parkingLot = new Set();

    for (let entry of entries) {
        let [direction, carNumber] = entry.split(', ');
        if (direction === 'IN') {
            parkingLot.add(carNumber);
        } else if (direction === 'OUT') {
            if (parkingLot.has(carNumber)) {
                parkingLot.delete(carNumber);
            }
        }
    }

    let sortedParkingLot = Array.from(parkingLot).sort();

    if (sortedParkingLot.length === 0) {
        console.log("Parking Lot is Empty");
    } else {
        for (let carNumber of sortedParkingLot) {
            console.log(carNumber);
        }
    }
}

// Example usage:
let entries1 = ['IN, CA2844AA',
                'IN, CA1234TA',
                'OUT, CA2844AA',
                'IN, CA9999TT',
                'IN, CA2866HI',
                'OUT, CA1234TA',
                'IN, CA2844AA',
                'OUT, CA2866HI',
                'IN, CA9876HH',
                'IN, CA2822UU'];

let entries2 = ['IN, CA2844AA',
                'IN, CA1234TA',
                'OUT, CA2844AA',
                'OUT, CA1234TA'];

manageParkingLot(entries1);
// Output:
// CA2822UU
// CA2844AA
// CA9876HH
// CA9999TT

manageParkingLot(entries2);
// Output:
// Parking Lot is Empty
