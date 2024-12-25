class ButtonService{
    createButton(text, onClick) {
        const button = document.createElement('button');
        button.textContent = text;
        button.type = 'button';
        button.addEventListener('click', (event) => {
            event.stopPropagation();
            onClick();
        });
        return button;
    }

    createMetadataElement(label, value) {
        const span = document.createElement('span');
        span.className = `metadata-${label.toLowerCase()}`;
        span.textContent = `${label}: ${value}`;
        return span;
    }
}

class MovieService {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
        this.listElement = document.getElementById('list');
    }

    async fetchMovies(endpoint) {
        const response = await fetch(`${this.baseUrl}/${endpoint}`);
        if (!response.ok) {
            throw new Error('Failed to fetch data');
        }
        return response.json();
    }

    async loadAllMovies() {
        try {
            const movies = await this.fetchMovies('movies');
            this.renderMovies(movies);
        } catch (error) {
            console.error(error);
        }
    }

    async loadPopularMovies() {
        try {
            const movies = await this.fetchMovies('movies/popular');
            this.renderMovies(movies);
        } catch (error) {
            console.error(error);
        }
    }

    async loadMovie(movieId) {
        try {
            const movie = await this.fetchMovies(`movies/${movieId}`);
            this.renderMovies([movie]);
        } catch (error) {
            console.error(error);
        }
    }

    async createMovie(movieData) {
        try {
            const response = await fetch(`${this.baseUrl}/movies`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(movieData),
            });
            if (!response.ok) {
                throw new Error('Failed to create movie');
            }
            const movie = await response.json();
            this.renderMovies([movie]);
        } catch (error) {
            console.error(error);
        }
    }

    async updateMovie(movieId, movieData) {
        try {
            const response = await fetch(`${this.baseUrl}/movies/${movieId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(movieData),
            });
            if (!response.ok) {
                throw new Error('Failed to update movie');
            }
            this.loadAllMovies();
        } catch (error) {
            console.error(error);
        }
    }

    async deleteMovie(movieId) {
        try {
            const response = await fetch(`${this.baseUrl}/movies/${movieId}`, {
                method: 'DELETE',
            });
            if (!response.ok) {
                throw new Error('Failed to delete movie');
            }
            alert('Movie deleted successfully!');
            this.loadAllMovies();
        } catch (error) {
            console.error(error);
        }
    }

    async exportAllMovies()
    {
        try {
            let response = await fetch('http://localhost:5101/api/movies/download', {
                method: 'get'
            });

            let blob = await response.blob();
            let url = URL.createObjectURL(blob);

            let link = document.createElement('a');
            link.href = url;
            link.download = 'movies.csv';
            link.click();

            URL.revokeObjectURL(url);
        } catch (error) {
            console.error(error);
        }
    }

    async addWatchedMovie(userId, movieId) {
        try {
            const response = await fetch(`${this.baseUrl}/users/${userId}/watched-movies/${movieId}`, {
                method: 'PUT',
            });
            if (!response.ok) {
                throw new Error('Failed to add watched movie');
            }
            this.loadWatchedMovies(userId);
        } catch (error) {
            console.error(error);
        }
    }

    async removeWatchedMovie(userId, movieId) {
        try {
            const response = await fetch(`${this.baseUrl}/users/${userId}/watched-movies/${movieId}`, {
                method: 'DELETE',
            });
            if (!response.ok) {
                throw new Error('Failed to remove watched movie');
            }
            this.loadWatchedMovies(userId);
        } catch (error) {
            console.error(error);
        }
    }

    async loadWatchedMovies(userId) {
        try {
            const data = await this.fetchMovies(`users/${userId}/watched-movies`);
            this.renderMovies(data.watchedMovies, true);
        } catch (error) {
            console.error(error);
        }
    }

    createMovieElement(movie, isWatched = false) {
        const li = document.createElement('li');
        li.setAttribute('data-id', movie.id);

        const titleDiv = document.createElement('div');
        titleDiv.className = 'title';
        const title = document.createElement('span');
        title.textContent = movie.title;

        // Динамічні кнопки
        const actionButtons = [
            buttonService.createButton('Update', () => this.updateMovie(movie.id, this.getMovieDataFromInputs())),
            buttonService.createButton('Delete', () => this.deleteMovie(movie.id)),
            isWatched
                ? buttonService.createButton('Unwatched', () => this.removeWatchedMovie(2, movie.id))
                : buttonService.createButton('Watched', () => this.addWatchedMovie(2, movie.id))
        ];

        titleDiv.append(title, ...actionButtons);

        const metadataDiv = this.createMetadataDiv(movie);

        const viewsDiv = document.createElement('div');
        viewsDiv.className = 'views';
        const views = document.createElement('span');
        views.textContent = `${movie.viewCount} views`;
        viewsDiv.appendChild(views);

        li.append(titleDiv, metadataDiv, viewsDiv);

        return li;
    }

    createMetadataDiv(movie) {
        const metadataDiv = document.createElement('div');
        metadataDiv.className = 'metadata';

        const metadataElements = [
            buttonService.createMetadataElement('Director', movie.director),
            buttonService.createMetadataElement('Genre', movie.genre),
            buttonService.createMetadataElement('Release Date', movie.releaseDate)
        ];

        metadataElements.forEach((element, index) => {
            const classNames = ['metadata-director', 'metadata-genre', 'metadata-release-date'];
            element.className = classNames[index];
            metadataDiv.appendChild(element);
        });

        return metadataDiv;
    }

    renderMovies(movies, isWatched = false) {
        this.listElement.innerHTML = '';
        movies.forEach((movie) => {
            this.listElement.appendChild(this.createMovieElement(movie, isWatched));
        });
    }

    getMovieDataFromInputs() {
        return {
            title: document.getElementById('title-input').value,
            director: document.getElementById('director-input').value,
            genre: document.getElementById('genre-select').value,
            isReleased: document.getElementById('released-checkbox').checked,
            releaseDate: document.getElementById('release-date').value,
        };
    }
}

class UserService {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
        this.listElement = document.getElementById('list');
    }

    async fetchUsers(endpoint) {
        const response = await fetch(`${this.baseUrl}/${endpoint}`);
        if (!response.ok) {
            throw new Error('Failed to fetch data');
        }
        return response.json();
    }

    async loadAllUsers() {
        try {
            const users = await this.fetchUsers('users');
            this.renderUsers(users);
        } catch (error) {
            console.error(error);
        }
    }

    async loadUser(userId) {
        try {
            const user = await this.fetchUsers(`users/${userId}`);
            this.renderUsers([user]);
        } catch (error) {
            console.error(error);
        }
    }

    async createUser(userData) {
        try {
            const response = await fetch(`${this.baseUrl}/users`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(userData),
            });
            if (!response.ok) {
                throw new Error('Failed to create user');
            }
            const user = await response.json();
            this.renderUsers([user]);
        } catch (error) {
            console.error(error);
        }
    }

    async updateUser(userId, userData) {
        try {
            const response = await fetch(`${this.baseUrl}/users/${userId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(userData),
            });
            if (!response.ok) {
                throw new Error('Failed to update user');
            }
            this.loadAllUsers();
        } catch (error) {
            console.error(error);
        }
    }

    async deleteUser(userId) {
        try {
            const response = await fetch(`${this.baseUrl}/users/${userId}`, {
                method: 'DELETE',
            });
            if (!response.ok) {
                throw new Error('Failed to delete user');
            }
            alert('User deleted successfully!');
            this.loadAllUsers();
        } catch (error) {
            console.error(error);
        }
    }

    createUserElement(user) {
        const li = document.createElement('li');
        li.setAttribute('data-id', user.id);

        const nameDiv = document.createElement('div');
        nameDiv.className = 'title';
        const name = document.createElement('span');
        name.textContent = user.name;

        const updateButton = buttonService.createButton('Update', () => this.updateUser(user.id, this.getUserDataFromInputs()));
        const deleteButton = buttonService.createButton('Delete', () => this.deleteUser(user.id));

        nameDiv.append(name, updateButton, deleteButton);

        const metadataDiv = document.createElement('div');
        metadataDiv.className = 'metadata';

        const email = buttonService.createMetadataElement('Email', user.email);

        metadataDiv.append(email);

        li.append(nameDiv, metadataDiv);

        return li;
    }

    renderUsers(users) {
        this.listElement.innerHTML = '';
        users.forEach((user) => {
            this.listElement.appendChild(this.createUserElement(user));
        });
    }

    getUserDataFromInputs() {
        return {
            name: document.getElementById('name-input').value,
            email: document.getElementById('email-input').value,
        };
    }
}

const baseUrl = 'http://localhost:5101/api';
const movieService = new MovieService(baseUrl);
const userService = new UserService(baseUrl);
const buttonService = new ButtonService();

// Ініціалізація DOM елементів і кнопок
let showAllMoviesButton = document.getElementById('all-movies-button');
let idMovieInput = document.getElementById('id-movie-input');
let getMovieButton = document.getElementById('get-movie-button');
let createMovieButton = document.getElementById('create-movie-button');
let showPopularMoviesButton = document.getElementById('popular-movies-button');
let exportButton = document.getElementById('export-movies-button');

let showAllUsersButton = document.getElementById('all-users-button');
let idUserInput = document.getElementById('id-user-input');
let getUserButton = document.getElementById('id-user-button');
let createUserButton = document.getElementById('create-user-button');
let showWatchedMoviesButton = document.getElementById('watched-movies-button');

// Додавання обробників подій
showAllMoviesButton.addEventListener('click', () => movieService.loadAllMovies());
getMovieButton.addEventListener('click', () => movieService.loadMovie(idMovieInput.value));
createMovieButton.addEventListener('click', () => movieService.createMovie(movieService.getMovieDataFromInputs()));
showPopularMoviesButton.addEventListener('click', () => movieService.loadPopularMovies());
exportButton.addEventListener('click', () => movieService.exportAllMovies());

showAllUsersButton.addEventListener('click', () => userService.loadAllUsers());
getUserButton.addEventListener('click', () => userService.loadUser(idUserInput.value));
createUserButton.addEventListener('click', () => userService.createUser(userService.getUserDataFromInputs()));
showWatchedMoviesButton.addEventListener('click', () => movieService.loadWatchedMovies(2));

movieService.loadAllMovies();