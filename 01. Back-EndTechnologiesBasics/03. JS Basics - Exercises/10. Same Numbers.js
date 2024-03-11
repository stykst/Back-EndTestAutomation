function checkDigitsAndSum(input) {
    var numberString = input.toString();
    var firstDigit = numberString[0];
    var isSame = true;
    var digitSum = 0;

    for (var i = 0; i < numberString.length; i++) {
        digitSum += parseInt(numberString[i]);

        if (numberString[i] !== firstDigit) {
            isSame = false;
        }
    }

    console.log(isSame);
    console.log(digitSum);
}