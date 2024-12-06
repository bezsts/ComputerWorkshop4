using WebApp.Common.Exporters;
using WebApp.Common.Exporters.Contracts;
using WebApp.Services;
using WebApp.Services.Contracts;

namespace WebApp
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddScoped<IMovieService, MovieService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<ICsvExporter, CsvExporter>();
    }
}
