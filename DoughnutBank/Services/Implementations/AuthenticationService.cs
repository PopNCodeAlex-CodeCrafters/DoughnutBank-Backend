using DoughnutBank.Entities;
using DoughnutBank.Repositories.Interfaces;
using DoughnutBank.Services.Interfaces;
using DoughnutBank.Utils;

namespace DoughnutBank.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        public AuthenticationService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }
        public async Task<User> LoginUser(User user)
        {
            HashUserSensitiveData(user);
            var userFromDatabase = await _userRepository.LoginUser(user);
            return userFromDatabase;
        }

        public async Task<string> RegisterUser(User user)
        {
            var JWThardcoded = "HARDCODED JWToken";
            HashUserSensitiveData(user);
            await _userRepository.RegisterUser(user);
            return JWThardcoded;
        }

        private void HashUserSensitiveData(User user)
        {
            if (user == null) return;
            if (user.Password == null) return;
            user.Password = HashUtil.ComputeSha256Hash(user.Password);
            if (user.OTP == null || user.OTP.OTPValue == null) return;
            user.OTP.OTPValue = HashUtil.ComputeSha256Hash(user.OTP.OTPValue);
        }
    }
}
