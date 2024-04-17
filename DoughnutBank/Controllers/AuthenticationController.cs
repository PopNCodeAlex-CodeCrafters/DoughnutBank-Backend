using DoughnutBank.Exceptions;
using DoughnutBank.Services.Interfaces;
using DoughnutBank.Utils;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("/OTP")]
        public ActionResult<string> GetOTP()
        {
            try
            {
                return Ok(_otpGenerator.GenerateOTP());
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
