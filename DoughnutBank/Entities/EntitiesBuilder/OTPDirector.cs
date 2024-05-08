using DoughnutBank.Commons.ExpirationTimeStamp;

namespace DoughnutBank.Entities.EntitiesBuilder
{
    public class OTPDirector
    {
        public void Build1MinuteTimestamp<T>(GenericOTPBuilder<T> genericOTPBuilder) where T : OTP
        {
            genericOTPBuilder.TimeStampCreator(new MinutesTimeStampCreator()).TimeStampAvailability(1);
        }
    }
}
