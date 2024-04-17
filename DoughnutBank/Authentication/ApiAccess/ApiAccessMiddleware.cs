
using DoughnutBank.Exceptions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace DoughnutBank.Authentication.ApiAccess
{
    public class ApiAccessMiddleware : IMiddleware
    {
        private readonly IConfiguration _configuration;

        public ApiAccessMiddleware(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string requestApiKey = GetApiKeyFromContext(context);
            string apiKey = GetApiKeyFromServerSettings();

            if(!apiKey.Equals(requestApiKey))
            {
                throw new CustomException("Invalid API key", 401);
            }

            await next(context);
        }

        private string GetApiKeyFromContext(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyRequestHeaderName, out var extractedApiKey))
            {
                throw new CustomException("No API key provided", 400);
            }

            return extractedApiKey;
        }

        private string GetApiKeyFromServerSettings()
        {
            try
            {
                var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);
                return apiKey;
            }
            catch (Exception)
            {
                throw new CustomException("API key not configured in settings");
            }
        }
    }
}
