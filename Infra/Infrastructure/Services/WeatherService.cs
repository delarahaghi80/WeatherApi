using Application.Dto;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherDataProvider _weatherDataProvider;
        private readonly IAirQualityProvider _airQualityProvider;

        public WeatherService(
            IWeatherDataProvider weatherDataProvider,
            IAirQualityProvider airQualityProvider)
        {
            _weatherDataProvider = weatherDataProvider;
            _airQualityProvider = airQualityProvider;
        }

        public async Task<CityEnvironmentResponse?> GetCityEnvironmentAsync(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return null;

            // Get weather data and coordinates
            var weatherData = await _weatherDataProvider.GetWeatherAsync(cityName);
            if (weatherData == null)
                return null;

            // Get air quality data
            var airQualityData = await _airQualityProvider.GetAirQualityAsync(
                weatherData.Coordinates.Latitude,
                weatherData.Coordinates.Longitude);

            // Create domain entity
            var cityEnvironment = CityEnvironment.Create(
                cityName,
                weatherData.Temperature,
                weatherData.Humidity,
                weatherData.WindSpeed,
                airQualityData?.AirQualityIndex ?? 1,
                airQualityData?.Pollutants ?? new List<Pollutant>(),
                weatherData.Coordinates
            );

            // Map to DTO
            return MapToDto(cityEnvironment);
        }

        private CityEnvironmentResponse MapToDto(CityEnvironment entity)
        {
            return new CityEnvironmentResponse
            {
                CityName = entity.CityName,
                Temperature = entity.Temperature,
                Humidity = entity.Humidity,
                WindSpeed = entity.WindSpeed,
                AirQualityIndex = entity.AirQualityIndex,
                MajorPollutants = entity.Pollutants.Select(p => p.ToString()).ToList(),
                Coordinates = new CoordinatesDto
                {
                    Latitude = entity.Coordinates.Latitude,
                    Longitude = entity.Coordinates.Longitude
                }
            };
        }
    }

 
}
