using Microsoft.Extensions.Logging;
using Moq;
using WebApiTemplate.Controllers;

namespace WebApiTemplate.Tests
{
    public class WeatherForecastTests
    {
        [Fact]
        public async Task FarenheitShouldReturnCelsius()
        {
            var logger = new Mock<ILogger<WeatherForecastController>> ();
            var controller = new WeatherForecastController(logger.Object);
            var result = await controller.ConvertToCelsius(100);

            Assert.Equal(37, result);

        }
    }
}