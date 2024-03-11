function reverseAndPrint(n, elements) {
    const newArray = elements.slice(0, n);
  
    newArray.reverse();
  
    console.log(newArray.join(" "));
  }