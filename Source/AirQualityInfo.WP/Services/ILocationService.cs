using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirQualityInfo.WP.Models;

namespace AirQualityInfo.WP.Services
{
    public interface ILocationService
    {
        Task<LocationResult> GetCurrentPosition();
    }
}
