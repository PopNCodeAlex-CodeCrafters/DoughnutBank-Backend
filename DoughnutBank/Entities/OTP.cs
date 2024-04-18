namespace DoughnutBank.Entities
{
    public class OTP
    {
        public OTP()
        {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var expirationTimeInMinutes = 1;
            ExpirationTime = currentTime + (expirationTimeInMinutes * 60 * 1000);
        }
        public string UserEmail { get; set; }
        public string OTPValue { get; set; }
        public long ExpirationTime { get; set; }
    }
}
