function substringAndPrint(inputString, startIndex, count) {
    if (startIndex < 0 || startIndex >= inputString.length) {
      console.log("Invalid starting index.");
      return;
    }
  
    const result = inputString.substr(startIndex, count);
  
    console.log(result);
  }