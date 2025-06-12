using atf.Data.DatabaseContext;
using atf.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atf.Data.Repositories
{
    /// <summary>
    /// Repository for maintaining <see cref="User"/> entities in the database.
    /// Provides methods to create and retrieve users.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        public User Create(User user)
        {
            using var context = DbContextFactory.CreateContext();
            user.CreatedAt = DateTime.UtcNow;
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public User GetById(int id)
        {
            using var context = DbContextFactory.CreateContext();
            return context.Users.Find(id);
        }

        public User GetByEmail(string email)
        {
            using var context = DbContextFactory.CreateContext();
            return context.Users.FirstOrDefault(u => u.Email == email);
        }

        public List<User> GetAll()
        {
            using var context = DbContextFactory.CreateContext();
            return context.Users.ToList();
        }

        public User Update(User user)
        {
            using var context = DbContextFactory.CreateContext();
            context.Users.Update(user);
            context.SaveChanges();
            return user;
        }

        public void Delete(int id)
        {
            using var context = DbContextFactory.CreateContext();
            var user = context.Users.Find(id);
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }
    }
}
