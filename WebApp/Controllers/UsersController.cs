using ApiDomain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using WebApp.Dtos.User;
using WebApp.Services.Contracts;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private readonly IUserService _service;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService service, ILogger<UsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>All Users.</returns>
        /// <response code="200">Returns all Users.</response>
        [HttpGet]
        public async Task<ActionResult<List<UserWatchedMoviesCountOutputDto>>> GetAllUsers()
        {
            var users = await _service.GetAllUsersAsync();

            _logger.LogInformation("{MethodName} returned {UsersCount} users",
                    nameof(GetAllUsers), users.Count);

            return Ok(users);
        }

        /// <summary>
        /// Get a specific user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve</param>
        /// <returns>The user with the specified ID.</returns>
        /// <response code="200">Returns the user with the specified ID.</response>
        /// <response code="404">The user is not found.</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserWatchedMoviesCountOutputDto>> GetUserById(int id)
        {
            var user = await _service.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} is not found in {MethodName}",
                    id, nameof(GetUserById));

                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Get a specific user by name.
        /// </summary>
        /// <param name="name">The name of the user to retrieve</param>
        /// <returns>The user with the specified name.</returns>
        /// <response code="200">Returns the user with the specified name.</response>
        /// <response code="404">The user is not found.</response>
        [HttpGet("{name}")]
        public async Task<ActionResult<UserWatchedMoviesCountOutputDto>> GetUserByName(string name)
        {
            var user = await _service.GetUserByNameAsync(name);
            if (user == null)
            {
                _logger.LogWarning("User with name {Name} is not found in {MethodName}",
                    name, nameof(GetUserByName));
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userDto">The Dto of user object to be created</param>
        /// <returns>The created user.</returns>
        /// <response code="201">The user is successfully added.</response>
        /// <response code="400">The user data is invalid.</response>
        [HttpPost]
        public async Task<ActionResult<UserWatchedMoviesCountOutputDto>> CreateUser(UserCreateDto userDto)
        {
            var user = await _service.CreateUserAsync(userDto);

            _logger.LogInformation("{MethodName} created user with ID {Id}",
                nameof(CreateUser), user.Id);

            return Created(nameof(CreateUser), user);
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
        public async Task<ActionResult<UserWatchedMoviesCountOutputDto>> UpdateUser(int id, UserCreateDto updatedUser)
        {
            var existingUser = await _service.UpdateUserAsync(id, updatedUser);

            if (existingUser == null)
            {
                _logger.LogWarning("User with ID {Id} is not found in {MethodName}",
                    id, nameof(UpdateUser));

                return NotFound();
            }
            return Ok(existingUser);
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
            var existingUser = await _service.DeleteUserAsync(id);

            if (existingUser == null)
            {
                _logger.LogWarning("User with ID {Id} is not found in {MethodName}",
                    id, nameof(DeleteUser));

                return NotFound();
            }

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
        public async Task<ActionResult<UserWatchedMoviesListOutputDto>> GetWatchedMoviesOfUser(int id)
        {
            var user = await _service.GetWatchedMoviesOfUserAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} is not found in {MethodName}",
                    id, nameof(GetWatchedMoviesOfUser));

                return NotFound();
            }

            return Ok(user);
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
        public async Task<ActionResult<UserWatchedMoviesListOutputDto>> AddWatchedMovie(int id, int movieId)
        {
            UserWatchedMoviesListOutputDto user = new UserWatchedMoviesListOutputDto();
            try
            {
                user = await _service.AddWatchedMovieAsync(id, movieId);
            }
            catch (UserNotFoundException)
            {
                _logger.LogWarning("User with ID {UserId} is not found in {MethodName}",
                    id, nameof(AddWatchedMovie));

                return NotFound();
            }
            catch (MovieNotFoundException)
            {
                _logger.LogWarning("Movie with ID {MovieId} is not found in {MethodName}",
                    movieId, nameof(AddWatchedMovie));

                return NotFound();
            }
            catch (MovieConflictException)
            {
                _logger.LogWarning("User with ID {UserId} already has movie with ID {MovieId} in watched movies in {MethodName}",
                    id, movieId, nameof(AddWatchedMovie));
                return Conflict();
            }
            return Ok(user);
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
            try
            {
                var user = await _service.RemoveWatchedMovieAsync(id, movieId);
            }
            catch (UserNotFoundException)
            {
                _logger.LogWarning("User with ID {UserId} is not found in {MethodName}",
                    id, nameof(AddWatchedMovie));

                return NotFound();
            }
            catch (MovieNotFoundException)
            {
                _logger.LogWarning("Movie with ID {MovieId} is not found in {MethodName}",
                    movieId, nameof(AddWatchedMovie));

                return NotFound();
            }
            return NoContent();
        }
    }
}
