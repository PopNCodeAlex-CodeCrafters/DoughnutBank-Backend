using DoughnutBank.Commons.ExpirationTimeStamp;
using DoughnutBank.Utils;

namespace DoughnutBank.Entities.EntitiesBuilder
{
    public abstract class GenericOTPBuilder<T> where T : OTP
    {
        protected string _otpValue;
        protected int _timestampAvailability = 1;
        protected TimeStampCreator _timeStampCreator = new MinutesTimeStampCreator();

        public GenericOTPBuilder<T> OtpValue(string otpValue) {
            _otpValue = otpValue;
            return this;
        }

        public GenericOTPBuilder<T> TimeStampCreator(TimeStampCreator timeStampCreator)
        {
            _timeStampCreator = timeStampCreator;
            return this;
        }

        public GenericOTPBuilder<T> TimeStampAvailability(int timestampAvailability)
        {
            _timestampAvailability = timestampAvailability;
            return this;
        }

        public abstract T Build();


    }
}
