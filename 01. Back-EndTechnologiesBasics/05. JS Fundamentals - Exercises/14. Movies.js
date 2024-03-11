function solve(input) {
    const movies = [];
  
    for (const line of input) {
      const [command, ...params] = line.split(' ');
  
      if (command === 'addMovie') {
        const movieName = params.join(' ');
        movies.push({ name: movieName });
      } else {
        const movieIndex = movies.findIndex(movie => movie.name === command);
  
        if (movieIndex !== -1) {
          if (params.includes('directedBy')) {
            const director = params.slice(params.indexOf('directedBy') + 1).join(' ');
            movies[movieIndex].director = director;
          } else if (params.includes('onDate')) {
            const date = params.slice(params.indexOf('onDate') + 1).join(' ');
            movies[movieIndex].date = date;
          }
        }
      }
    }
  
    const filteredMovies = movies.filter(movie => movie.name && movie.director && movie.date);
    filteredMovies.forEach(movie => {
      console.log(JSON.stringify(movie));
    });
  }

 function manageMovies(input){
  const movies = [];

  for (const item of input) {
    if (input.includes('addMovie')) {
      const movieName = input.replace('addMovie', '')
      
      movies.push({
        name: movieName,
        date: '',
        director: '',
      })
    } else if (input.includes('directedBy')) {
      const movieName = item.substring(0, item.indexOf('directedBy')).trim()
      const directedBy = item.substring(item.indexOf('directedBy') + 'directedBy'.length).trim()

      const result = movies.find(movie => movie.name === movieName)

      if(!result) {
        movies.push({
          name: movieName,
          director: directedBy,
          date: '',
        })
      } else {
        result.director = directedBy
      }
    } else if (input.includes('onDate')) {
      const movieName = item.substring(0, item.indexOf('onDate')).trim()
      const onDate = item.substring(item.indexOf('onDate') + 'onDate'.length).trim()

      const result = movies.find(movie => movie.name === movieName)

      if(!result) {
        movies.push({
          name: movieName,
          director: '',
          date: onDate,
        })
      } else {
        result.date = onDate
      }
    }
  }
  
  const filteredMovies = movies.filter(movie => movie.name && movie.director && movie.date);
  filteredMovies.forEach(movie => {
    console.log(JSON.stringify(movie));
  });
 } 

 solve([
  'addMovie Fast and Furious',
  'addMovie Godfather',
  'Inception directedBy Christopher Nolan',
  'Godfather directedBy Francis Ford Coppola',
  'Godfather onDate 29.07.2018',
  'Fast and Furious onDate 30.07.2018',
  'Batman onDate 01.08.2018',
  'Fast and Furious directedBy Rob Cohen'
  ]
  )