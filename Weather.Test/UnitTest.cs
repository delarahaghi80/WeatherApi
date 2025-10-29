using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Weather.Api.Controllers;


namespace Weather.Test
{
    public class UnitTest
    {
        private readonly Mock<IWeatherService> _mockWeatherService;
        private readonly Mock<ILogger<WeatherController>> _mockLogger;
        private readonly WeatherController _controller;

        public UnitTest()


        {
            _mockWeatherService = new Mock<IWeatherService>();
            _mockLogger = new Mock<ILogger<WeatherController>>();
            _controller = new WeatherController(_mockWeatherService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetCityEnvironment_WithValidCity_ReturnsOkResult()
        {
            // Arrange
            var cityName = "Tehran";
            var expectedResponse = new CityEnvironmentResponse
            {
                CityName = cityName,
                Temperature = 25.5,
                Humidity = 40,
                WindSpeed = 3.2,
                AirQualityIndex = 3,
                MajorPollutants = new List<string> { "PM2.5 (25 μg/m³)" },
                Coordinates = new CoordinatesDto { Latitude = 35.6892, Longitude = 51.3890 }
            };

            _mockWeatherService
                .Setup(s => s.GetCityEnvironmentAsync(cityName))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetCityEnvironment(cityName);

            // Assert
            var okResult = Assert.IsType<ActionResult<CityEnvironmentResponse>>(result);
            var returnValue = Assert.IsType<OkObjectResult>(okResult.Result);
            var actualResponse = Assert.IsType<CityEnvironmentResponse>(returnValue.Value);

            Assert.Equal(cityName, actualResponse.CityName);
            Assert.Equal(25.5, actualResponse.Temperature);
            Assert.Equal(40, actualResponse.Humidity);
            Assert.Equal(3.2, actualResponse.WindSpeed);
            Assert.Equal(3, actualResponse.AirQualityIndex);
        }

        [Fact]
        public async Task GetCityEnvironment_WithEmptyCity_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetCityEnvironment("");

            // Assert
            var badRequestResult = Assert.IsType<ActionResult<CityEnvironmentResponse>>(result);
            Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
        }

        [Fact]
        public async Task GetCityEnvironment_WithUnknownCity_ReturnsNotFound()
        {
            // Arrange
            var cityName = "UnknownCity123";
            _mockWeatherService
                .Setup(s => s.GetCityEnvironmentAsync(cityName))
                .ReturnsAsync((CityEnvironmentResponse?)null);

            // Act
            var result = await _controller.GetCityEnvironment(cityName);

            // Assert
            var notFoundResult = Assert.IsType<ActionResult<CityEnvironmentResponse>>(result);
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }

        [Fact]
        public async Task GetCityEnvironment_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var cityName = "Tehran";
            _mockWeatherService
                .Setup(s => s.GetCityEnvironmentAsync(cityName))
                .ThrowsAsync(new Exception("API Error"));

            // Act
            var result = await _controller.GetCityEnvironment(cityName);

            // Assert
            var statusCodeResult = Assert.IsType<ActionResult<CityEnvironmentResponse>>(result);
            var objectResult = Assert.IsType<ObjectResult>(statusCodeResult.Result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task GetCityEnvironment_WithInvalidInput_ReturnsBadRequest(string invalidCity)
        {
            // Act
            var result = await _controller.GetCityEnvironment(invalidCity);

            // Assert
            var badRequestResult = Assert.IsType<ActionResult<CityEnvironmentResponse>>(result);
            Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
        }
    }
}