using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using AirQualityInfo.DataClient;
using AirQualityInfo.DataClient.Models;
using AirQualityInfo.DataClient.Services;
using AirQualityInfo.WP.Services;
using Caliburn.Micro;

namespace AirQualityInfo.WP.ViewModels
{
    public class MainViewModel : Screen, IMainViewModel
    {
        private readonly INavigationService _navigationService;

        public DataAggregate Aggregate { get; set; }

        public MainViewModel(INavigationService navigationService, 
            ILocationService locationService, 
            IOzoneDataService dataService)
        {
            _navigationService = navigationService;

            Aggregate = new DataAggregate(locationService, dataService)
            {
                AutoInferAirqualityOnFilterPropertyChanges = true
            };
        }

        protected async override void OnActivate()
        {
            base.OnActivate();
            Aggregate.RefreshData();
        }

        public void StationSelected(ItemClickEventArgs eventArgs)
        {
            var selected = (OzoneInformation)eventArgs.ClickedItem;

            _navigationService.UriFor<StationDetailsViewModel>()
                .WithParam(vm => vm.StationId, selected.Id)
                .Navigate();
        }

        public void AboutApp()
        {
            _navigationService.UriFor<AboutViewModel>()
                .Navigate();
        }

        public void RefreshData()
        {
            Aggregate.RefreshData(forceOzoneDataRefresh: true);
        }
    }
}
