namespace DoughnutBank.Entities.EntitiesBuilder
{
    public class EncryptedOTPBuilder : GenericOTPBuilder<EncryptedOTP>
    {
        protected string? _publicKey;
        protected string? _iv;
        public override EncryptedOTP Build()
        {
            return new EncryptedOTP(_otpValue, _timeStampCreator, _timestampAvailability, _publicKey, _iv);
        }

        public EncryptedOTPBuilder PublicKeyAndIV(string publicKey, string iv)
        {
            _publicKey = publicKey;
            _iv = iv;
            return this;
        }
    }
}
