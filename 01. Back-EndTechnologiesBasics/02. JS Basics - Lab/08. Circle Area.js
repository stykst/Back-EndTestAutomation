function calculateCircleArea(input) {
    if (typeof input === 'number') {
        var area = Math.pow(input, 2) * Math.PI;
        console.log(area.toFixed(2));
    } else {
        console.log(`We can not calculate the circle area, because we receive a ${typeof input}.`);
    }
}