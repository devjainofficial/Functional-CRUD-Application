using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkTrial.GenericRepository;
using UnitOfWorkTrial.Models;
using UnitOfWorkTrial.Repository;

namespace UnitOfWorkTrial.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User?> VerifyUser(string email, string password)
        {
            var User = _context.Users.Where(x => x.Email == email && x.Password == password).FirstOrDefault();
            return User;
        }
    }
}
