using DoughnutBank.Entities.DBContext;
using DoughnutBank.Entities;
using DoughnutBank.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DoughnutBank.Repositories.Implementations
{
    public class OTPRepository
    {
        private readonly DoughnutBankContext _context;

        public OTPRepository(DoughnutBankContext context)
        {
            _context = context;
        }

        public async Task checkOTP(OTP otp)
        {
            var existingOtp = _context.OTPs.FirstOrDefault(o => o.UserEmail == otp.UserEmail);
            if (existingOtp == null) throw new CustomException("No OTP was generated for current user");
            if (!StringEqualsIgnoringWhitespace(existingOtp.OTPValue, otp.OTPValue)) throw new CustomException("Invalid OTP");
            if (OTPExpired(existingOtp)) throw new CustomException("OTP Expired");

        }
        private bool OTPExpired(OTP otp)
        {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return currentTime >= otp.ExpirationTime;
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

        public async Task<bool> UpdateUserOTP(User user, OTP otp)
        {
            var existingOtp = _context.OTPs.FirstOrDefault(o => o.UserEmail == user.Email);

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
            catch (Exception) {
                return false;
            }
           
        }

        //public async Task<User> LoginUser(User user)
        //{
        //    try
        //    {
        //        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

        //        if (existingUser == null)
        //            throw new CustomException("Could not find account");

        //        return existingUser;

        //    }
        //    catch
        //    {
        //        throw new CustomException("Could not find account");
        //    }
        //}

        //public async Task RegisterUser(User user)
        //{
        //    try
        //    {
        //        await _context.Users.AddAsync(user);
        //        await _context.SaveChangesAsync();
        //        //return user;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw;
        //    }
        //}

        //public async Task<bool> CheckOtp(User user)
        //{
        //    try
        //    {
        //        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

        //        if (existingUser == null)
        //            return false;

        //        return true;

        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}
