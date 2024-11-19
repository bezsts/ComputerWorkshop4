using ApiDomain.Entities;
using ApiDomain.Repositories.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WebApp.Dtos.Movie;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/movies")]
    [Produces("application/json")]
    public class MoviesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMovieRepository _repository;

        public MoviesController(IMovieRepository repository, IMapper mapper) 
        { 
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all movies.
        /// </summary>
        /// <returns>All Movies.</returns>
        /// <response code="200">Returns all Movies.</response>
        [HttpGet]
        public Task<List<MovieOutputDto>> GetAllMovies() => 
            _mapper.ProjectTo<MovieOutputDto>(_repository.GetAll()).ToListAsync();

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
            var movie = await _repository.FindAsync(id);

            if (movie == null)
            { 
                return NotFound();
            }
            var movieDto = _mapper.Map<MovieOutputDto>(movie);
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
            var movie = await _repository.FindMovieByTitleAsync(title);

            if (movie == null)
            {
                return NotFound();
            }
            var movieDto = _mapper.Map<MovieOutputDto>(movie);
            return Ok(movieDto);
        }

        /// <summary>
        /// Creates a new movie.
        /// </summary>
        /// <param name="movie">The movie object to be created</param>
        /// <returns>The created movie.</returns>
        /// <response code="201">The movie is successfully added.</response>
        /// <response code="400">The movie data is invalid.</response>
        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieCreateDto movie)
        {
            await _repository.AddAsync(_mapper.Map<Movie>(movie));
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
            var existingMovie = await _repository.FindAsync(id);
            if (existingMovie == null)
            {
                return NotFound();
            }
            _mapper.Map(updatedMovie, existingMovie);
            await _repository.UpdateAsync(existingMovie);
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
            var existingMovie = await _repository.FindAsync(id);
            if (existingMovie == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(existingMovie);
            return NoContent();
        }
    }
}
