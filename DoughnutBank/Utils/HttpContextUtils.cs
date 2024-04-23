using DoughnutBank.Authentication.ApiAccess;
using DoughnutBank.Entities;
using DoughnutBank.Exceptions;

namespace DoughnutBank.Utils
{
    public class HttpContextUtils
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static User GetUserFromContext()
        {
            var context = _httpContextAccessor.HttpContext;
            if (!context.Request.Headers.TryGetValue(AuthConstants.AuthorizationRequestHeaderName, out var extractedAuthorization))
            {
                throw new CustomException("No authorization credentials provided", 400);
            }

            string authorization = extractedAuthorization.ToString();

            if (!authorization.StartsWith("Basic "))
                return null;

            string credentials = authorization.Substring("Basic ".Length);
            string[] parts = credentials.Split(':');

            if (parts.Length == 2)
            {
                string email = parts[0];
                string password = parts[1];

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
