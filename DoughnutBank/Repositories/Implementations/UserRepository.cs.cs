using DoughnutBank.Entities;
using DoughnutBank.Entities.DBContext;
using DoughnutBank.Exceptions;
using DoughnutBank.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoughnutBank.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly DoughnutBankContext _context;

        public UserRepository(DoughnutBankContext context)
        {
            _context = context;
        }

        public async Task<User> LoginUserAsync(User user)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

                if (existingUser == null)
                throw new CustomException("Could not find account");

                return existingUser;

            }
            catch {
                throw new CustomException("Could not find account");
            }
        }

        public async Task RegisterUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> UserExistsAsync(User user)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

                if (existingUser == null)
                   return false;

                return true;

            }
            catch
            {
                return false;
            }
        }
    }
}
