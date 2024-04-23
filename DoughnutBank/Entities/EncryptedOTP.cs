namespace DoughnutBank.Entities
{
    public class EncryptedOTP : OTP
    {
        public EncryptedOTP(string otp) : base(otp) { }
        public string? PublicKey { get; set; }
        public string? Iv {  get; set; }

        
    }
}
