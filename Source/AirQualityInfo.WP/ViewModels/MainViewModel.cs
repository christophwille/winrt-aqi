using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
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

            // do this before listening to INPC callbacks
            LoadLocalSettings();

            Aggregate.PropertyChanged += Aggregate_PropertyChanged;
        }

        private void LoadLocalSettings()
        {
            var settings = ApplicationData.Current.LocalSettings;

            object filterSetting = settings.Values[CurrentFilterSettingsKey];
            if (null != filterSetting)
            {
                FilterByState preselectState = Aggregate.States.FirstOrDefault(s => s.Id == filterSetting.ToString());
                if (null != preselectState) Aggregate.CurrentFilter = preselectState;
            }

            object sortSetting = settings.Values[CurrentSortSettingsKey];
            if (null != sortSetting)
            {
                SortByOption preselectSort = Aggregate.SortOptions.FirstOrDefault(s => s.SortBy == (SortByOptionEnum)((int)sortSetting));
                if (null != preselectSort) Aggregate.CurrentSort = preselectSort;
            }
        }

        private const string CurrentFilterSettingsKey = "CurrentFilter";
        private const string CurrentSortSettingsKey = "CurrentSort";
        void Aggregate_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentFilter")
            {
                ApplicationData.Current.LocalSettings.Values[CurrentFilterSettingsKey] = Aggregate.CurrentFilter.Id;
            }
            else if (e.PropertyName == "CurrentSort")
            {
                ApplicationData.Current.LocalSettings.Values[CurrentSortSettingsKey] = (int)Aggregate.CurrentSort.SortBy;
            }
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
