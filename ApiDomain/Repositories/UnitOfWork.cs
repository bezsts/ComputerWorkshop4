using ApiDomain.Repositories.Contracts;

namespace ApiDomain.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataModelContext _context;

        public UnitOfWork(DataModelContext context)
        {
            _context = context;
            Movies = new MovieRepository(_context);
            Users = new UserRepository(_context);
        }
        public IMovieRepository Movies { get; set; }

        public IUserRepository Users { get; set; }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
