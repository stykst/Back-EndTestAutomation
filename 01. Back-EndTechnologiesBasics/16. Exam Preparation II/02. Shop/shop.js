function shop(products, args) {
    const n = parseInt(products[0]);
    let productList = products.slice(1, n + 1);
    
    for (let i = n + 1; i < products.length; i++) {
        let command = products[i].split(' ');
        
        switch(command[0]) {
            case 'Sell':
                if (productList.length > 0) {
                    console.log(`${productList.shift()} product sold!`);
                }
                break;
            case 'Add':
                productList.push(command.slice(1).join(' '));
                break;
            case 'Swap':
                let startIdx = parseInt(command[1]);
                let endIdx = parseInt(command[2]);
                let temp = productList[startIdx];
                productList[startIdx] = productList[endIdx];
                productList[endIdx] = temp;
                console.log("Swapped!");
                break;
            case 'End':
                if (productList.length > 0) {
                    console.log(`Products left: ${productList.join(', ')}`);
                } else {
                    console.log("The shop is empty");
                }
                return;
        }
    }
}

// Example usage:
shop(['3', 'Apple', 'Banana', 'Orange', 'Sell', 'End', 'Swap 0 1']);
shop(['5', 'Milk', 'Eggs', 'Bread', 'Cheese', 'Butter', 'Add Yogurt', 'Swap 1 4', 'End']);
shop(['3', 'Shampoo', 'Soap', 'Toothpaste', 'Sell', 'Sell', 'Sell', 'End']);
