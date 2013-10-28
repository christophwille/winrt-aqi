using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirQualityInfo.DataClient.Models;
using AirQualityInfo.Models;
using AirQualityInfo.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WindowsMapsHelper;

namespace AirQualityInfo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public const string CurrentLocationPropertyName = "CurrentLocation";
        private GeoCoordinate _currentLocation = null;

        public GeoCoordinate CurrentLocation
        {
            get
            {
                return _currentLocation;
            }
            set
            {
                Set(CurrentLocationPropertyName, ref _currentLocation, value);
            }
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

        public const string OzoneDataPropertyName = "OzoneData";
        private List<OzoneInformation> _ozoneData = null;

        private List<OzoneInformation> OzoneData
        {
            get { return _ozoneData; }
            set
            {
                Set(OzoneDataPropertyName, ref _ozoneData, value);
                OzoneDisplayData = null;
            }
        }

        // Bindable data
        public const string OzoneDisplayDataPropertyName = "OzoneDisplayData";
        private List<OzoneInformation> _ozoneDisplayData = null;

        public List<OzoneInformation> OzoneDisplayData
        {
            get { return _ozoneDisplayData; }
            set
            {
                Set(OzoneDisplayDataPropertyName, ref _ozoneDisplayData, value);
            }
        }

        public const string UpdateInProgressPropertyName = "UpdateInProgress";
        private bool _updateInProgress = false;

        public bool UpdateInProgress
        {
            get
            {
                return _updateInProgress;
            }
            set
            {
                Set(UpdateInProgressPropertyName, ref _updateInProgress, value);
            }
        }

        public const string CurrentFilterPropertyName = "CurrentFilter";
        private FilterByState _currentFilter = FilterByState.GetDefaultFilter();

        public FilterByState CurrentFilter
        {
            get
            {
                return _currentFilter;
            }
            set
            {
                Set(CurrentFilterPropertyName, ref _currentFilter, value);
            }
        }

        public const string CurrentSortPropertyName = "CurrentSort";
        private SortByOption _currentSort = SortByOption.GetDefaultSort();

        public SortByOption CurrentSort
        {
            get
            {
                return _currentSort;
            }
            set
            {
                Set(CurrentSortPropertyName, ref _currentSort, value);
            }
        }

        private int _outstandingAsyncOperations = 0;

        public async void LookupPositionAsync()
        {
            var position = await PerformOperationAsync(CurrentPositionService.LookupAsync);

            if (null != position)
            {
                CurrentLocation = position;
                InferAirQuality();
            }
        }

        public async void LoadOzoneDataAsync()
        {
            var data = await PerformOperationAsync(OzoneDataService.LoadAsync);

            if (null != data)
            {
                OzoneData = data;
                InferAirQuality();
            }
        }

        private void UpdateProgress()
        {
            if (0 == _outstandingAsyncOperations)
                UpdateInProgress = false;
        }

        private void InferAirQuality()
        {
            // If any async operations are still in progress, return immediately
            if (0 != _outstandingAsyncOperations) return;

            // If there is no ozone data, we cannot do any calculations
            if (null == OzoneData) return;

            // If there is no location, we at least have the ozone data itself without distance information
            if (null == CurrentLocation)
            {
                SelectedMeasurement = null;
                ResetDisplayMeasurements();
                return;
            }

            // Work with copies of the data (if while processing another async operation is triggered)
            GeoCoordinate currentPos = CurrentLocation;
            List<OzoneInformation> ozoneMeasurements = OzoneData.ToList();

            OzoneInformation nearestMeasurement = null;
            double nearestDistance = double.MaxValue;

            foreach (var dataPoint in ozoneMeasurements)
            {
                if (!dataPoint.HasOzoneData()) continue;

                double distance = currentPos.DistanceTo(dataPoint.Location);

                dataPoint.DistanceToCurrentPosition = distance;

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestMeasurement = dataPoint;
                }
            }

            // The default measurement is the nearest measurement
            SelectedMeasurement = nearestMeasurement ?? null;
            ResetDisplayMeasurements();
        }

        public void ResetDisplayMeasurements()
        {
            CurrentFilter = FilterByState.GetDefaultFilter();
            CurrentSort = SortByOption.GetDefaultSort();

            DisplayMeasurements();
        }

        public void DisplayMeasurements()
        {
            if (null == OzoneData) return;

            var defaultFilter = FilterByState.GetDefaultFilter();
            string stateFilter = CurrentFilter.Id;

            var query = OzoneData.ToList().AsQueryable();

            if (stateFilter != defaultFilter.Id)
                query = query.Where(oi => 0 == String.Compare(stateFilter, oi.State, StringComparison.OrdinalIgnoreCase));

            if (CurrentSort.SortBy == SortByOptionEnum.Alpha)
            {
                query = query.OrderBy(oi => oi.Name);
            }
            else if (CurrentSort.SortBy == SortByOptionEnum.Distance)
            {
                query = query.OrderBy(oi => oi.DistanceToCurrentPosition);
            }
            else if (CurrentSort.SortBy == SortByOptionEnum.OneHourAverage)
            {
                query = query.OrderBy(oi => oi.OneHourAverage);
            }

            OzoneDisplayData = query.ToList();
        }

        private async Task<T> PerformOperationAsync<T>(Func<Task<T>> operationAsync)
            where T: class
        {
            if (null == operationAsync)
                throw new ArgumentNullException();

            T result = null;
            try
            {
                UpdateInProgress = true;

                Interlocked.Increment(ref _outstandingAsyncOperations);
                result = await operationAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PerformOperationAsync: " + ex.ToString());
            }
            finally
            {
                Interlocked.Decrement(ref _outstandingAsyncOperations);
                UpdateProgress();
            }

            return result;
        }

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
                    ?? (_refreshDataCommand = new RelayCommand(RefreshData));
            }
        }

        public void RefreshData()
        {
            LoadOzoneDataAsync();
            LookupPositionAsync();
        }
    }
}
