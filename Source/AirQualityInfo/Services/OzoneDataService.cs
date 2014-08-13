using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirQualityInfo.DataClient;
using AirQualityInfo.DataClient.Models;
using AirQualityInfo.Models;

namespace AirQualityInfo.Services
{
    public static class OzoneDataService
    {
        private static bool _loadingOzoneData = false;
        private static object _loadingOzoneDataLock = new object();
        public static bool LoadingOzoneData
        {
            get
            {
                bool retval = false;
                lock (_loadingOzoneDataLock)
                {
                    retval = _loadingOzoneData;
                }
                return retval;
            }
            set
            {
                if (value != _loadingOzoneData)
                {
                    lock (_loadingOzoneDataLock)
                    {
                        _loadingOzoneData = value;
                    }
                }
            }
        }

        public static async Task<List<OzoneInformation>> LoadAsync()
        {
            List<OzoneInformation> result = null;

            if (LoadingOzoneData) return result;

            try
            {
                LoadingOzoneData = true;

                var proxy = new UmweltbundesamtOzoneDataClient(new DefaultHttpClient());
                var data = await proxy.RetrieveAsync();

                result = data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading ozone data: " + ex.ToString());
            }
            finally
            {
                LoadingOzoneData = false;
            }

            return result;
        }
    }
}
