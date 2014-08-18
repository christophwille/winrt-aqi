using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirQualityInfo.DataClient;
using AirQualityInfo.DataClient.Models;
using AirQualityInfo.WP.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WindowsMapsHelper;

namespace AirQualityInfo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel()
        {
            Aggregate = new DataAggregate(new LocationService(), new OzoneDataService(new DefaultHttpClient()));
            Aggregate.ResetAnyMeasurementSelection += Aggregate_ResetAnyMeasurementSelection;
        }

        void Aggregate_ResetAnyMeasurementSelection(object sender, OzoneInformation info)
        {
            SelectedMeasurement = info;
        }

        public const string SelectedMeasurementPropertyName = "SelectedMeasurement";
        private OzoneInformation _selectedMeasurement = null;

        public OzoneInformation SelectedMeasurement
        {
            get
            {
                return _selectedMeasurement;
            }
            set
            {
                Set(SelectedMeasurementPropertyName, ref _selectedMeasurement, value);
                _showMapCommand.RaiseCanExecuteChanged();
            }
        }

        public DataAggregate Aggregate { get; set; }

        private RelayCommand _showMapCommand;
        public RelayCommand ShowMapCommand
        {
            get
            {
                return _showMapCommand
                    ?? (_showMapCommand = new RelayCommand(
                        ShowMap,
                        () => CanShowMap));
            }
        }

        public bool CanShowMap
        {
            get { return (SelectedMeasurement != null); }
        }

        public void ShowMap()
        {
            // TODO: Show a pin or something with current air quality measurements? (nice to have, seems not possible right now)
            var options = new MapOptions()
                              {
                                  CenterPoint =
                                      new MapPosition(SelectedMeasurement.Location.Latitude,
                                                      SelectedMeasurement.Location.Longitude)
                              };

            MapsHelper.ShowMapWithOptionsAsync(options);
        }

        private RelayCommand _refreshDataCommand;
        public RelayCommand RefreshDataCommand
        {
            get
            {
                return _refreshDataCommand
                    ?? (_refreshDataCommand = new RelayCommand(Aggregate.RefreshData));
            }
        }
    }
}
