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

        public async Task<User> LoginUser(User user)
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

        public async Task RegisterUser(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                //return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> UserExists(User user)
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
