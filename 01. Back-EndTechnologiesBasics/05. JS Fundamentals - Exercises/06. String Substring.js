function findWord(word, text) {
    const lowercaseWord = text
    .toLowerCase()
    .split(' ')
    .includes(word);

    if (lowercaseWord) {
        console.log(word);
    } else {
        console.log(`${word} not found!`);
    }
}