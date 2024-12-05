using ApiDomain.Entities;
using ApiDomain.Repositories.Contracts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text;
using WebApp.Dtos.Movie;
using WebApp.Options;
using WebApp.Services.Contracts;

namespace WebApp.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IOptionsSnapshot<CsvOptions> _csvOptions;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper, 
                            IMemoryCache memoryCache, IOptionsSnapshot<CsvOptions> csvExportOptions)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _csvOptions = csvExportOptions;
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
        
        //TODO: оптимізувати метод експорту фільмів
        //TODO: розглянути можливість кешування
        public async Task<byte[]> ExportMovies()
        { 
            var movies = await GetAllMoviesAsync();

            var csvData = new StringBuilder();
            var csvExportOptions = _csvOptions.Value;
            var fieldsToExport = csvExportOptions.FieldsToExport;
            char delimiter = csvExportOptions.Delimiter;

            movies = movies.Take(csvExportOptions.MaxExportRecords).ToList();
            
            csvData.AppendLine(String.Join(delimiter, fieldsToExport));

            foreach (var movie in movies)
            {
                var values = fieldsToExport.Select(field =>
                {
                    var property = typeof(MovieViewsOutputDto).GetProperty(field);

                    if (property?.Name == "ReleaseDate")
                    {
                        var releaseDateValue = property.GetValue(movie);
                        return releaseDateValue != null
                            ? ((DateOnly)releaseDateValue).ToString(csvExportOptions.DateFormat)
                            : string.Empty;
                    }
                    return property != null
                        ? property.GetValue(movie)?.ToString() : string.Empty;
                });

                csvData.AppendLine(String.Join(delimiter, values));
            }

            return Encoding.UTF8.GetBytes(csvData.ToString());
        }
    }
}
