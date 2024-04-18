using DoughnutBank.Entities;
using DoughnutBank.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DoughnutBank.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<User> LoginUser(User user);

        public Task<string> RegisterUser(User user);

    }
}
