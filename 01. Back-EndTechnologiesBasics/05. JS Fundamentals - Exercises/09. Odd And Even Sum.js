function sumOddEvenDigits(number) {
    const digits = String(number).split('');

    let oddSum = 0;
    let evenSum = 0;

    digits.forEach(digit => {
        const num = parseInt(digit);

        if (num % 2 === 0) {
            evenSum += num;
        } else {
            oddSum += num;
        }
    });

    console.log(`Odd sum = ${oddSum}, Even sum = ${evenSum}`);
}