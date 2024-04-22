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
        public async Task<ActionResult<EncryptedOTP>> GetOTPAsync([FromBody] EncryptedOTP partialOtp)
        {
            try
            {
                var encryptedOtp = await _otpService.ComputeEncryptedOTPAsync(partialOtp);
                return Ok(encryptedOtp);
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

        [HttpPost("/checkOTP")]
        public async Task<ActionResult> CheckOTPAsync([FromBody] User user)
        {
            try
            {
                await _otpService.CheckOTP(user);
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
    }
}
