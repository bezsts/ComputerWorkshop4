let moviesList = document.getElementById('movies-list');

let showAllMoviesButton = document.getElementById('all-movies-button');
let showPopularMoviesButton = document.getElementById('popular-movies-button');
let showWatchedMoviesButton = document.getElementById('watched-movies-button');
let exportButton = document.getElementById('export-movies-button');

window.onload = loadAllMovies();

showAllMoviesButton.addEventListener('click', loadAllMovies);
showPopularMoviesButton.addEventListener('click', loadPopularMovies);
exportButton.addEventListener('click', ExportAllMovies);

async function loadAllMovies() {
    let moviesResponse = await fetch('https://localhost:5001/api/movies', {
        method: 'get'
    });

    if (!moviesResponse.ok) {
        return;
    }

    let moviesData = await moviesResponse.json();

    moviesList.innerHTML = '';

    for (let i = 0; i < moviesData.length; i++) {
        moviesList.appendChild(createMovieElement(moviesData[i]));
    }
}

async function loadPopularMovies() {
    let moviesResponse = await fetch('https://localhost:5001/api/movies/popular', {
        method: 'get'
    });

    if (!moviesResponse.ok) {
        return;
    }

    let moviesData = await moviesResponse.json();

    moviesList.innerHTML = '';

    for (let i = 0; i < moviesData.length; i++) {
        moviesList.appendChild(createMovieElement(moviesData[i]));
    }
}

async function ExportAllMovies() {
    let response = await fetch('https://localhost:5001/api/movies/download', {
        method: 'get'
    });

    if (!response.ok) {
        return;
    }

    let blob = await response.blob();
    let url = URL.createObjectURL(blob);

    let link = document.createElement('a');
    link.href = url;
    link.download = 'movies.csv';
    link.click();

    URL.revokeObjectURL(url);
}
function createMovieElement(movieData) {
    let li = document.createElement('li');

    let titleDiv = document.createElement('div');
    titleDiv.className = 'title';
    let title = document.createElement('span');
    title.textContent = movieData.title;
    titleDiv.appendChild(title);

    let metadataDiv = document.createElement('div');
    metadataDiv.className = 'metadata';
    let director = document.createElement('span');
    director.textContent = movieData.director;
    let genre = document.createElement('span');
    genre.textContent = movieData.genre;
    let releaseDate = document.createElement('span');
    releaseDate.textContent = movieData.releaseDate;

    metadataDiv.appendChild(director);
    metadataDiv.appendChild(genre);
    metadataDiv.appendChild(releaseDate);

    let viewsDiv = document.createElement('div');
    viewsDiv.className = 'views';
    let views = document.createElement('span');
    views.textContent = movieData.viewCount + ' views';
    viewsDiv.appendChild(views);

    li.appendChild(titleDiv);
    li.appendChild(metadataDiv);
    li.appendChild(viewsDiv);

    return li;
}