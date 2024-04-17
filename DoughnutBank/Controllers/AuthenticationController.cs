using DoughnutBank.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DoughnutBank.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IOTPGenerator _otpGenerator;
        public AuthenticationController(/*ILogger<WeatherForecastController> logger*/ IOTPGenerator otpGenerator)
        {
            //_logger = logger;
            _otpGenerator = otpGenerator;
        }

        [HttpGet("/login")]
        public string Get()
        {
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
            return _otpGenerator.GenerateOTP();
        }
    }
}
