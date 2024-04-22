namespace DoughnutBank.Entities
{
    public class EncryptedOTP
    {
        public EncryptedOTP() {
            ExpirationTime = OneMinuteExpirationTimesStamp();
        }

        public EncryptedOTP(string otp) : base()
        {
            OTPValue = otp;
        }

        public string? OTPValue { get; set; }    
        public long? ExpirationTime { get; set; }
        public string? PublicKey { get; set; }
        public string? Iv {  get; set; }

        private long OneMinuteExpirationTimesStamp()
        {
            return ExpirationTimestamp(1);
        }
        private long ExpirationTimestamp(int minutesUntilExpiration)
        {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return currentTime + (minutesUntilExpiration * 60 * 1000);
        }
    }
}
