function printObjectProperties(cityObject) {
    for (const key in cityObject) {
      console.log(`${key} -> ${cityObject[key]}`);
    }
  }