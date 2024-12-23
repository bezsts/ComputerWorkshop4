let list = document.getElementById('list');

let showAllMoviesButton = document.getElementById('all-movies-button');

let idMovieInput = document.getElementById('id-movie-input');
let getMovieButton = document.getElementById('get-movie-button');

let titleInput = document.getElementById('title-input');
let directorInput = document.getElementById('director-input');
let genreInput = document.getElementById('genre-select');
let isReleasedInput = document.getElementById('released-checkbox');
let releaseDateInput = document.getElementById('release-date');
let createMovieButton = document.getElementById('create-movie-button');

let showPopularMoviesButton = document.getElementById('popular-movies-button');

let exportButton = document.getElementById('export-movies-button');

showAllMoviesButton.addEventListener('click', loadAllMovies);
getMovieButton.addEventListener('click', loadMovie);
createMovieButton.addEventListener('click', createMovie);
showPopularMoviesButton.addEventListener('click', loadPopularMovies);
exportButton.addEventListener('click', ExportAllMovies);

// ---------------------------------

let showAllUsersButton = document.getElementById('all-users-button');

let idUserInput = document.getElementById('id-user-input');
let getUserButton = document.getElementById('id-user-button');

let nameInput = document.getElementById('name-input');
let emailInput = document.getElementById('email-input');
let createUserButton = document.getElementById('create-user-button');

let showWatchedMoviesButton = document.getElementById('watched-movies-button');

showAllUsersButton.addEventListener('click', loadAllUsers);
getUserButton.addEventListener('click', loadUser);
createUserButton.addEventListener('click', createUser);
showWatchedMoviesButton.addEventListener('click', loadWatchedMovies);

//window.onload = loadAllMovies();


async function loadAllMovies() {
    let moviesResponse = await fetch('http://localhost:5101/api/movies', {
        method: 'get'
    });

    if (!moviesResponse.ok) {
        return;
    }

    let moviesData = await moviesResponse.json();

    list.innerHTML = '';

    for (let i = 0; i < moviesData.length; i++) {
        list.appendChild(createMovieElement(moviesData[i]));
    }
}

async function loadMovie() {
    let movieId = idMovieInput.value;

    let moviesResponse = await fetch(`http://localhost:5101/api/movies/${movieId}`, {
        method: 'get'
    });

    if (!moviesResponse.ok) {
        return;
    }

    let moviesData = await moviesResponse.json();

    list.innerHTML = '';

    list.appendChild(createMovieElement(moviesData));
}

async function createMovie() {
    let movieData = {
        title: titleInput.value,
        director: directorInput.value,
        genre: genreInput.value,
        isReleased: isReleasedInput.checked,
        releaseDate: releaseDateInput.value
    };

    let moviesResponse = await fetch('http://localhost:5101/api/movies', {
        method: 'post',
        headers: {
            'accept': 'application/json',
            'Content-Type': 'application/json'
        },
        mode: 'cors',
        body: JSON.stringify(movieData)
    });

    if (!moviesResponse.ok) {
        return;
    }

    let moviesData = await moviesResponse.json();
    list.innerHTML = '';
    list.appendChild(createMovieElement(moviesData));
}

async function updateMovie(movieId)
{
    let movieData = {
        title: titleInput.value,
        director: directorInput.value,
        genre: genreInput.value,
        isReleased: isReleasedInput.checked,
        releaseDate: releaseDateInput.value
    };

    let moviesResponse = await fetch(`http://localhost:5101/api/movies/${movieId}`, {
        method: 'put',
        headers: {
            'accept': 'application/json',
            'Content-Type': 'application/json'
        },
        mode: 'cors',
        body: JSON.stringify(movieData)
    });

    if (!moviesResponse.ok) {
        return;
    }

    loadAllMovies();
}

async function deleteMovie(movieId)
{
    let response = await fetch(`http://localhost:5101/api/movies/${movieId}`, {
        method: 'delete'
    })

    if (!response.ok)
    {
        alert('Something went wrong!');
    }

    alert('Movie deleted successfully!');

    loadAllMovies();
}

async function loadPopularMovies() {
    let moviesResponse = await fetch('http://localhost:5101/api/movies/popular', {
        method: 'get'
    });

    if (!moviesResponse.ok) {
        return;
    }

    let moviesData = await moviesResponse.json();

    list.innerHTML = '';

    for (let i = 0; i < moviesData.length; i++) {
        list.appendChild(createMovieElement(moviesData[i]));
    }
}

async function ExportAllMovies() {
    let response = await fetch('http://localhost:5101/api/movies/download', {
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

//--------------------

async function loadAllUsers() {
    let response = await fetch('http://localhost:5101/api/users', {
        method: 'get'
    });

    if (!response.ok) {
        return;
    }

    let usersData = await response.json();

    list.innerHTML = '';

    for (let i = 0; i < usersData.length; i++) {
        list.appendChild(createUserElement(usersData[i]));
    }
}

async function loadUser() {
    let userId = idUserInput.value;

    let response = await fetch(`http://localhost:5101/api/users/${userId}`, {
        method: 'get'
    });

    if (!response.ok) {
        return;
    }

    let usersData = await response.json();

    list.innerHTML = '';

    list.appendChild(createUserElement(usersData));
}

async function createUser() {
    let userData = {
        name: nameInput.value,
        email: emailInput.value
    };

    let response = await fetch('http://localhost:5101/api/users', {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(userData)
    });

    if (!response.ok) {
        return;
    }

    let userResponseData = await response.json();

    list.innerHTML = '';

    list.appendChild(createUserElement(userResponseData));
}

async function updateUser(userId) {
    let userData = {
        name: nameInput.value,
        email: emailInput.value
    };

    let response = await fetch(`http://localhost:5101/api/users/${userId}`, {
        method: 'put',
        headers: {
            'accept': 'application/json',
            'Content-Type': 'application/json'
        },
        mode: 'cors',
        body: JSON.stringify(userData)
    });

    if (!response.ok) {
        return;
    }

    loadAllUsers();
}

async function deleteUser(userId) {
    let response = await fetch(`http://localhost:5101/api/users/${userId}`, {
        method: 'delete'
    })

    if (!response.ok) {
        alert('Something went wrong!');
    }

    alert('User deleted successfully!');

    loadAllUsers();
}

async function loadWatchedMovies() {
    let response = await fetch('http://localhost:5101/api/users/2/watched-movies', {
        method: 'get'
    });

    if (!response.ok) {
        return;
    }

    let moviesData = await response.json();

    list.innerHTML = '';

    for (let i = 0; i < moviesData.watchedMovies.length; i++) {
        list.appendChild(createWatchedMovieElement(moviesData.watchedMovies[i]));
    }
}

async function addWatchedMovie(movieId) {
    let response = await fetch(`http://localhost:5101/api/users/2/watched-movies/${movieId}`, {
        method: 'put'
    })

    if (!response.ok) {
        alert('Something went wrong!');
    }

    loadWatchedMovies();
}

async function removeWatchedMovie(movieId) {
    let response = await fetch(`http://localhost:5101/api/users/2/watched-movies/${movieId}`, {
        method: 'delete'
    })

    if (!response.ok) {
        alert('Something went wrong!');
    }

    loadWatchedMovies();
}

function createMovieElement(movieData) {
    let li = document.createElement('li');
    li.setAttribute('data-id', movieData.id);

    let titleDiv = document.createElement('div');
    titleDiv.className = 'title';
    let title = document.createElement('span');
    title.textContent = movieData.title;
    let updateButton = document.createElement('button');
    updateButton.className = 'edit-buttons';
    updateButton.type = 'button';
    updateButton.textContent = 'Update';
    updateButton.addEventListener('click', (event) => {
        event.stopPropagation();
        updateMovie(movieData.id);
    });
    let deleteButton = document.createElement('button');
    deleteButton.className = 'edit-buttons';
    deleteButton.type = 'button';
    deleteButton.textContent = 'Delete';
    deleteButton.addEventListener('click', (event) => {
        event.stopPropagation();
        deleteMovie(movieData.id);
    });
    let watchedButton = document.createElement('button');
    watchedButton.type = 'button';
    watchedButton.textContent = 'Watched';
    watchedButton.addEventListener('click', (event) => {
        event.stopPropagation();
        addWatchedMovie(movieData.id);
    });
    titleDiv.appendChild(title);
    titleDiv.appendChild(updateButton);
    titleDiv.appendChild(deleteButton);
    titleDiv.appendChild(watchedButton);

    let metadataDiv = document.createElement('div');
    metadataDiv.className = 'metadata';
    let director = document.createElement('span');
    director.textContent = movieData.director;
    director.className = 'metadata-director';
    let genre = document.createElement('span');
    genre.textContent = movieData.genre;
    genre.className = 'metadata-genre';
    let releaseDate = document.createElement('span');
    releaseDate.textContent = movieData.releaseDate;
    releaseDate.className = 'metadata-release-date';

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

function createWatchedMovieElement(movieData) {
    let li = document.createElement('li');
    li.setAttribute('data-id', movieData.id);

    let titleDiv = document.createElement('div');
    titleDiv.className = 'title';
    let title = document.createElement('span');
    title.textContent = movieData.title;
    let updateButton = document.createElement('button');
    updateButton.className = 'edit-buttons';
    updateButton.type = 'button';
    updateButton.textContent = 'Update';
    updateButton.addEventListener('click', (event) => {
        event.stopPropagation();
        updateMovie(movieData.id);
    });
    let deleteButton = document.createElement('button');
    deleteButton.className = 'edit-buttons';
    deleteButton.type = 'button';
    deleteButton.textContent = 'Delete';
    deleteButton.addEventListener('click', (event) => {
        event.stopPropagation();
        deleteMovie(movieData.id);
    });
    let unWatchedButtonunWatchedButton = document.createElement('button');
    unWatchedButtonunWatchedButton.type = 'button';
    unWatchedButtonunWatchedButton.textContent = 'Unwatched';
    unWatchedButtonunWatchedButton.addEventListener('click', (event) => {
        event.stopPropagation();
        removeWatchedMovie(movieData.id);
    });
    titleDiv.appendChild(title);
    titleDiv.appendChild(updateButton);
    titleDiv.appendChild(deleteButton);
    titleDiv.appendChild(unWatchedButtonunWatchedButton);

    let metadataDiv = document.createElement('div');
    metadataDiv.className = 'metadata';
    let director = document.createElement('span');
    director.textContent = movieData.director;
    director.className = 'metadata-director';
    let genre = document.createElement('span');
    genre.textContent = movieData.genre;
    genre.className = 'metadata-genre';
    let releaseDate = document.createElement('span');
    releaseDate.textContent = movieData.releaseDate;
    releaseDate.className = 'metadata-release-date';

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

function createUserElement(userData) {
    let li = document.createElement('li');
    li.setAttribute('data-id', userData.id);

    let titleDiv = document.createElement('div');
    titleDiv.className = 'title';
    let name = document.createElement('p');
    name.textContent = userData.name;
    let updateButton = document.createElement('button');
    updateButton.className = 'edit-buttons';
    updateButton.type = 'button';
    updateButton.textContent = 'Update';
    updateButton.addEventListener('click', (event) => {
        event.stopPropagation();
        updateUser(userData.id);
    });
    let deleteButton = document.createElement('button');
    deleteButton.className = 'edit-buttons';
    deleteButton.type = 'button';
    deleteButton.textContent = 'Delete';
    deleteButton.addEventListener('click', (event) => {
        event.stopPropagation();
        deleteUser(userData.id);
    });

    titleDiv.appendChild(name);
    titleDiv.appendChild(updateButton);
    titleDiv.appendChild(deleteButton);

    let email = document.createElement('p');
    email.textContent = userData.email;

    li.appendChild(titleDiv);
    li.appendChild(email);

    return li;
}