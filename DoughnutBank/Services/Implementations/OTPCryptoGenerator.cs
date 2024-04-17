using DoughnutBank.Exceptions;
using DoughnutBank.Services.Interfaces;
using DoughnutBank.Utils;
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
                throw new Exception();
                return (NextInt() % 1000000).ToString("000 000");
            }
            catch (Exception)
            {
                throw new InvalidCastException();
             //   throw new CustomException("Could not generate OTP", 234); 
            }

           
        }
        public static int NextInt()
        {
            crng.Value.GetBytes(bytes.Value);
            return BitConverter.ToInt32(bytes.Value, 0) & int.MaxValue;
        }
       
    }
}
