namespace WebApiTemplate;

public class WeatherForecast
{

    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// The 2 digit country code
    /// </summary>
    /// <example>New Zealand, bro</example>
    public string? Summary { get; set; }
}
