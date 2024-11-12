using ApiDomain.Entities;
using ApiDomain.Repositories.Contracts;

namespace ApiDomain.Repositories
{
    internal class UserRepository : GenericRepository<User, int>, IUserRepository
    {
        public UserRepository(DataModelContext context) : base(context) { }
    }
}
