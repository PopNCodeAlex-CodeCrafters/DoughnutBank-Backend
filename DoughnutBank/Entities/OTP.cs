namespace DoughnutBank.Entities
{
    public class OTP
    {
        public string UserEmail { get; set; }
        public string OTPValue { get; set; }
        public long ExpirationTime { get; set; }
    }
}
