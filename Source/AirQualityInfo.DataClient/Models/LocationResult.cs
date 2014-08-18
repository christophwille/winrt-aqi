using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityInfo.DataClient.Models
{
    public class LocationResult
    {
        public LocationResult(GeoCoordinate pos)
        {
            Coordinate = pos;
        }

        public LocationResult(string errMsg)
        {
            ErrorMessage = errMsg;
        }

        public GeoCoordinate Coordinate { get; set; }

        public bool Succeeded
        {
            get { return null != Coordinate; }
        }

        public string ErrorMessage { get; set; }
    }
}
