using DoughnutBank.Entities;
using DoughnutBank.Exceptions;
using DoughnutBank.Services.Interfaces;
using DoughnutBank.Utils;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DoughnutBank.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IOTPGenerator _otpGenerator;
        public AuthenticationController(IOTPGenerator otpGenerator)
        {
            _otpGenerator = otpGenerator;
        }

        [HttpPost("/login")]
        public ActionResult<string> LoginUser([FromBody] User user)
        {
            try
            {
                Console.WriteLine("The email: " + user.Email + ", the password: " + user.Password);
                    return Ok();
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("/OTP")]
        public ActionResult<string> GetOTP()
        {
            try
            {
                return Ok(new { otp = _otpGenerator.GenerateOTP() });
            }
            catch(CustomException ex)
            {
                return StatusCode(ex.ErrorCode, ex.Message);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
