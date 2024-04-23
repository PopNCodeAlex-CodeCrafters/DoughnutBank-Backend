using DoughnutBank.Entities.DBContext;
using DoughnutBank.Entities;
using DoughnutBank.Exceptions;

namespace DoughnutBank.Repositories.Implementations
{
    public class OTPRepository
    {
        private readonly DoughnutBankContext _context;

        public OTPRepository(DoughnutBankContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateOTPAsync(OTP otp)
        {
            var existingOtp = _context.OTPs.FirstOrDefault(o => o.UserEmail == otp.UserEmail);

            try
            {
                if (existingOtp == null)
                {
                    await _context.OTPs.AddAsync(otp);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    existingOtp.OTPValue = otp.OTPValue;
                    existingOtp.ExpirationTime = otp.ExpirationTime;
                    _context.Update(existingOtp);
                    await _context.SaveChangesAsync();
                    return true;

                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task CheckOTPAsync(OTP otp)
        {
            var existingOtp = _context.OTPs.FirstOrDefault(o => o.UserEmail == otp.UserEmail);
            if (existingOtp == null) throw new CustomException("No OTP was generated for current user");
            if (!StringEqualsIgnoringWhitespace(existingOtp.OTPValue, otp.OTPValue)) throw new CustomException("Invalid OTP");
            if (OTPExpired(existingOtp)) throw new CustomException("OTP Expired");

        }
       
        private bool StringEqualsIgnoringWhitespace(string str1, string str2)
        {
            string trimmedStr1 = RemoveWhitespace(str1);
            string trimmedStr2 = RemoveWhitespace(str2);

            return trimmedStr1.Equals(trimmedStr2);
        }

        private string RemoveWhitespace(string input)
        {
            return new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        private bool OTPExpired(OTP otp)
        {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return currentTime >= otp.ExpirationTime;
        }

    }
}
