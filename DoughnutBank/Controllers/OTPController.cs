using DoughnutBank.Authentication;
using DoughnutBank.Entities;
using DoughnutBank.Exceptions;
using DoughnutBank.Services.Implementations;
using DoughnutBank.Services.Interfaces;
using DoughnutBank.Utils;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace DoughnutBank.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(AuthorizationFilter))]

    public class OTPController : ControllerBase
    {
        private readonly OTPService _otpService;
        public OTPController(OTPService otpService)
        {
            _otpService = otpService;
        }


        [HttpPost("/OTP")]
        public ActionResult<EncryptedOTP> GetOTP([FromBody] EncryptedOTP partialOtp)
        {
            try
            {  
                return Ok(_otpService.ComputeEncryptedOTP(partialOtp));
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
