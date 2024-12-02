using ApiDomain.Entities;
using ApiDomain.Repositories.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Dtos.Movie;
using WebApp.Dtos.User;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<UsersController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>All Users.</returns>
        /// <response code="200">Returns all Users.</response>
        [HttpGet]
        public async Task<List<UserOutputDto>> GetAllUsers()
        {
            var users = await _mapper.ProjectTo<UserOutputDto>(_unitOfWork.Users.GetAll()).ToListAsync();

            _logger.LogInformation("{MethodName} returned {UsersCount} users",
                    nameof(GetAllUsers), users.Count);

            return users;
        }

        /// <summary>
        /// Get a specific user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve</param>
        /// <returns>The user with the specified ID.</returns>
        /// <response code="200">Returns the user with the specified ID.</response>
        /// <response code="404">The user is not found.</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserOutputDto>> GetUserById(int id)
        {
            var user = await _unitOfWork.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} is not found in {MethodName}",
                    id, nameof(GetUserById));

                return NotFound();
            }
            var userDto = _mapper.Map<UserOutputDto>(user);

            _logger.LogInformation("{MethodName} returned user by ID {Id}",
                nameof(GetUserById), id);

            return Ok(userDto);
        }

        /// <summary>
        /// Get a specific user by name.
        /// </summary>
        /// <param name="name">The name of the user to retrieve</param>
        /// <returns>The user with the specified name.</returns>
        /// <response code="200">Returns the user with the specified name.</response>
        /// <response code="404">The user is not found.</response>
        [HttpGet("{name}")]
        public async Task<ActionResult<UserOutputDto>> GetUserByName(string name)
        {
            var user = await _unitOfWork.Users.FindByNameAsync(name);
            if (user == null)
            {
                _logger.LogWarning("User with name {Name} is not found in {MethodName}",
                    name, nameof(GetUserByName));
                return NotFound();
            }
            var userDto = _mapper.Map<UserOutputDto>(user);

            _logger.LogInformation("{MethodName} returned user by name {Id} {Name}",
                nameof(GetUserByName), userDto.Id, userDto.Name);

            return Ok(userDto);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userDto">The Dto of user object to be created</param>
        /// <returns>The created user.</returns>
        /// <response code="201">The user is successfully added.</response>
        /// <response code="400">The user data is invalid.</response>
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.Users.AddAsync(_mapper.Map<User>(userDto));

            _logger.LogInformation("{MethodName} created user with ID {Id}",
                nameof(CreateUser), user.Id);

            return Created();
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updatedUser">The user object containing the updated details.</param>
        /// <returns>The updated user.</returns>
        /// <response code="200">The operation of updating is successful.</response>
        /// <response code="404">The user is not found.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserCreateDto updatedUser)
        {
            var existingUser = await _unitOfWork.Users.FindAsync(id);

            if (existingUser == null)
            {
                _logger.LogWarning("User with ID {Id} is not found in {MethodName}",
                    id, nameof(UpdateUser));

                return NotFound();
            }

            _mapper.Map(updatedUser, existingUser);
            await _unitOfWork.Users.UpdateAsync(existingUser);

            _logger.LogInformation("{MethodName} updated user with ID {Id}",
                nameof(UpdateUser), id);

            return Ok();
        }

        /// <summary>
        /// Deletes a user by its id.
        /// </summary>
        /// <param name="id">The ID of the user delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        /// <response code="204">The user is successfully deleted.</response>
        /// <response code="404">The user is not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await _unitOfWork.Users.FindAsync(id);

            if (existingUser == null)
            {
                _logger.LogWarning("User with ID {Id} is not found in {MethodName}",
                    id, nameof(DeleteUser));

                return NotFound();
            }

            await _unitOfWork.Users.DeleteAsync(existingUser);

            _logger.LogInformation("{MethodName} deleted user with ID {Id}",
                nameof(DeleteUser), id);

            return NoContent();
        }

        /// <summary>
        /// Get a list of watched movies of a specific user by ID.
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns>The list of movies the user has watched.</returns>
        /// <response code="200">Returns the list of watched movies of the user with the specified ID.</response>
        /// <response code="404">The user is not found.</response>
        [HttpGet("{id}/watched-movies")]
        public async Task<IActionResult> GetWatchedMoviesOfUser(int id)
        {
            var user = await _unitOfWork.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} is not found in {MethodName}",
                    id, nameof(GetWatchedMoviesOfUser));

                return NotFound();
            }
            var watchedMoviesList = _mapper.Map<List<MovieOutputDto>>(user.WatchedMovies);

            _logger.LogInformation("{MethodName} returned {MoviesCount} watched movies of user with ID {Id}",
                    nameof(GetWatchedMoviesOfUser), watchedMoviesList.Count, id);

            return Ok(watchedMoviesList);
        }

        /// <summary>
        /// Adds a movie to the watched list of a specific user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <param name="movieId">The ID of the movie to add to the watched list.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        /// <response code="200">The movie was successfully added to the user's watched list.</response>
        /// <response code="404">The user or movie was not found.</response>
        /// <response code="409">The movie is already in the user's watched list.</response>
        [HttpPut("{id}/watched-movies/{movieId}")]
        public async Task<IActionResult> AddWatchedMovie(int id, int movieId)
        {
            var user = await _unitOfWork.Users.FindAsync(id);
            var movie = await _unitOfWork.Movies.FindAsync(movieId);

            if (user is null)
            {
                _logger.LogWarning("User with ID {UserId} is not found in {MethodName}",
                    id, nameof(AddWatchedMovie));

                return NotFound();
            }

            if (movie is null)
            {
                _logger.LogWarning("Movie with ID {MovieId} is not found in {MethodName}",
                    movieId, nameof(AddWatchedMovie));

                return NotFound();
            }

            if (user.WatchedMovies.Contains(movie))
            {
                _logger.LogWarning("User with ID {UserId} already has movie with ID {MovieId} in watched movies in {MethodName}",
                    id, movieId, nameof(AddWatchedMovie));
                return Conflict();
            }

            user.WatchedMovies.Add(movie);

            _logger.LogInformation("Movie with ID {MovieId} is added to watched movies of user with ID {UserId} in {MethodName}",
                movieId, id, nameof(AddWatchedMovie));

            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Removes a movie from the user's list of watched movies by its ID.
        /// </summary>
        /// <param name="id">The ID of the user whose watched movies list is being updated.</param>
        /// <param name="movieId">The ID of the movie to remove from the user's watched movies list.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        /// <response code="204">The movie was successfully removed from the user's watched movies list.</response>
        /// <response code="404">The user or the movie was not found, or the movie is not in the user's watched movies list.</response>
        [HttpDelete("{id}/watched-movies/{movieId}")]
        public async Task<IActionResult> RemoveWatchedMovie(int id, int movieId)
        {
            var user = await _unitOfWork.Users.FindAsync(id);
            var movie = await _unitOfWork.Movies.FindAsync(movieId);

            if (user is null)
            {
                _logger.LogWarning("User with ID {UserId} is not found in {MethodName}",
                    id, nameof(RemoveWatchedMovie));

                return NotFound();
            }

            if (movie is null)
            {
                _logger.LogWarning("Movie with ID {MovieId} is not found in {MethodName}",
                    movieId, nameof(RemoveWatchedMovie));

                return NotFound();
            }

            if (!user.WatchedMovies.Contains(movie))
            {
                _logger.LogWarning("Movie with ID {MovieId} is not found in watched movies of user with ID {UserId} in {MethodName}",
                    movieId, id, nameof(RemoveWatchedMovie));
                return NotFound();
            }

            user.WatchedMovies.Remove(movie);

            _logger.LogInformation("Movie with ID {MovieId} is removed from watched movies of user with ID {UserId} in {MethodName}",
                movieId, id, nameof(RemoveWatchedMovie));

            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
    }
}
