using Microsoft.AspNetCore.Mvc;
using WebApp.Dtos.Movie;

namespace WebApp.Services.Contracts
{
    public interface IMovieService
    {
        Task<List<MovieViewsOutputDto>> GetAllMoviesAsync();
        Task<MovieViewsOutputDto> GetMovieByIdAsync(int id);
        Task<MovieViewsOutputDto> GetMovieByTitleAsync(string title);
        Task<MovieOutputDto> CreateMovieAsync(MovieCreateDto movieDto);
        Task<MovieOutputDto?> UpdateMovieAsync(int id, MovieCreateDto updatedMovie);
        Task<MovieViewsOutputDto?> DeleteMovieAsync(int id);
        Task<List<MovieViewsOutputDto>?> GetPopularMoviesAsync();
        Task<byte[]> ExportMovies();
    }
}
