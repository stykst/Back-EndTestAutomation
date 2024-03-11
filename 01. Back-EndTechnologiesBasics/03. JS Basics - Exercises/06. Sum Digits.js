function sumOfDigits(number) {
    var digits = number.toString().split('');

    var digitSum = digits.reduce(function (sum, digit) {
        return sum + parseInt(digit, 10);
    }, 0);

    console.log(digitSum);
}