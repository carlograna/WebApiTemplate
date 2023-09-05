using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore;

namespace WebApiTemplate.Controllers;

[ApiController]
//[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet("forecast", Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var val = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();



        return val;
    }

    //[HttpGet("temperature", Name = "GetTemperature")]
    //public async Task<IEnumerable<WeatherForecast>> GetTemperature()
    //{
    //    var @value = Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //    {
    //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //        TemperatureC = Random.Shared.Next(-20, 55),
    //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //    })
    //    .ToArray();

    //    return await Task.FromResult(@value);
    //}

    [HttpGet("convertToCelsius/{degrees}", Name = "ConvertToCelsius")]
    public async Task<int> ConvertToCelsius(int degrees)
    {
        var @value = (degrees - 32) * 5 / 9;

        _logger.LogInformation("convertion successful");
        return await Task.FromResult(@value);
    }

    [HttpGet]
    [Route("{Date}/{TemperatureC}/{TemperatureF}/{Summary}")]
    public string Get([FromRoute] WeatherForecast wr)
    {
        return wr.TemperatureF.ToString();
    }
}
