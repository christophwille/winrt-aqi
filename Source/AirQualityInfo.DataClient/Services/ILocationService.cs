using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirQualityInfo.DataClient.Models;

namespace AirQualityInfo.DataClient.Services
{
    public interface ILocationService
    {
        Task<LocationResult> GetCurrentPosition();
    }
}
