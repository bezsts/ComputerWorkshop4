using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Runtime.Serialization;
using WebApp.Dtos.Movie;
using WebApp.Options;
using WebApp.Services.Contracts;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/movies")]
    [Produces("application/json")]
    public class MoviesController : Controller
    {
        private readonly IMovieService _service;
        private readonly ILogger<MoviesController> _logger;
        private readonly IOptionsSnapshot<ExportMoviesOptions> _exportOptions;

        public MoviesController(IMovieService service, ILogger<MoviesController> logger,
                                IOptionsSnapshot<ExportMoviesOptions> exportOptions)
        {
            _service = service;
            _logger = logger;
            _exportOptions = exportOptions;
        }

        /// <summary>
        /// Retrieves all movies.
        /// </summary>
        /// <returns>All Movies.</returns>
        /// <response code="200">Returns all Movies.</response>
        [HttpGet]
        public async Task<ActionResult<List<MovieViewsOutputDto>>> GetAllMovies()
        {
            var movies = await _service.GetAllMoviesAsync();

            _logger.LogInformation("{MethodName} returned {MoviesCount} movies",
                nameof(GetAllMovies), movies.Count);
            return Ok(movies);
        }

        /// <summary>
        /// Get a specific movie by ID.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve</param>
        /// <returns>The movie with the specified ID.</returns>
        /// <response code="200">Returns the movie with the specified ID.</response>
        /// <response code="404">The movie is not found.</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieViewsOutputDto>> GetMovieById(int id)
        {
            var movie = await _service.GetMovieByIdAsync(id);

            if (movie == null)
            {
                _logger.LogWarning("Movie with ID {Id} is not found in {MethodName}",
                    id, nameof(GetMovieById));

                return NotFound();
            }

            return Ok(movie);
        }

        /// <summary>
        /// Get a specific movie by title.
        /// </summary>
        /// <param name="title">The title of the movie to retrieve</param>
        /// <returns>The movie with the specified title.</returns>
        /// <response code="200">Returns the movie with the specified title.</response>
        /// <response code="404">The movie is not found.</response>
        [HttpGet("{title}")]
        public async Task<ActionResult<MovieViewsOutputDto>> GetMovieByTitle(string title)
        {
            var movie = await _service.GetMovieByTitleAsync(title);

            if (movie == null)
            {
                _logger.LogWarning("Movie with title {Title} is not found in {MethodName}",
                    title, nameof(GetMovieByTitle));
                return NotFound();
            }

            return Ok(movie);
        }

        /// <summary>
        /// Creates a new movie.
        /// </summary>
        /// <param name="movieDto">The Dto of movie object to be created</param>
        /// <returns>The created movie.</returns>
        /// <response code="201">The movie is successfully added.</response>
        /// <response code="400">The movie data is invalid.</response>
        [HttpPost]
        public async Task<ActionResult<MovieOutputDto>> CreateMovie(MovieCreateDto movieDto)
        {
            var movie = await _service.CreateMovieAsync(movieDto);

            _logger.LogInformation("{MethodName} created movie with ID {Id}",
                nameof(CreateMovie), movie.Id);

            return Created(nameof(CreateMovie), movie);
        }

        /// <summary>
        /// Updates an existing movie.
        /// </summary>
        /// <param name="id">The ID of the movie to update.</param>
        /// <param name="updatedMovie">The movie object containing the updated details.</param>
        /// <returns>The updated movie.</returns>
        /// <response code="200">The operation of updating is successful.</response>
        /// <response code="404">The movie is not found.</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<MovieOutputDto>> UpdateMovie(int id, MovieCreateDto updatedMovie)
        {
            var existingMovie = await _service.UpdateMovieAsync(id, updatedMovie);
            if (existingMovie == null)
            {
                _logger.LogWarning("Movie with ID {Id} is not found in {MethodName}",
                    id, nameof(UpdateMovie));

                return NotFound();
            }
            return Ok(existingMovie);
        }

        /// <summary>
        /// Deletes a movie by its id.
        /// </summary>
        /// <param name="id">The ID of the movie delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        /// <response code="204">The movie is successfully deleted.</response>
        /// <response code="404">The movie is not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var existingMovie = await _service.DeleteMovieAsync(id);
            if (existingMovie == null)
            {
                _logger.LogWarning("Movie with ID {Id} is not found in {MethodName}",
                    id, nameof(DeleteMovie));

                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Retrieves a list of popular movies, either from the cache or by querying the database.
        /// </summary>
        /// <returns>A list of popular movies as <see cref="MovieViewsOutputDto"/> objects.</returns>
        /// <response code="200">Returns a list of popular movies.</response>
        /// <response code="500">Internal server error if there is a failure during database query or caching.</response>
        [HttpGet("popular")]
        public async Task<ActionResult<List<MovieViewsOutputDto>>> GetPopularMovies()
        {
            var popularMovies = await _service.GetPopularMoviesAsync();

            if (popularMovies != null && popularMovies.Any())
            {
                _logger.LogInformation("{MethodName} successfully retrieved popular movies from cache.", nameof(GetPopularMovies));
            }
            else
            {
                _logger.LogWarning("{MethodName} no popular movies found in cache.", nameof(GetPopularMovies));
            }

            return Ok(popularMovies ?? new List<MovieViewsOutputDto>());
        }

        //TODO: додати коментарі
        //TODO: додати логування
        [HttpGet("download")]
        public async Task<ActionResult> ExportMoviesInCsv()
        {
            var data = await _service.ExportMovies();

            return File(data, "text/csv", _exportOptions.Value.FileName);
        }
    }
}
