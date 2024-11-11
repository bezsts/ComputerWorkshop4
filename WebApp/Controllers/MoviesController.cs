using ApiDomain.Entities;
using ApiDomain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/movies")]
    [Produces("application/json")]
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _repository;

        public MoviesController(IMovieRepository service) => _repository = service;

        /// <summary>
        /// Retrieves all movies.
        /// </summary>
        /// <returns>All Movies.</returns>
        /// <response code="200">Returns all Movies.</response>
        [HttpGet]
        public IEnumerable<Movie> GetAllMovies() => _repository.GetAll();

        /// <summary>
        /// Get a specific movie by ID.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve</param>
        /// <returns>The movie with the specified ID.</returns>
        /// <response code="200">Returns the movie with the specified ID.</response>
        /// <response code="404">The movie is not found.</response>
        [HttpGet("{id}")]
        public IActionResult GetMovie(int id)
        {
            var movie = _repository.Get(id);
            return movie == null ? NotFound() : Ok(movie);
        }

        /// <summary>
        /// Creates a new movie.
        /// </summary>
        /// <param name="movie">The movie object to be created</param>
        /// <returns>The created movie.</returns>
        /// <response code="201">The movie is successfully added.</response>
        /// <response code="400">The movie data is invalid.</response>
        [HttpPost]
        public IActionResult CreateMovie(Movie movie) => CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, _repository.Create(movie));

        /// <summary>
        /// Updates an existing movie.
        /// </summary>
        /// <param name="id">The ID of the movie to update.</param>
        /// <param name="updatedMovie">The movie object containing the updated details.</param>
        /// <returns>The updated movie.</returns>
        /// <response code="200">The operation of updating is successful.</response>
        /// <response code="404">The movie is not found.</response>
        [HttpPut("{id}")]
        public IActionResult UpdateMovie(int id, Movie updatedMovie)
        {
            var result = _repository.Update(id, updatedMovie);
            return result ? Ok(result) : NotFound();
        }

        /// <summary>
        /// Deletes a movie by its id.
        /// </summary>
        /// <param name="id">The ID of the movie delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        /// <response code="204">The movie is successfully deleted.</response>
        /// <response code="404">The movie is not found.</response>
        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id) => _repository.Delete(id) ? NoContent() : NotFound();
    }
}
