using DoughnutBank.Commons.ExpirationTimeStamp;
using DoughnutBank.Entities;
using DoughnutBank.Entities.EntitiesBuilder;
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
                var otpBuilder = new OTPBuilder().OtpValue(otp);
                var otpDirector = new OTPDirector();
                otpDirector.Build1MinuteTimestamp(otpBuilder);

                var otpToSearchFor = otpBuilder.Build();
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
            {
                var otpBuilder = new EncryptedOTPBuilder().OtpValue(otp);
                var otpDirector = new OTPDirector();
                otpDirector.Build1MinuteTimestamp(otpBuilder);
                return otpBuilder.Build();
            }
          

            if (otp == null) throw new CustomException("Null input for encryption");

           
            EncryptedOTP encryptedOTP = EncryptOTPWithDiffieHellmanAndThrow(otp, diffieHellmanPublicKey);
            return encryptedOTP;
            
        }

        private async Task StoreOtpInRepositoryAsync(string otp)
        {
            try
            {
                var otpBuilder = new OTPBuilder().OtpValue(otp);
                var otpDirector = new OTPDirector();
                otpDirector.Build1MinuteTimestamp(otpBuilder);
                var otpObject = otpBuilder.Build();

                var isSuccessful = await _otpRepository.UpdateOTPAsync(otpObject);
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
            byte[] encryptedOTPBytes = diffieHellman.Encrypt(Encoding.UTF8.GetBytes(otherPartyPublicKey), otp);

            var publicKey = Convert.ToBase64String(diffieHellman.PublicKey);
            var iv = Convert.ToBase64String(diffieHellman.IV);
            string encryptedOTP = Convert.ToBase64String(encryptedOTPBytes);

            var otpBuilder = new EncryptedOTPBuilder().PublicKeyAndIV(publicKey, iv).OtpValue(encryptedOTP);
            var otpDirector = new OTPDirector();
            otpDirector.Build1MinuteTimestamp(otpBuilder);
            return otpBuilder.Build();
        }

    }
}
