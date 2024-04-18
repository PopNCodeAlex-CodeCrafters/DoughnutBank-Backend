namespace DoughnutBank.Entities
{
    public class EncryptedOTP
    {
        public EncryptedOTP() {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var expirationTimeInMinutes = 1;
            ExpirationTime = currentTime + (expirationTimeInMinutes * 60 * 1000);
        }
        public string? OTPValue { get; set; }    
        public long? ExpirationTime { get; set; }
        public string? PublicKey { get; set; }
        public string? Iv {  get; set; }
    }
}
