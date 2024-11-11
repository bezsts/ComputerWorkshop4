using ApiDomain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ApiDomain.Services
{
    public static class MovieServiceCollectionExtensions
    {
        public static IServiceCollection AddMovie(this IServiceCollection services) =>
    services.AddSingleton<IMovieRepository, MovieRepository>();
    }
}
