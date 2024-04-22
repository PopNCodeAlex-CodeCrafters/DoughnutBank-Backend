using DoughnutBank.Entities;
using DoughnutBank.Exceptions;
using DoughnutBank.Repositories.Implementations;
using DoughnutBank.Services.Interfaces;
using DoughnutBank.Utils;
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

        public async Task CheckOTP(User user)
        {
            try
            {
                await _otpRepository.CheckOTPAsync(user.OTP);
            }
            catch (Exception)
            {
                throw new CustomException("OTP invalid");
            }
        }
        public async Task<EncryptedOTP> ComputeEncryptedOTPAsync(EncryptedOTP partialOtp)
        {
            string otp = null;
            otp = _otpGenerator.GenerateOTP();

            StoreOtpInRepositoryAsync(otp);

            if (!ShouldEncryptOTP())
                return new EncryptedOTP(otp);
          

            if (otp == null) throw new CustomException("Null input for encryption");

           
            EncryptedOTP encryptedOTP = EncryptOTPWithDiffieHellmanAndThrow(otp, partialOtp.PublicKey);
            return encryptedOTP;
            
        }

        private async Task StoreOtpInRepositoryAsync(string otp)
        {
            try
            {
                var userOfRequest = HttpContextUtils.GetUserFromContext(_httpContextAccessor.HttpContext);
                var isSuccessful = await _otpRepository.UpdateUserOTPAsync(userOfRequest, new OTP
                {
                    UserEmail = userOfRequest.Email,
                    OTPValue = otp
                });
            }
            catch (Exception ex)
            {
                throw new CustomException("OTP update in database failed", ex);
            }
        }

        private bool ShouldEncryptOTP()
        {
            return _configuration.GetValue<bool>("Encryption");
        }

        private EncryptedOTP EncryptOTPWithDiffieHellmanAndThrow(string otp, string otherPartyPublicKey)
        {
            try
            {
                EncryptedOTP encryptedOTP = EncryptOTPWithDiffieHellman(otp, otherPartyPublicKey);
                return encryptedOTP;
            }
            catch (Exception ex)
            {
                throw new CustomException("OTP encryption failed", ex);
            }
        }

        private EncryptedOTP EncryptOTPWithDiffieHellman(string otp, string otherPartyPublicKey)
        {
            var diffieHellman = new DiffieHellman();
            byte[] encryptedOTP = diffieHellman.Encrypt(Encoding.UTF8.GetBytes(otherPartyPublicKey), otp);
            return new EncryptedOTP()
            {
                PublicKey = Convert.ToBase64String(diffieHellman.PublicKey),
                Iv = Convert.ToBase64String(diffieHellman.IV),
                OTPValue = Convert.ToBase64String(encryptedOTP)
            };
        }

    }
}
