using ApiDomain.Entities;

namespace ApiDomain.Repositories.Contracts
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
        Task<User?> FindByNameAsync(string name);
    }
}
