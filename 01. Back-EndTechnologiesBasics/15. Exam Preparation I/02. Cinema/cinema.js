function cinema(input) {
    const n = parseInt(input[0]);
    let movies = input.slice(1, n + 1);
    
    for (let i = n + 1; i < input.length; i++) {
        let command = input[i].split(' ');
        
        switch(command[0]) {
            case 'Sell':
                if (movies.length > 0) {
                    console.log(`${movies.shift()} ticket sold!`);
                }
                break;
            case 'Add':
                movies.push(command.slice(1).join(' '));
                break;
            case 'Swap':
                let startIdx = parseInt(command[1]);
                let endIdx = parseInt(command[2]);
                let temp = movies[startIdx];
                movies[startIdx] = movies[endIdx];
                movies[endIdx] = temp;
                console.log("Swapped!");
                break;
            case 'End':
                if (movies.length > 0) {
                    console.log(`Tickets left: ${movies.join(', ')}`);
                } else {
                    console.log("The box office is empty");
                }
                return;
        }
    }
}

cinema(['3', 'Avatar', 'Titanic', 'Joker', 'Sell', 'End', 'Swap 0 1']);
cinema(['5', 'The Matrix', 'The Godfather', 'The Shawshank Redemption', 'The Dark Knight', 'Inception', 'Add The Lord of the Rings', 'Swap 1 4', 'End']);
cinema(['3', 'Star Wars', 'Harry Potter', 'The Hunger Games', 'Sell', 'Sell', 'Sell', 'End']);
