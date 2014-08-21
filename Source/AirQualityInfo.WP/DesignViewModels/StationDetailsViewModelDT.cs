using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirQualityInfo.DataClient.Models;
using AirQualityInfo.WP.ViewModels;

namespace AirQualityInfo.WP.DesignViewModels
{
    public class StationDetailsViewModelDT : IStationDetailsViewModel
    {
        public OzoneInformation Station { get; set; }

        public StationDetailsViewModelDT()
        {
            Station = new OzoneInformation()
            {
                Id = "1",
                Name = "Leoben Zentrum",
                OneHourAverage = 5,
                OneHourAverageTimestampLocal = new DateTime(2012, 10, 22, 14, 00, 00),
                EightHoursAverage = 18,
                Height = 540,
                State = "ST",
                DistanceToCurrentPosition = 1.5
            };
        }
    }
}
