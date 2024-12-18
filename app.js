let allMoviesTableBody = document.getElementById('all-movies-table').getElementsByTagName('tbody')[0];
let popularMoviesTableBody = document.getElementById('popular-movies-table').getElementsByTagName('tbody')[0];
let exportButton = document.getElementById('export-movies-button');
window.onload = function () {
    loadAllMovies();
    loadPopularMovies();
};
exportButton.addEventListener('click', ExportAllMovies);

async function loadAllMovies() {
    let moviesResponse = await fetch('https://localhost:5001/api/movies', {
        method: 'get'
    });

    if (!moviesResponse.ok) {
        return;
    }

    let moviesData = await moviesResponse.json();

    for (let i = 0; i < moviesData.length; i++) {
        allMoviesTableBody.appendChild(createMovieElement(moviesData[i]));
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

    for (let i = 0; i < moviesData.length; i++) {
        popularMoviesTableBody.appendChild(createMovieElement(moviesData[i]));
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
    let title = document.createElement('td');
    title.textContent = movieData.title;

    let director = document.createElement('td');
    director.textContent = movieData.director;

    let genre = document.createElement('td');
    genre.textContent = movieData.genre;

    let releaseDate = document.createElement('td');
    releaseDate.textContent = movieData.releaseDate;

    let viewCount = document.createElement('td');
    viewCount.textContent = movieData.viewCount;

    let tableRow = document.createElement('tr');
    tableRow.appendChild(title);
    tableRow.appendChild(director);
    tableRow.appendChild(genre);
    tableRow.appendChild(releaseDate);
    tableRow.appendChild(viewCount);

    return tableRow;
}