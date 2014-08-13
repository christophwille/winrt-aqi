using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityInfo.DataClient
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string url);
    }
}
