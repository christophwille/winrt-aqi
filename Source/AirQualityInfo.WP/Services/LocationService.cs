using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Devices.Geolocation;
using AirQualityInfo.DataClient.Models;
using AirQualityInfo.DataClient.Services;

namespace AirQualityInfo.WP.Services
{
    // http://msdn.microsoft.com/en-us/library/windows/apps/hh465148.aspx
    public class LocationService : ILocationService
    {
        public async Task<LocationResult> GetCurrentPosition()
        {
            try
            {
                var geolocator = new Geolocator()
                {
                    DesiredAccuracyInMeters = 50
                };

                Geoposition pos = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(1),
                    timeout: TimeSpan.FromSeconds(10)
                    );

                return new LocationResult(new GeoCoordinate(pos.Coordinate.Latitude, pos.Coordinate.Longitude));
            }
            catch (UnauthorizedAccessException)
            {
                return new LocationResult("Die aktuelle Position ist zur Berechnung der Distanz zur Messstation notwendig, Zugriff wurde verweigert.");
            }
            catch (Exception)
            {
            }

            return new LocationResult("Aktuelle Position konnte nicht ermittelt werden");
        }
    }
}
