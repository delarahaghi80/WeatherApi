namespace Domain.Entities
{
    /// <summary>
    /// Core domain entity representing environmental data for a city
    /// </summary>
    public class CityEnvironment
    {
        public string CityName { get; private set; }
        public double Temperature { get; private set; } // Celsius
        public int Humidity { get; private set; } // %
        public double WindSpeed { get; private set; } // m/s
        public int AirQualityIndex { get; private set; }
        public List<Pollutant> Pollutants { get; private set; }
        public Coordinates Coordinates { get; private set; }

        private CityEnvironment()
        {
            Pollutants = new List<Pollutant>();
        }

        public static CityEnvironment Create(
            string cityName,
            double temperature,
            int humidity,
            double windSpeed,
            int airQualityIndex,
            List<Pollutant> pollutants,
            Coordinates coordinates)
        {
            if (string.IsNullOrWhiteSpace(cityName))
                throw new ArgumentException("City name cannot be empty", nameof(cityName));

            if (humidity < 0 || humidity > 100)
                throw new ArgumentException("Humidity must be between 0 and 100", nameof(humidity));

            if (windSpeed < 0)
                throw new ArgumentException("Wind speed cannot be negative", nameof(windSpeed));

            if (airQualityIndex < 1 || airQualityIndex > 5)
                throw new ArgumentException("AQI must be between 1 and 5", nameof(airQualityIndex));

            return new CityEnvironment
            {
                CityName = cityName,
                Temperature = Math.Round(temperature, 1),
                Humidity = humidity,
                WindSpeed = Math.Round(windSpeed, 1),
                AirQualityIndex = airQualityIndex,
                Pollutants = pollutants ?? new List<Pollutant>(),
                Coordinates = coordinates
            };
        }

        public bool HasPoorAirQuality() => AirQualityIndex >= 4;

        public bool IsExtremeCold() => Temperature < -10;

        public bool IsExtremeHeat() => Temperature > 40;
    }

    public class Pollutant
    {
        public string Name { get; private set; }
        public double Concentration { get; private set; }
        public string Unit { get; private set; }

        private Pollutant() { }

        public static Pollutant Create(string name, double concentration, string unit = "μg/m³")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Pollutant name cannot be empty", nameof(name));

            if (concentration < 0)
                throw new ArgumentException("Concentration cannot be negative", nameof(concentration));

            return new Pollutant
            {
                Name = name,
                Concentration = concentration,
                Unit = unit
            };
        }

        public override string ToString() => $"{Name} ({Concentration} {Unit})";
    }

    public class Coordinates
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        private Coordinates() { }

        public static Coordinates Create(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new ArgumentException("Latitude must be between -90 and 90", nameof(latitude));

            if (longitude < -180 || longitude > 180)
                throw new ArgumentException("Longitude must be between -180 and 180", nameof(longitude));

            return new Coordinates
            {
                Latitude = latitude,
                Longitude = longitude
            };
        }
    }
}