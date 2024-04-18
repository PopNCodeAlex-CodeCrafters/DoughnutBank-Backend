using DoughnutBank.Entities;
using DoughnutBank.Exceptions;
using DoughnutBank.Repositories.Implementations;
using DoughnutBank.Services.Interfaces;
using DoughnutBank.Utils;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;

namespace DoughnutBank.Services.Implementations
{
    public class OTPService
    {
        private readonly IConfiguration _configuration;
        private readonly IOTPGenerator _otpGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly OTPRepository _otpRepository;
        public OTPService(IOTPGenerator otpGenerator, IConfiguration configuration, IHttpContextAccessor httpContextAccessor,
            OTPRepository otpRepository) { 
            _otpGenerator = otpGenerator;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _otpRepository = otpRepository;
        }
        public async Task<EncryptedOTP> ComputeEncryptedOTP(EncryptedOTP partialOtp)
        {
            string otp = null;
            try
            {
                otp = _otpGenerator.GenerateOTP();
            }
            catch (Exception)
            {
                throw new CustomException("OTP generation failed");
            }

            try
            {
                var userOfRequest = HttpContextUtils.GetUserFromContext(_httpContextAccessor.HttpContext);
                var isSuccessful = await _otpRepository.UpdateUserOTP(userOfRequest, new OTP
                {
                    UserEmail = userOfRequest.Email,
                    OTPValue = otp
                });
            }
            catch (Exception)
            {
                throw new CustomException("OTP update in database failed");
            }


           
            if (!_configuration.GetValue<bool>("Encryption"))
            {
                return new EncryptedOTP()
                {
                    OTPValue = otp,
                    Iv = "",
                    PublicKey = ""
                };
            }

            try
            {
                if (otp == null) throw new CustomException("Null input for encryption");
                var diffieHellman = new DiffieHellman();
                byte[] encryptedOTP = diffieHellman.Encrypt(Encoding.UTF8.GetBytes(partialOtp.PublicKey), otp);
                return new EncryptedOTP()
                {
                    PublicKey = Convert.ToBase64String(diffieHellman.PublicKey),
                    Iv = Convert.ToBase64String(diffieHellman.IV),
                    OTPValue = Convert.ToBase64String(encryptedOTP)
                };
            }
            catch(Exception)
            {
                throw new CustomException("OTP encryption failed");
            }
          

            
        }
        public async Task checkOTP(User user)
        {
            try
            {
                 await _otpRepository.checkOTP(user.OTP);
            }
            catch (Exception)
            {
                throw new CustomException("OTP invalid");
            }
        }

    }
}
