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
            return (NextInt() % 1000000).ToString("000 000");
        }
        public static int NextInt()
        {
            crng.Value.GetBytes(bytes.Value);
            return BitConverter.ToInt32(bytes.Value, 0) & int.MaxValue;

        }
       
    }
}
