function calculateTotalPrice(product, quantity) {
    let pricePerPiece;
  
    switch (product) {
      case "coffee":
        pricePerPiece = 1.50;
        break;
      case "water":
        pricePerPiece = 1.00;
        break;
      case "coke":
        pricePerPiece = 1.40;
        break;
      case "snacks":
        pricePerPiece = 2.00;
        break;
    }
  
    const totalPrice = pricePerPiece * quantity;
  
    console.log(totalPrice.toFixed(2));
  }