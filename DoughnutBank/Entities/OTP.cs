using DoughnutBank.Commons.ExpirationTimeStamp;
using DoughnutBank.Utils;

namespace DoughnutBank.Entities
{
    public partial class OTP
    {
        public OTP(string otpValue, TimeStampCreator timeStampCreator, int timestampAvailabilty)
        {
            _timeStampCreator = timeStampCreator;
            ExpirationTime = _timeStampCreator.createTimeStamp(timestampAvailabilty);
            UserEmail = GetUserEmailFromHTTPContext();
            OTPValue = otpValue;
        }
        public string UserEmail { get; set; }
        public string OTPValue { get; set; }
        public long ExpirationTime { get; set; }

        /// factory method pattern
        private TimeStampCreator _timeStampCreator;

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
