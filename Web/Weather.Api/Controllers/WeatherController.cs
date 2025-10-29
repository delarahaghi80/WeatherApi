using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Weather.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
      
            private readonly IWeatherService _weatherService;
            private readonly ILogger<WeatherController> _logger;

            public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
            {
                _weatherService = weatherService;
                _logger = logger;
            }

            /// <summary>
            /// Gets environmental data for a specified city
            /// </summary>
            /// <param name="cityName">Name of the city</param>
            /// <returns>City environmental data including weather and air quality</returns>
            [HttpGet("{cityName}")]
            [ProducesResponseType(typeof(CityEnvironmentResponse), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public async Task<ActionResult<CityEnvironmentResponse>> GetCityEnvironment(string cityName)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(cityName))
                    {
                        _logger.LogWarning("Empty city name provided");
                        return BadRequest("City name cannot be empty");
                    }

                    _logger.LogInformation("Fetching environment data for city: {CityName}", cityName);

                    var result = await _weatherService.GetCityEnvironmentAsync(cityName);

                    if (result == null)
                    {
                        _logger.LogWarning("City not found: {CityName}", cityName);
                        return NotFound($"City '{cityName}' not found");
                    }

                    _logger.LogInformation("Successfully retrieved data for: {CityName}", cityName);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting environment data for city: {CityName}", cityName);
                    return StatusCode(500, "An error occurred while processing your request");
                }
            }
        }
    }

