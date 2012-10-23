using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AirQualityInfo.Models;
using Newtonsoft.Json;

namespace AirQualityInfo.Proxies
{
    public class UmweltbundesamtOzoneData
    {
        private const string JsonpResponseStart = "jsonpResponse(";
        private const string JsonpResponseEnd = ")";

        public async Task<List<OzoneInformation>> Download()
        {
            var client = new HttpClient();
            List<OzoneRootObject> result = null;

            try
            {
                var response = await client.GetAsync("http://luft.umweltbundesamt.at/pub/ozonbericht/aktuell.json");
                string json = await response.Content.ReadAsStringAsync();
                client.Dispose();

                json = json.Trim();
                if (!String.IsNullOrWhiteSpace(json) && json.StartsWith(JsonpResponseStart) && json.EndsWith(JsonpResponseEnd))
                {
                    int originalLength = json.Length;
                    json = json.Substring(JsonpResponseStart.Length,
                                   originalLength - JsonpResponseStart.Length - JsonpResponseEnd.Length);
                    result = JsonConvert.DeserializeObject<List<OzoneRootObject>>(json);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Downloading ozone data, error: " + ex.ToString());
                return null;
            }

            return result.Select(r => new OzoneInformation()
                                          {
                                              Id = r.id,
                                              Name = r.name,
                                              Location = new GeoCoordinate(r.lat, r.lon),
                                              OneHourAverage = r.ozon1h,
                                              OneHourAverageTimestampLocal = ToLocalTime(r.ozon1hTimestamp_utc),
                                              OneHourMax = r.ozon1hMax,
                                              OneHourMaxTimestampLocal = ToLocalTime(r.ozon1hMaxTimestamp_utc),
                                              EightHoursAverage = r.ozon8h,
                                              Height = r.height,
                                              State = r.state,
                                              TimestampLocal = ToLocalTime(r.timestamp_utc)
                                          })
                                          .OrderBy(r => r.Name)
                                          .ToList();
        }

        private DateTime? ToLocalTime(string jsonDatetime)
        {
            DateTime parsed;
            bool ok = DateTime.TryParse(jsonDatetime, CultureInfo.InvariantCulture, DateTimeStyles.None , out parsed);

            if (ok)
                return parsed.ToLocalTime();

            return null;
        }
    }
}
