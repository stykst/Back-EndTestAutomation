function dvd_collection(dvds) {
    let numDvds = parseInt(dvds.shift());
    let collection = dvds.slice(0, numDvds);

    let remainingDVDs = () => {
        if (collection.length === 0) {
            console.log("The collection is empty");
        } else {
            console.log("DVDs left: " + collection.join(", "));
        }
    };

    for (let i = numDvds; i < dvds.length; i++) {
        let command = dvds[i].split(' ');

        if (command[0] === 'Watch') {
            if (collection.length > 0) {
                console.log(collection.shift() + " DVD watched!");
            }
        } else if (command[0] === 'Buy') {
            collection.push(command.slice(1).join(' '));
        } else if (command[0] === 'Swap') {
            let startIndex = parseInt(command[1]);
            let endIndex = parseInt(command[2]);
            if (startIndex >= 0 && startIndex < collection.length && endIndex >= 0 && endIndex < collection.length) {
                let temp = collection[startIndex];
                collection[startIndex] = collection[endIndex];
                collection[endIndex] = temp;
                console.log("Swapped!");
            }
        } else if (command[0] === 'Done') {
            remainingDVDs();
            return;
        }
    }
    remainingDVDs();
}

// Test cases
dvd_collection(['3', 'The Matrix', 'The Godfather', 'The Shawshank Redemption', 'Watch', 'Done', 'Swap 0 1']);
dvd_collection(['5', 'The Lion King', 'Frozen', 'Moana', 'Toy Story', 'Shrek', 'Buy Coco', 'Swap 2 4', 'Done']);
dvd_collection(['5', 'The Avengers', 'Iron Man', 'Thor', 'Captain America', 'Black Panther', 'Watch', 'Watch', 'Watch', 'Watch', 'Watch', 'Done']);
