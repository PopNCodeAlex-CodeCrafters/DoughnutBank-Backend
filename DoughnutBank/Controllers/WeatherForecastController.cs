using Microsoft.AspNetCore.Mvc;

namespace DoughnutBank.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        
        public AuthenticationController(/*ILogger<WeatherForecastController> logger*/)
        {
            //_logger = logger;
        }

        [HttpGet("/login")]
        public void Get()
        {
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
            Console.WriteLine();
        }
    }
}
