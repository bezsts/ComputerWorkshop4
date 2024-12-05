﻿using ApiDomain.Entities;
using ApiDomain.Repositories.Contracts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApp.Dtos.Movie;
using WebApp.Services.Contracts;

namespace WebApp.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<List<MovieViewsOutputDto>> GetAllMoviesAsync()
        {
            var movies = _unitOfWork.Movies.GetAll();
            return await _mapper.ProjectTo<MovieViewsOutputDto>(movies).ToListAsync();

        }

        public async Task<MovieViewsOutputDto> GetMovieByIdAsync(int id)
        {
            var movie = await _unitOfWork.Movies.FindAsync(id);

            return _mapper.Map<MovieViewsOutputDto>(movie);
        }

        public async Task<MovieViewsOutputDto> GetMovieByTitleAsync(string title)
        {
            var movie = await _unitOfWork.Movies.FindMovieByTitleAsync(title);

            return _mapper.Map<MovieViewsOutputDto>(movie);
        }

        public async Task<MovieOutputDto> CreateMovieAsync(MovieCreateDto movie)
        {
            var movieEntity = _mapper.Map<Movie>(movie);
            await _unitOfWork.Movies.AddAsync(movieEntity);
            return _mapper.Map<MovieOutputDto>(movieEntity);
        }

        public async Task<MovieOutputDto?> UpdateMovieAsync(int id, MovieCreateDto updatedMovie)
        {
            var existingMovie = await _unitOfWork.Movies.FindAsync(id);
            if (existingMovie == null)
            {
                return null;
            }
            _mapper.Map(updatedMovie, existingMovie);
            await _unitOfWork.Movies.UpdateAsync(existingMovie);
            return _mapper.Map<MovieOutputDto>(existingMovie);
        }

        public async Task<MovieViewsOutputDto?> DeleteMovieAsync(int id)
        {
            var existingMovie = await _unitOfWork.Movies.FindAsync(id);
            if (existingMovie == null)
            {
                return null;
            }
            await _unitOfWork.Movies.DeleteAsync(existingMovie);
            return _mapper.Map<MovieViewsOutputDto>(existingMovie);
        }
        public async Task<List<MovieViewsOutputDto>?> GetPopularMoviesAsync()
        {
            var popularMovies = await _memoryCache.GetOrCreateAsync(nameof(GetPopularMoviesAsync), async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);


                var popularMoviesQuery = _unitOfWork.Movies.GetMostPopularMovies();
                return await _mapper.ProjectTo<MovieViewsOutputDto>(popularMoviesQuery).ToListAsync();
            });

            return popularMovies;
        }
    }
}
