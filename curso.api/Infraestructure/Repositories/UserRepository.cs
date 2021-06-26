using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Infraestructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CourseDbContext _context;

        public UserRepository(CourseDbContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.User.Add(user);
            
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public User GetUser(string login)
        {
            return _context.User.FirstOrDefault(u => u.Login == login);
        }
    }
}
