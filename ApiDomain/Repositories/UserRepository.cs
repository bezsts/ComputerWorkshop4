using ApiDomain.Entities;
using ApiDomain.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ApiDomain.Repositories
{
    internal class UserRepository : GenericRepository<User, int>, IUserRepository
    {
        public UserRepository(DataModelContext context) : base(context) { }

        public override Task<User?> FindAsync(int key) =>
            _context.Set<User>()
            .Include(u => u.WatchedMovies)
                .ThenInclude(wm => wm.UsersWhoWatched)
            .FirstOrDefaultAsync(u => u.Id == key);

        public Task<User?> FindByNameAsync(string name) =>
            _context.Set<User>()
            .Include(u => u.WatchedMovies)
            .FirstOrDefaultAsync(u => u.Name.ToLower().Contains(name.ToLower()));
    }
}
