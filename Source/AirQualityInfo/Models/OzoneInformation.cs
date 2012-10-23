using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityInfo.Models
{
    public class OzoneInformation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? TimestampLocal { get; set; }

        public GeoCoordinate Location { get; set; }

        public int Height { get; set; }
        public string State { get; set; }
        public int? OneHourAverage { get; set; }
        public DateTime? OneHourAverageTimestampLocal { get; set; }
        public int? EightHoursAverage { get; set; }
        public int? OneHourMax { get; set; }
        public DateTime? OneHourMaxTimestampLocal { get; set; }

        // TODO: Add logic to decide when the data point contains info useful to end-user
        public bool HasOzoneData()
        {
            return true;
        }

        public string DisplayOneHourAverage
        {
            get { return String.Format("Ein-Stunden Durchschnitt: {0} µg/m3", OneHourMax); }
        }

        public string DisplayOneHourMax
        {
            get { return String.Format("Ein-Stunden Maximum: {0} µg/m3", OneHourMax); }
        }

        public string DisplayEightHoursAverage
        {
            get { return String.Format("8-Stunden Durchschnitt: {0} µg/m3", EightHoursAverage); }
        }

        public string DisplayOneHourAverageTime
        {
            get { return String.Format("Messzeitpunkt: {0}", OneHourAverageTimestampLocal); }
        }

        public string DisplayInfoFreshness
        {
            get { return String.Format("Letztes Update: {0}", TimestampLocal); }
        }
    }
}
