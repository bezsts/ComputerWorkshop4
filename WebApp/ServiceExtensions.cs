using Microsoft.Extensions.Options;
using WebApp.Common.Exporters;
using WebApp.Common.Exporters.Contracts;
using WebApp.Options.Validators;
using WebApp.Options;
using WebApp.Services;
using WebApp.Services.Contracts;

namespace WebApp
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddScoped<IMovieService, MovieService>()
            .AddScoped<IUserService, UserService>();

        public static IServiceCollection AddOptionsValidators(this IServiceCollection services) => services
            .AddSingleton<IValidateOptions<ExportMoviesOptions>, ExportMovieOptionsValidator>()
            .AddSingleton<IValidateOptions<CsvOptions>, CsvOptionsValidator>();

        public static IServiceCollection AddExporters(this IServiceCollection services) => services
            .AddScoped<ICsvExporter, CsvExporter>();
    }
}
