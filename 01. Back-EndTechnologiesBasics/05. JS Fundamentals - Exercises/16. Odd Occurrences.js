function extractOddOccurrences(input) {
  const words = input.toLowerCase().split(' ');

  const wordCount = {};
  const result = [];
  const order = [];

  words.forEach(word => {
      const lowerCaseWord = word.toLowerCase();
      wordCount[lowerCaseWord] = (wordCount[lowerCaseWord] || 0) + 1;

      if (!order.includes(lowerCaseWord)) {
          order.push(lowerCaseWord);
      }
  });

  order.forEach(word => {
      if (wordCount[word] % 2 !== 0) {
          result.push(word);
      }
  });

  return result.join(' ');
}