function calculateMoneyNeeded(fruit, weightInGrams, pricePerKilogram) {
    var weightInKilograms = weightInGrams / 1000;
    var money = weightInKilograms * pricePerKilogram;

    console.log(`I need $${money.toFixed(2)} to buy ${weightInKilograms.toFixed(2)} kilograms ${fruit}.`);
}