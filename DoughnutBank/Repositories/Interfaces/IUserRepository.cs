using DoughnutBank.Entities;
using DoughnutBank.Repositories.Implementations;

namespace DoughnutBank.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task RegisterUser(User user);
        public Task<User> LoginUser(User user);
        public Task<bool> UserExists(User user);
    }
}
