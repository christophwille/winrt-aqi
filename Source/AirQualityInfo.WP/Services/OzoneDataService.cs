using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirQualityInfo.DataClient;
using AirQualityInfo.DataClient.Models;
using AirQualityInfo.DataClient.Services;

namespace AirQualityInfo.WP.Services
{
    public class OzoneDataService : IOzoneDataService
    {
        private readonly IHttpClient _httpClient;

        public OzoneDataService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private List<OzoneInformation> _loadedStations = null;
        public async Task<List<OzoneInformation>> LoadAsync(bool forceReload)
        {
            if (_loadedStations != null && !forceReload)
                return _loadedStations;

            _loadedStations = await InternalLoadAsync().ConfigureAwait(false);
            return _loadedStations;
        }

        public async Task<OzoneInformation> GetStationAsync(string stationId)
        {
            var stations = await LoadAsync(false).ConfigureAwait(false);

            if (null == stations) return null;
            return stations.FirstOrDefault(s => s.Id == stationId);
        }

        private bool _currentlyLoading = false;
        private async Task<List<OzoneInformation>> InternalLoadAsync()
        {
            if (_currentlyLoading) return null;

            try
            {
                _currentlyLoading = true;

                var proxy = new UmweltbundesamtOzoneDataClient(_httpClient);
                var data = await proxy.RetrieveAsync().ConfigureAwait(false);

                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading ozone data: " + ex.ToString());
            }
            finally
            {
                _currentlyLoading = false;
            }

            return null;
        }
    }
}
