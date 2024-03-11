function createObjectsFromTable(tableRows) {
    const result = [];
  
    for (const row of tableRows) {
      const [town, latitudeStr, longitudeStr] = row.split(' | ');
      const latitude = parseFloat(latitudeStr).toFixed(2);
      const longitude = parseFloat(longitudeStr).toFixed(2);
      const townObject = {
        town,
        latitude,
        longitude
      };
  
      result.push(townObject);
    }
  
    for (const townObject of result) {
      console.log(townObject);
    }
  }