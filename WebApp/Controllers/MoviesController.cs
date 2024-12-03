using ApiDomain.Entities;
using ApiDomain.Repositories.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApp.Dtos.Movie;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/movies")]
    [Produces("application/json")]
    public class MoviesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MoviesController> _logger;
        private readonly IMemoryCache _memoryCache;

        public MoviesController(IUnitOfWork unitOfWork, IMapper mapper, 
                                ILogger<MoviesController> logger, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Retrieves all movies.
        /// </summary>
        /// <returns>All Movies.</returns>
        /// <response code="200">Returns all Movies.</response>
        [HttpGet]
        public async Task<List<MovieOutputDto>> GetAllMovies()
        {
            var movies = await _mapper.ProjectTo<MovieOutputDto>(_unitOfWork.Movies.GetAll()).ToListAsync();

            _logger.LogInformation("{MethodName} returned {MoviesCount} movies",
                nameof(GetAllMovies), movies.Count);
            return movies;
        }

        /// <summary>
        /// Get a specific movie by ID.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve</param>
        /// <returns>The movie with the specified ID.</returns>
        /// <response code="200">Returns the movie with the specified ID.</response>
        /// <response code="404">The movie is not found.</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieOutputDto>> GetMovieById(int id)
        {
            var movie = await _unitOfWork.Movies.FindAsync(id);

            if (movie == null)
            {
                _logger.LogWarning("Movie with ID {Id} is not found in {MethodName}",
                    id, nameof(GetMovieById));

                return NotFound();
            }

            var movieDto = _mapper.Map<MovieOutputDto>(movie);
            _logger.LogInformation("{MethodName} returned movie by ID {Id}",
                nameof(GetMovieById), id);

            return Ok(movieDto);
        }

        /// <summary>
        /// Get a specific movie by title.
        /// </summary>
        /// <param name="title">The title of the movie to retrieve</param>
        /// <returns>The movie with the specified title.</returns>
        /// <response code="200">Returns the movie with the specified title.</response>
        /// <response code="404">The movie is not found.</response>
        [HttpGet("{title}")]
        public async Task<ActionResult<MovieOutputDto>> GetMovieByTitle(string title)
        {
            var movie = await _unitOfWork.Movies.FindMovieByTitleAsync(title);

            if (movie == null)
            {
                _logger.LogWarning("Movie with title {Title} is not found in {MethodName}",
                    title, nameof(GetMovieByTitle));
                return NotFound();
            }
            var movieDto = _mapper.Map<MovieOutputDto>(movie);
            _logger.LogInformation("{MethodName} returned movie by title {Id} {Title}",
                nameof(GetMovieById), movieDto.Id, movieDto.Title);
            return Ok(movieDto);
        }

        /// <summary>
        /// Creates a new movie.
        /// </summary>
        /// <param name="movieDto">The Dto of movie object to be created</param>
        /// <returns>The created movie.</returns>
        /// <response code="201">The movie is successfully added.</response>
        /// <response code="400">The movie data is invalid.</response>
        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieCreateDto movieDto)
        {
            var movie = _mapper.Map<Movie>(movieDto);
            await _unitOfWork.Movies.AddAsync(movie);

            _logger.LogInformation("{MethodName} created movie with ID {Id}",
                nameof(CreateMovie), movie.Id);

            return Created();
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
        public async Task<IActionResult> UpdateMovie(int id, MovieCreateDto updatedMovie)
        {
            var existingMovie = await _unitOfWork.Movies.FindAsync(id);
            if (existingMovie == null)
            {
                _logger.LogWarning("Movie with ID {Id} is not found in {MethodName}",
                    id, nameof(UpdateMovie));

                return NotFound();
            }
            _mapper.Map(updatedMovie, existingMovie);
            await _unitOfWork.Movies.UpdateAsync(existingMovie);

            _logger.LogInformation("{MethodName} updated movie with ID {Id}",
                nameof(UpdateMovie), id);

            return Ok();
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
            var existingMovie = await _unitOfWork.Movies.FindAsync(id);
            if (existingMovie == null)
            {
                _logger.LogWarning("Movie with ID {Id} is not found in {MethodName}",
                    id, nameof(DeleteMovie));

                return NotFound();
            }

            await _unitOfWork.Movies.DeleteAsync(existingMovie);

            _logger.LogInformation("{MethodName} deleted movie with ID {Id}",
                nameof(DeleteMovie), id);

            return NoContent();
        }

        /// <summary>
        /// Retrieves a list of popular movies, either from the cache or by querying the database.
        /// </summary>
        /// <returns>A list of popular movies as <see cref="MovieOutputDto"/> objects.</returns>
        /// <response code="200">Returns a list of popular movies.</response>
        /// <response code="500">Internal server error if there is a failure during database query or caching.</response>
        [HttpGet("popular")]
        public async Task<List<MovieOutputDto>> GetPopularMovies()
        {
            var popularMovies = await _memoryCache.GetOrCreateAsync(nameof(GetPopularMovies), async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);


                var popularMoviesQuery = _unitOfWork.Movies.GetMostPopularMovies();
                return await _mapper.ProjectTo<MovieOutputDto>(popularMoviesQuery).ToListAsync();
            });

            if (popularMovies != null && popularMovies.Any())
            {
                _logger.LogInformation("{MethodName} successfully retrieved popular movies from cache.", nameof(GetPopularMovies));
            }
            else
            {
                _logger.LogWarning("{MethodName} no popular movies found in cache.", nameof(GetPopularMovies));
            }

            return popularMovies ?? new List<MovieOutputDto>();
        }
    }
}
