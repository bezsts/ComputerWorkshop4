using WebApp.Services.Contracts;

namespace WebApp.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddScoped<IMovieService, MovieService>()
            .AddScoped<IUserService, UserService>();
    }
}
