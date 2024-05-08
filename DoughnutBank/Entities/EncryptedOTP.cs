using DoughnutBank.Commons.ExpirationTimeStamp;

namespace DoughnutBank.Entities
{
    public class EncryptedOTP : OTP
    {
        public EncryptedOTP(string otp, TimeStampCreator timeStampCreator, int timestampAvailability, string publicKey, string iv) 
            : base(otp, timeStampCreator, timestampAvailability) {
            _publicKey = publicKey;
            _iv = iv;
        }
        private string _publicKey;
        private string _iv;

    }
}
