using ApiDomain.Repositories;
using ApiDomain.Repositories.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDomain.Services
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>))
                    .AddTransient<IMovieRepository, MovieRepository>()
                    .AddTransient<IUserRepository, UserRepository>();
    }
}
