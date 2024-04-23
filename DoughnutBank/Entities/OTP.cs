using DoughnutBank.Utils;

namespace DoughnutBank.Entities
{
    public class OTP
    {
        public OTP()
        {
            ExpirationTime = OneMinuteExpirationTimesStamp();
            UserEmail = GetUserEmailFromHTTPContext();
        }

        public OTP(string otpValue) : this()
        {
            OTPValue = otpValue;
        }
        public string UserEmail { get; set; }
        public string OTPValue { get; set; }
        public long ExpirationTime { get; set; }

        private long OneMinuteExpirationTimesStamp()
        {
            return ExpirationTimestamp(1);
        }
        private long ExpirationTimestamp(int minutesUntilExpiration)
        {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return currentTime + (minutesUntilExpiration * 60 * 1000);
        }

        private string GetUserEmailFromHTTPContext()
        {
            try
            {
                var user = HttpContextUtils.GetUserFromContext();
                return user.Email;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
