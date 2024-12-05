using ApiDomain.Entities;
using ApiDomain.Exceptions;
using ApiDomain.Repositories.Contracts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp.Dtos.Movie;
using WebApp.Dtos.User;
using WebApp.Services.Contracts;

namespace WebApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserWatchedMoviesCountOutputDto>> GetAllUsersAsync()
        {
            var users = _unitOfWork.Users.GetAll();

            return await _mapper.ProjectTo<UserWatchedMoviesCountOutputDto>(users).ToListAsync();
        }

        public async Task<UserWatchedMoviesCountOutputDto> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.FindAsync(id);
            return _mapper.Map<UserWatchedMoviesCountOutputDto>(user);
        }

        public async Task<UserWatchedMoviesCountOutputDto> GetUserByNameAsync(string name)
        {
            var user = await _unitOfWork.Users.FindByNameAsync(name);
            return _mapper.Map<UserWatchedMoviesCountOutputDto>(user);
        }

        public async Task<UserOutputDto> CreateUserAsync(UserCreateDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.Users.AddAsync(user);
            return _mapper.Map<UserOutputDto>(user);
        }

        public async Task<UserOutputDto?> UpdateUserAsync(int id, UserCreateDto updatedUser)
        {
            var existingUser = await _unitOfWork.Users.FindAsync(id);

            if (existingUser == null)
            {
                return null;
            }

            _mapper.Map(updatedUser, existingUser);
            await _unitOfWork.Users.UpdateAsync(existingUser);
            return _mapper.Map<UserOutputDto>(existingUser);
        }

        public async Task<UserOutputDto?> DeleteUserAsync(int id)
        {
            var existingUser = await _unitOfWork.Users.FindAsync(id);

            if (existingUser == null)
            {
                return null;
            }

            await _unitOfWork.Users.DeleteAsync(existingUser);

            return _mapper.Map<UserOutputDto>(existingUser);
        }

        public async Task<UserWatchedMoviesListOutputDto?> GetWatchedMoviesOfUserAsync(int id)
        {
            var user = await _unitOfWork.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserWatchedMoviesListOutputDto>(user);
        }

        public async Task<UserWatchedMoviesListOutputDto> AddWatchedMovieAsync(int id, int movieId)
        {
            var user = await _unitOfWork.Users.FindAsync(id);
            var movie = await _unitOfWork.Movies.FindAsync(movieId);

            if (user is null)
            {
                throw new UserNotFoundException();
            }

            if (movie is null)
            {
                throw new MovieNotFoundException();
            }

            if (user.WatchedMovies.Contains(movie))
            {
                throw new MovieConflictException();
            }

            user.WatchedMovies.Add(movie);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserWatchedMoviesListOutputDto>(user);
        }
        
        public async Task<UserWatchedMoviesListOutputDto> RemoveWatchedMovieAsync(int id, int movieId)
        {
            var user = await _unitOfWork.Users.FindAsync(id);
            var movie = await _unitOfWork.Movies.FindAsync(movieId);

            if (user is null)
            {
                throw new UserNotFoundException();
            }

            if (movie is null)
            {
                throw new MovieNotFoundException("Movie with ID {MovieId} is not found in {MethodName}");
            }

            if (!user.WatchedMovies.Contains(movie))
            {
                throw new MovieNotFoundException("Movie with ID {MovieId} is not found in watched movies of user with ID {UserId} in {MethodName}");
            }

            user.WatchedMovies.Remove(movie);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserWatchedMoviesListOutputDto>(user);
        }
    }
}
