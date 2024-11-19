using ApiDomain.Entities;

namespace ApiDomain.Repositories.Contracts
{
    public interface IMovieRepository : IGenericRepository<Movie, int>
    {
        Task<Movie?> FindMovieByTitleAsync(string title);
    }
}
