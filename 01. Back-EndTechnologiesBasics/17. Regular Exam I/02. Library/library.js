function library(books) {
    let numBooks = parseInt(books[0]);
    let bookList = books.slice(1, numBooks + 1);
    let commands = books.slice(numBooks + 1);

    for (let command of commands) {
        let tokens = command.split(' ');
        let action = tokens[0];

        if (action === 'Lend') {
            if (bookList.length > 0) {
                let lentBook = bookList.shift();
                console.log(`${lentBook} book lent!`);
            }
        } else if (action === 'Return') {
            let bookTitle = tokens.slice(1).join(' ');
            bookList.unshift(bookTitle);
        } else if (action === 'Exchange') {
            let startIndex = parseInt(tokens[1]);
            let endIndex = parseInt(tokens[2]);

            if (startIndex >= 0 && endIndex < bookList.length && startIndex < endIndex) {
                let temp = bookList[startIndex];
                bookList[startIndex] = bookList[endIndex];
                bookList[endIndex] = temp;
                console.log("Exchanged!");
            }
        } else if (action === 'Stop') {
            if (bookList.length > 0) {
                console.log(`Books left: ${bookList.join(', ')}`);
            } else {
                console.log("The library is empty");
            }
            break;
        }
    }
}

// Test cases
library(['3', 'Harry Potter', 'The Lord of the Rings', 'The Hunger Games', 'Lend', 'Stop', 'Exchange 0 1']);
library(['5', 'The Catcher in the Rye', 'To Kill a Mockingbird', 'The Great Gatsby', '1984', 'Animal Farm', 'Return Brave New World', 'Exchange 1 4', 'Stop']);
library(['3', 'The Da Vinci Code', 'The Girl with the Dragon Tattoo', 'The Kite Runner', 'Lend', 'Lend', 'Lend', 'Stop']);
