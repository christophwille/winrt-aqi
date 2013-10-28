using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityInfo.DataClient.Models
{
    public partial class GeoCoordinate
    {
        public GeoCoordinate(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double DistanceTo(GeoCoordinate second)
        {
            return distance(Latitude, Longitude, second.Latitude, second.Longitude, 'K');
        }
    }
}
