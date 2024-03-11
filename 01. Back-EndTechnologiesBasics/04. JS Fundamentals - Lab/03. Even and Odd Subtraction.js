function calculateDifference(arr) {
    let evenSum = 0;
    let oddSum = 0;
  
    for (let i = 0; i < arr.length; i++) {
      const num = parseFloat(arr[i]);
  
      if (!isNaN(num)) {
        if (num % 2 === 0) {
          evenSum += num;
        } else {
          oddSum += num;
        }
      }
    }
  
    console.log(evenSum - oddSum);
  }