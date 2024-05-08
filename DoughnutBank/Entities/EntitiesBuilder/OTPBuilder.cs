namespace DoughnutBank.Entities.EntitiesBuilder
{
    public class OTPBuilder : GenericOTPBuilder<OTP>
    {
        public override OTP Build()
        {
            return new OTP(_otpValue, _timeStampCreator, _timestampAvailability);
        }
    }
}
