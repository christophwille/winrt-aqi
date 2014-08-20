using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirQualityInfo.DataClient;
using AirQualityInfo.DataClient.Models;
using AirQualityInfo.WP.ViewModels;

namespace AirQualityInfo.WP.DesignViewModels
{
    public class MainViewModelDT : IMainViewModel
    {
        public DataAggregate Aggregate { get; set; }
        public string MesswerteHeader { get; set; }


        public MainViewModelDT()
        {
            MesswerteHeader = "MESSWERTE (x)";
            Aggregate = new DataAggregate(null, null);

            Aggregate.OzoneDisplayData = new List<OzoneInformation>()
            {
                new OzoneInformation()
                {
                    Id = "1",
                    Name = "Leoben Zentrum",
                    OneHourAverage = 5,
                    OneHourAverageTimestampLocal = new DateTime(2012, 10, 22, 14, 00, 00),
                    EightHoursAverage = 18,
                    Height = 540,
                    State = "ST"
                },
                new OzoneInformation()
                {
                    Id = "2",
                    Name = "Bad Ischl",
                    OneHourAverage = 221,
                    OneHourAverageTimestampLocal = new DateTime(2012, 10, 22, 14, 00, 00),
                    EightHoursAverage = 240,
                    Height = 568,
                    State = "OÖ"
                },
            };
        }
    }
}
