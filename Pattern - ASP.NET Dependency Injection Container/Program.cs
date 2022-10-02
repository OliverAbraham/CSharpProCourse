using Microsoft.AspNetCore.Mvc;

namespace Pattern___ASP.NET_Dependency_Injection_Container
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // we tell the DI container if the ASP.NET library that there is a service calles IWeatherSource
            // This service can be used in every Controller.
            // The only thing we have to do is naming that parameter in the constructor of the controller
            builder.Services.AddSingleton<IWeatherSource>(new WeatherSource());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }

    public interface IWeatherSource
    {
        string GetWeather();
    }

    public class WeatherSource : IWeatherSource
    {
        public string GetWeather() { return "The weather is fine!"; }
    }
}

namespace Pattern___ASP.NET_Dependency_Injection_Container.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IWeatherSource _weatherService;

        public WeatherForecastController(IWeatherSource weatherService) // <--------- the DI container locates a suitable instance for us
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new OkObjectResult("Hello I am your controller! " + _weatherService.GetWeather());
        }
    }
}