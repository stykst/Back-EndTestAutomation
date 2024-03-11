function processPhonebook(inputArray) {
    const phonebook = {};
  
    for (const entry of inputArray) {
      const [name, number] = entry.split(' ');
  
      phonebook[name] = number;
    }
  
    for (const name in phonebook) {
      console.log(`${name} -> ${phonebook[name]}`);
    }
  }