using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Recipes.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    [HttpGet(Name = "GetWeatherForecast")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<WeatherForecast>))]
    [ProducesResponseType(401)]
    public IActionResult Get()
    {
        var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        return Ok(forecast);
    }
}