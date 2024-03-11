function rotateArray(arr, rotations) {
    const effectiveRotations = rotations % arr.length;  
    const rotatedArray = arr.slice(effectiveRotations).concat(arr.slice(0, effectiveRotations));
  
    console.log(rotatedArray.join(' '));
  }