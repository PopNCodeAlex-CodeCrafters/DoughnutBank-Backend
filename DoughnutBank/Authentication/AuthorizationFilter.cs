using DoughnutBank.Authentication.ApiAccess;
using DoughnutBank.Entities;
using DoughnutBank.Exceptions;
using DoughnutBank.Repositories.Interfaces;
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
            User userFromRequestHeader = GetUserFromContext(context.HttpContext);
            bool validCredentials = await _userRepository.UserExists(userFromRequestHeader);

            if (!validCredentials)
            {
                context.Result = new UnauthorizedObjectResult("Not authorized. Check credentials");
                return;
            }
        }

        private User GetUserFromContext(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(AuthConstants.AuthorizationRequestHeaderName, out var extractedAuthorization))
            {
                throw new CustomException("No authorization credentials provided", 400);
            }

            string authorization = extractedAuthorization.ToString();

            if (!authorization.StartsWith("Basic "))
                return null;

            string credentials = authorization.Substring("Basic ".Length);
            // string credentials = Encoding.UTF8.GetString(Convert.FromBase64String(base64Credentials));
            string[] parts = credentials.Split(':');

            if (parts.Length == 2)
            {
                string email = parts[0];
                string password = parts[1];

                Console.WriteLine("Email in middle: " + email);
                Console.WriteLine("Password  in middle: " + password);

                return new User
                {
                    Email = email,
                    Password = password
                };
            }
            else return null;
        }
    }
}
