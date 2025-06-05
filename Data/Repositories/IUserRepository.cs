using atf.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atf.Data.Repositories
{
    public interface IUserRepository
    {
        User Create(User user);
        User GetById(int id);
        User GetByEmail(string email);
        List<User> GetAll();
        User Update(User user);
        void Delete(int id);
    }
}
