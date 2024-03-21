using UnitOfWorkTrial.GenericRepository;
using UnitOfWorkTrial.Models;

namespace UnitOfWorkTrial.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> VerifyUser(string email, string password);
    }
}
