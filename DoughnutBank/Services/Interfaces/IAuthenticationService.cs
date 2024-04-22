using DoughnutBank.Entities;
using DoughnutBank.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DoughnutBank.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<User> LoginUserAsync(User user);

        public Task<string> RegisterUserAsync(User user);

    }
}
