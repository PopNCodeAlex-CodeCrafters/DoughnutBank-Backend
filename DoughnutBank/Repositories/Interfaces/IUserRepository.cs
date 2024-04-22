using DoughnutBank.Entities;
using DoughnutBank.Repositories.Implementations;

namespace DoughnutBank.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task RegisterUserAsync(User user);
        public Task<User> LoginUserAsync(User user);
        public Task<bool> UserExistsAsync(User user);
    }
}
