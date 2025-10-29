using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class AirQualityData
    {
        public int AirQualityIndex { get; set; }
        public List<Pollutant> Pollutants { get; set; }
    }
}
