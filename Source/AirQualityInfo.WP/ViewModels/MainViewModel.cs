using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using AirQualityInfo.DataClient.Models;
using AirQualityInfo.Models;
using AirQualityInfo.WP.Services;
using Caliburn.Micro;

namespace AirQualityInfo.WP.ViewModels
{
    public class MainViewModel : Screen
    {
        private readonly INavigationService _navigationService;
        private readonly IOzoneDataService _dataService;

        public bool UpdateInProgress { get; set; }
        public GeoCoordinate CurrentLocation { get; set; }
        public ObservableCollection<OzoneInformation> Stations { get; set; }
        public string FilterDisplay { get; set; }


        private List<OzoneInformation> _loadedStations = null;
        private FilterByState _currentFilter = FilterByState.GetDefaultFilter();
        private SortByOption _currentSort = SortByOption.GetDefaultSort();

        public MainViewModel(INavigationService navigationService, IOzoneDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
        }

        protected async override void OnActivate()
        {
            base.OnActivate();

            _loadedStations = await _dataService.LoadAsync(false);

            // TODO: Fix for filtering && do a null check && do cache the stuff
            Stations = new ObservableCollection<OzoneInformation>(_loadedStations);
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
    }
}
