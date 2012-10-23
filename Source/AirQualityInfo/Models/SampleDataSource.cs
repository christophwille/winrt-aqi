using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityInfo.Models
{
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private readonly List<OzoneInformation> _measurements = new List<OzoneInformation>();
        public List<OzoneInformation> OzoneData
        {
            get { return _measurements; }
        }

        public SampleDataSource()
        {
            var dm = new OzoneInformation()
            {
                Id = "1",
                Name = "Leoben Zentrum",
                OneHourAverage = 5,
                OneHourAverageTimestampLocal = new DateTime(2012, 10, 22, 14, 00, 00),
                EightHoursAverage = 18,
                Height = 540,
                State = "ST"
            };

            _measurements.Add(dm);
        }
    }
}
