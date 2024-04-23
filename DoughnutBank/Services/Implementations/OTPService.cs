using DoughnutBank.Entities;
using DoughnutBank.Exceptions;
using DoughnutBank.Repositories.Implementations;
using DoughnutBank.Services.Interfaces;
using DoughnutBank.Utils;
using System.Text;

namespace DoughnutBank.Services.Implementations
{
    public class OTPService
    {
        private readonly IConfiguration _configuration;
        private readonly IOTPGenerator _otpGenerator;
        private readonly OTPRepository _otpRepository;
        public OTPService(IOTPGenerator otpGenerator, IConfiguration configuration,
            OTPRepository otpRepository) { 
            _otpGenerator = otpGenerator;
            _configuration = configuration;
            _otpRepository = otpRepository;
        }

        public async Task CheckOTPAsync(string otp)
        {
            try
            {
                var otpToSearchFor = new OTP(otp);
                await _otpRepository.CheckOTPAsync(otpToSearchFor);
            }
            catch (Exception)
            {
                throw new CustomException("OTP invalid");
            }
        }
        public async Task<EncryptedOTP> ComputeEncryptedOTPAsync(string diffieHellmanPublicKey)
        {
            string otp = _otpGenerator.GenerateOTP();

            StoreOtpInRepositoryAsync(otp);

            if (!ShouldEncryptOTP())
                return new EncryptedOTP(otp);
          

            if (otp == null) throw new CustomException("Null input for encryption");

           
            EncryptedOTP encryptedOTP = EncryptOTPWithDiffieHellmanAndThrow(otp, diffieHellmanPublicKey);
            return encryptedOTP;
            
        }

        private async Task StoreOtpInRepositoryAsync(string otp)
        {
            try
            {
                var isSuccessful = await _otpRepository.UpdateOTPAsync(new OTP(otp));
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
            return new EncryptedOTP(Convert.ToBase64String(encryptedOTP))
            {
                PublicKey = Convert.ToBase64String(diffieHellman.PublicKey),
                Iv = Convert.ToBase64String(diffieHellman.IV)
            };
        }

    }
}
