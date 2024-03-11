function sortAlternately(arr) {
    arr.sort((a, b) => a - b);
  
    const result = [];
  
    for (let i = 0, j = arr.length - 1; i <= j; i++, j--) {
      result.push(arr[i]);
  
      if (i !== j) {
        result.push(arr[j]);
      }
    }
  
    return result;
  }