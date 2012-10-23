using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirQualityInfo.Models;
using Windows.Devices.Geolocation;

namespace AirQualityInfo.Services
{
    public static class CurrentPositionService
    {
        private static bool _acquiringPosition = false;
        private static object _acquiringPositionLock = new object();
        public static bool AcquiringPosition
        {
            get
            {
                bool retval = false;
                lock (_acquiringPositionLock)
                {
                    retval = _acquiringPosition;
                }
                return retval;
            }
            set
            {
                if (value != _acquiringPosition)
                {
                    lock (_acquiringPositionLock)
                    {
                        _acquiringPosition = value;
                    }
                }
            }
        }

        public static async Task<GeoCoordinate> LookupAsync()
        {
            GeoCoordinate result = null;

            if (AcquiringPosition) 
                return result;

            try
            {
                AcquiringPosition = true;

                var currentGeoLocator = new Geolocator();
                var location = await currentGeoLocator.GetGeopositionAsync();

                result = new GeoCoordinate(location.Coordinate.Latitude, location.Coordinate.Longitude);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error looking up position: " + ex.ToString());
            }
            finally
            {
                AcquiringPosition = false;
            }

            return result;
        }
    }
}
