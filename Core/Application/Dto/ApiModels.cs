using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class WeatherApiResponse
    {
        public CoordResponse coord { get; set; }
        public MainResponse main { get; set; }
        public WindResponse wind { get; set; }
        public string name { get; set; }
    }

    public class CoordResponse
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class MainResponse
    {
        public double temp { get; set; }
        public int humidity { get; set; }
    }

    public class WindResponse
    {
        public double speed { get; set; }
    }

    // OpenWeatherMap Air Pollution API Response
    public class AirPollutionApiResponse
    {
        public List<AirPollutionDataResponse> list { get; set; }
    }

    public class AirPollutionDataResponse
    {
        public AirMainResponse main { get; set; }
        public AirComponentsResponse components { get; set; }
    }

    public class AirMainResponse
    {
        public int aqi { get; set; }
    }

    public class AirComponentsResponse
    {
        public double co { get; set; }
        public double no2 { get; set; }
        public double o3 { get; set; }
        public double so2 { get; set; }
        public double pm2_5 { get; set; }
        public double pm10 { get; set; }
    }
}
