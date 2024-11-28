using ApiDomain.Entities;
using ApiDomain.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ApiDomain.Repositories
{
    internal class MovieRepository : GenericRepository<Movie, int>, IMovieRepository
    {
        public MovieRepository(DataModelContext context) : base(context) { }

        public override Task<Movie?> FindAsync(int key) =>
            _context.Set<Movie>()
            .Include(m => m.UsersWhoWatched)
            .FirstOrDefaultAsync(m => m.Id == key);

        public Task<Movie?> FindMovieByTitleAsync(string title) =>
            _context.Set<Movie>()
            .Include(m => m.UsersWhoWatched)
            .FirstOrDefaultAsync(m => m.Title.ToLower().Contains(title));
    }
}
