using ApiDomain.Repositories;
using ApiDomain.Repositories.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ApiDomain.Services
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}
