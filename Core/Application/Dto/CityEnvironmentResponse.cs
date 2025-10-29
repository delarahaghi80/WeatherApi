using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class CityEnvironmentResponse
    {
        public string CityName { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public int AirQualityIndex { get; set; }
        public List<string> MajorPollutants { get; set; }
        public CoordinatesDto Coordinates { get; set; }

        public CityEnvironmentResponse()
        {
            MajorPollutants = new List<string>();
        }
    }

    public class CoordinatesDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
