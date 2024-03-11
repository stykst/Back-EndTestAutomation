function updateStock(currentStock, orderedProducts) {
    const stockObject = {};
  
    for (let i = 0; i < currentStock.length; i += 2) {
      const product = currentStock[i];
      const quantity = parseInt(currentStock[i + 1]);
  
      stockObject[product] = quantity;
    }
  
    for (let i = 0; i < orderedProducts.length; i += 2) {
      const product = orderedProducts[i];
      const quantity = parseInt(orderedProducts[i + 1]);
  
      if (stockObject.hasOwnProperty(product)) {
        stockObject[product] += quantity;
      } else {
        stockObject[product] = quantity;
      }
    }
  
    for (const [product, quantity] of Object.entries(stockObject)) {
      console.log(`${product} -> ${quantity}`);
    }
  }