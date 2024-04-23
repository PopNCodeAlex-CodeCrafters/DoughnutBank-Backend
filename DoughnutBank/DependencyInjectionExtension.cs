using DoughnutBank.Authentication.ApiAccess;
using DoughnutBank.Authentication;
using DoughnutBank.Repositories.Implementations;
using DoughnutBank.Repositories.Interfaces;
using DoughnutBank.Services.Implementations;
using DoughnutBank.Services.Interfaces;

namespace DoughnutBank
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddOTPFeature(this IServiceCollection services) {
            services.AddScoped<OTPService>();
            services.AddScoped<IOTPGenerator, OTPCryptoGenerator>();
            services.AddScoped<OTPRepository>();
            return services;
        }

        public static IServiceCollection AddAuthenticationFeature(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services.AddTransient<ApiAccessMiddleware>();
            services.AddScoped<AuthorizationFilter>();
            return services;
        }
    }
}
