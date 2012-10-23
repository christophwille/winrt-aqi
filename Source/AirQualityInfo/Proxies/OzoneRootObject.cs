using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityInfo.Proxies
{
    class OzoneRootObject
    {
        public string id { get; set; }
        public string name { get; set; }
        public string timestamp_utc { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public int height { get; set; }
        public string state { get; set; }
        public int? ozon1h { get; set; }
        public string ozon1hTimestamp_utc { get; set; }
        public int? ozon8h { get; set; }
        public int? ozon1hMax { get; set; }
        public string ozon1hMaxTimestamp_utc { get; set; }
    }
}
