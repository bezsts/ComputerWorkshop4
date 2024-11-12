using ApiDomain.Entities;
using ApiDomain.Repositories.Contracts;

namespace ApiDomain.Repositories
{
    internal class MovieRepository : GenericRepository<Movie, int>, IMovieRepository
    {
        public MovieRepository(DataModelContext context) : base(context) { }
    }
}
