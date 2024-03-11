function printSortedNames(names) {
    const sortedNames = names.sort((a, b) => a.localeCompare(b));

    sortedNames.forEach((name, index) => {
        console.log(`${index + 1}.${name}`);
    });
}