using DoughnutBank.Entities;
using DoughnutBank.Exceptions;
using DoughnutBank.Services.Interfaces;
using DoughnutBank.Utils;
using System.Security.Cryptography;
using System.Text;

namespace DoughnutBank.Services.Implementations
{
    public class OTPService
    {
        private readonly IConfiguration _configuration;
        private readonly IOTPGenerator _otpGenerator;
        public OTPService(IOTPGenerator otpGenerator, IConfiguration configuration) { 
            _otpGenerator = otpGenerator;
            _configuration = configuration;
        }
        public EncryptedOTP ComputeEncryptedOTP(EncryptedOTP partialOtp)
        {
            string otp = null;
            try
            {
                otp = _otpGenerator.GenerateOTP();
                if(!_configuration.GetValue<bool>("Encryption"))
                {
                    return new EncryptedOTP()
                    {
                        OTPValue = otp
                    };
                }
            }
            catch (Exception)
            {
                throw new CustomException("OTP generation failed");
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
    }
}
