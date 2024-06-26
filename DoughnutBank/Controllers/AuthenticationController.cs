using DoughnutBank.Entities;
using DoughnutBank.Exceptions;
using DoughnutBank.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DoughnutBank.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("/login")]
        public async Task<ActionResult<User>> LoginUser([FromBody] User user)
        {
            try
            {
                var userFromDatabase = await _authenticationService.LoginUser(user);

                return Ok(userFromDatabase);
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

        [HttpPost("/signup")]
        public async Task<ActionResult<string>> RegisterUser([FromBody] User user)
        {
            try
            {
                var jwt = await _authenticationService.RegisterUser(user);
                return Ok(jwt);
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
