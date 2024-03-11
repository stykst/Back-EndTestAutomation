function countOccurrences(text, targetWord) {
    const words = text.split(' ');  
    let occurrences = 0;
  
    for (const word of words) {
      if (word === targetWord) {
        occurrences++;
      }
    }
  
    console.log(occurrences);
  }