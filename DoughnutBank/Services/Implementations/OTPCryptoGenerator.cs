using DoughnutBank.Exceptions;
using DoughnutBank.Services.Interfaces;
using System.Security.Cryptography;
namespace DoughnutBank.Services.Implementations
{
    public class OTPCryptoGenerator : IOTPGenerator
    {
        private static readonly ThreadLocal<RandomNumberGenerator> crng = new ThreadLocal<RandomNumberGenerator>(RandomNumberGenerator.Create);
        private static readonly ThreadLocal<byte[]> bytes = new ThreadLocal<byte[]>(() => new byte[sizeof(int)]);
        public string GenerateOTP()
        {
            try
            {
                return ConvertIntToOTPString(GetIntFromCryptoStrongByteSequence());
            }
            catch (Exception)
            {
                throw new CustomException("Could not generate OTP", 500); 
            }
        }

        private static string ConvertIntToOTPString(int intValue)
        {
            var OTPstringFormat = "000 000";
            return (intValue % 1000000).ToString(OTPstringFormat);
        }
        public static int GetIntFromCryptoStrongByteSequence()
        {
            crng.Value.GetBytes(bytes.Value);
            return BitConverter.ToInt32(bytes.Value, 0) & int.MaxValue;
        }
       
    }
}
