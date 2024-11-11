using ApiDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDomain.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User? Get(int Id);
        User Create(User user);
        bool Update(int id, User updatedUser);
        bool Delete(int Id);
    }
}
