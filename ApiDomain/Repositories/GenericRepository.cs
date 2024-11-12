using ApiDomain.Repositories.Contracts;

namespace ApiDomain.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class
    {
        protected DataModelContext _context;

        public GenericRepository(DataModelContext context) => _context = context;
        public virtual Task AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);

            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);

            return _context.SaveChangesAsync();
        }

        public Task<TEntity?> FindAsync(TKey key)
        {
            return _context.Set<TEntity>().FindAsync(key).AsTask();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);

            return _context.SaveChangesAsync();
        }
    }
}
