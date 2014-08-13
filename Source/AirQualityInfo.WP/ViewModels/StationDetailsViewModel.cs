using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirQualityInfo.DataClient.Models;
using AirQualityInfo.WP.Services;
using Caliburn.Micro;

namespace AirQualityInfo.WP.ViewModels
{
    public class StationDetailsViewModel : Screen, IStationDetailsViewModel
    {
        private readonly IOzoneDataService _dataService;
        public string StationId { get; set; }
        public OzoneInformation Station { get; set; }

        public StationDetailsViewModel(IOzoneDataService dataService)
        {
            _dataService = dataService;
        }

        protected async override void OnActivate()
        {
            base.OnActivate();

            Station = await _dataService.GetStationAsync(StationId);
        }
    }
}
