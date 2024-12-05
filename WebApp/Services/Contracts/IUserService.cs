using Microsoft.AspNetCore.Mvc;
using WebApp.Dtos.Movie;
using WebApp.Dtos.User;

namespace WebApp.Services.Contracts
{
    public interface IUserService
    {
        Task<List<UserWatchedMoviesCountOutputDto>> GetAllUsersAsync();
        Task<UserWatchedMoviesCountOutputDto> GetUserByIdAsync(int id);
        Task<UserWatchedMoviesCountOutputDto> GetUserByNameAsync(string name);
        Task<UserOutputDto> CreateUserAsync(UserCreateDto userDto);
        Task<UserOutputDto?> UpdateUserAsync(int id, UserCreateDto updatedUser);
        Task<UserOutputDto?> DeleteUserAsync(int id);
        Task<UserWatchedMoviesListOutputDto?> GetWatchedMoviesOfUserAsync(int id);
        Task<UserWatchedMoviesListOutputDto> AddWatchedMovieAsync(int id, int movieId);
        Task<UserWatchedMoviesListOutputDto> RemoveWatchedMovieAsync(int id, int movieId);
    }
}
