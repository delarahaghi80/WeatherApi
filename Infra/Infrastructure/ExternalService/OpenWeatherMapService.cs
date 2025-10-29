using Application.Dto;
using Domain.Entities;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.ExternalService
{
    public class OpenWeatherMapService : IWeatherDataProvider, IAirQualityProvider
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherMapSettings _settings;

        public OpenWeatherMapService(HttpClient httpClient, IOptions<OpenWeatherMapSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<WeatherData?> GetWeatherAsync(string cityName)
        {
            try
            {
                var url = $"{_settings.BaseUrl}/weather?q={cityName}&appid={_settings.ApiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<WeatherApiResponse>(json);

                if (apiResponse == null)
                    return null;

                return new WeatherData
                {
                    Temperature = apiResponse.main.temp,
                    Humidity = apiResponse.main.humidity,
                    WindSpeed = apiResponse.wind.speed,
                    Coordinates = Coordinates.Create(apiResponse.coord.lat, apiResponse.coord.lon)
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<AirQualityData?> GetAirQualityAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"{_settings.BaseUrl}/air_pollution?lat={latitude}&lon={longitude}&appid={_settings.ApiKey}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<AirPollutionApiResponse>(json);

                if (apiResponse?.list == null || !apiResponse.list.Any())
                    return null;

                var data = apiResponse.list[0];
                var pollutants = ExtractPollutants(data.components);

                return new AirQualityData
                {
                    AirQualityIndex = data.main.aqi,
                    Pollutants = pollutants
                };
            }
            catch
            {
                return null;
            }
        }

        private List<Pollutant> ExtractPollutants(AirComponentsResponse components)
        {
            var pollutants = new List<Pollutant>();

            if (components.pm2_5 > 15)
                pollutants.Add(Pollutant.Create("PM2.5", components.pm2_5));

            if (components.pm10 > 50)
                pollutants.Add(Pollutant.Create("PM10", components.pm10));

            if (components.co > 4000)
                pollutants.Add(Pollutant.Create("CO", components.co));

            if (components.no2 > 40)
                pollutants.Add(Pollutant.Create("NO2", components.no2));

            if (components.o3 > 100)
                pollutants.Add(Pollutant.Create("O3", components.o3));

            if (components.so2 > 20)
                pollutants.Add(Pollutant.Create("SO2", components.so2));

            if (!pollutants.Any())
                pollutants.Add(Pollutant.Create("No major pollutants", 0));

            return pollutants;
        }
    }
}
