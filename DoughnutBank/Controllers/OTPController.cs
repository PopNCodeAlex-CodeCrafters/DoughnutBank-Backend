using DoughnutBank.Authentication;
using DoughnutBank.Entities;
using DoughnutBank.Exceptions;
using DoughnutBank.Services.Implementations;
using DoughnutBank.Services.Interfaces;
using DoughnutBank.Utils;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;


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

        //The client was sending and EncryptedOTP object just to send its public key for DiffieHellman encryption

        [HttpPost("/OTP")]
        public async Task<ActionResult<EncryptedOTP>> GetOTPAsync([FromBody] JsonElement publicKey)
        {
            try
            {
                string diffieHellmanPublicKey = publicKey.GetProperty("diffieHellmanPublicKey").ToString();
                var encryptedOtp = await _otpService.ComputeEncryptedOTPAsync(diffieHellmanPublicKey);
                return Ok(encryptedOtp);
            }
            catch(KeyNotFoundException ex)
            {
                return StatusCode(500, "Pass public encryption key in json object with key 'diffieHellmanPublicKey'");

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

        [HttpPost("/checkOTP")]
        public async Task<ActionResult> CheckOTPAsync([FromBody] JsonElement otp)
        {
            try
            {
                string otpCode = otp.GetProperty("otpValue").GetString();
                await _otpService.CheckOTPAsync(otpCode);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(500, "Pass OTP in json object with key 'otpValue'");

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
