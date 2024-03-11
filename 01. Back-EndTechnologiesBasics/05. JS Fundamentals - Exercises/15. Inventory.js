function createHeroRegister(input) {
    const heroes = [];
  
    for (const line of input) {
      const [heroName, heroLevel, itemsString] = line.split(' / ');
  
      const hero = {
        name: heroName,
        level: Number(heroLevel),
        items: itemsString ? itemsString.split(', ') : []
      };
  
      heroes.push(hero);
    }
  
    // Sort the heroes array by level in ascending order
    heroes.sort((a, b) => a.level - b.level);
  
    // Print the information for each hero
    for (const hero of heroes) {
      console.log(`Hero: ${hero.name}`);
      console.log(`level => ${hero.level}`);
      console.log(`items => ${hero.items.join(', ')}`);
    }
  }
  
  // Example usage
  const input1 = [
    'Isacc / 25 / Apple, GravityGun',
    'Derek / 12 / BarrelVest, DestructionSword',
    'Hes / 1 / Desolator, Sentinel, Antara'
  ];
  createHeroRegister(input1);
  console.log('\n'); // Separate the outputs for better readability
  
  const input2 = [
    'Batman / 2 / Banana, Gun',
    'Superman / 18 / Sword',
    'Poppy / 28 / Sentinel, Antara'
  ];
  createHeroRegister(input2);
  