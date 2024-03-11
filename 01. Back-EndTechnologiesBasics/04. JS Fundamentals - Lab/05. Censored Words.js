function replaceWordWithStars(text, word) {
    const regex = new RegExp(word, 'g');  
    const stars = '*'.repeat(word.length);  
    const newText = text.replace(regex, stars);
  
    console.log(newText);
  }