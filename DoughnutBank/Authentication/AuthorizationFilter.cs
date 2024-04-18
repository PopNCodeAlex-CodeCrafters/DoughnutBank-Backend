using DoughnutBank.Authentication.ApiAccess;
using DoughnutBank.Entities;
using DoughnutBank.Exceptions;
using DoughnutBank.Repositories.Interfaces;
using DoughnutBank.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DoughnutBank.Authentication
{
    public class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthorizationFilter(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            User userFromRequestHeader = HttpContextUtils.GetUserFromContext(context.HttpContext);
            bool validCredentials = await _userRepository.UserExists(userFromRequestHeader);

            if (!validCredentials)
            {
                context.Result = new UnauthorizedObjectResult("Not authorized. Check credentials");
                return;
            }
        }
    }
}
