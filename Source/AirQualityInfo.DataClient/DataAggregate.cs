using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirQualityInfo.DataClient.Models;
using AirQualityInfo.DataClient.Services;

namespace AirQualityInfo.DataClient 
{
    public class DataAggregate : BindableBase
    {
        private readonly ILocationService _locationService;
        private readonly IOzoneDataService _ozoneDataService;

        public DataAggregate(ILocationService locationService, IOzoneDataService ozoneDataService)
        {
            _locationService = locationService;
            _ozoneDataService = ozoneDataService;
        }

        public bool AutoInferAirqualityOnFilterPropertyChanges { get; set; }

        private GeoCoordinate _currentLocation = null;

        public GeoCoordinate CurrentLocation
        {
            get
            {
                return _currentLocation;
            }
            set
            {
                SetProperty(ref _currentLocation, value);
            }
        }

        private bool _updateInProgress = false;

        public bool UpdateInProgress
        {
            get
            {
                return _updateInProgress;
            }
            set
            {
                SetProperty(ref _updateInProgress, value);
            }
        }

        private List<OzoneInformation> _ozoneData = null;

        private List<OzoneInformation> OzoneData
        {
            get { return _ozoneData; }
            set
            {
                SetProperty(ref _ozoneData, value);
                OzoneDisplayData = null;
            }
        }

        private List<OzoneInformation> _ozoneDisplayData = null;

        public List<OzoneInformation> OzoneDisplayData
        {
            get { return _ozoneDisplayData; }
            set
            {
                SetProperty(ref _ozoneDisplayData, value);
            }
        }

        public List<FilterByState> States { get { return FilterByState.GetStates(); }}
        private FilterByState _currentFilter = FilterByState.GetDefaultFilter();

        public FilterByState CurrentFilter
        {
            get
            {
                return _currentFilter;
            }
            set
            {
                if (SetProperty(ref _currentFilter, value) && AutoInferAirqualityOnFilterPropertyChanges)
                {
                    DisplayMeasurements();
                }
            }
        }

        public List<SortByOption> SortOptions { get { return SortByOption.GetSortings(); }}
        private SortByOption _currentSort = SortByOption.GetDefaultSort();

        public SortByOption CurrentSort
        {
            get
            {
                return _currentSort;
            }
            set
            {
                if (SetProperty(ref _currentSort, value) && AutoInferAirqualityOnFilterPropertyChanges)
                {
                    DisplayMeasurements();
                }
            }
        }

        private int _outstandingAsyncOperations = 0;

        private async Task<T> PerformOperationAsync<T>(Func<Task<T>> operationAsync)
                where T : class
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
                OnResetAnyMeasurementSelection(null);
                DisplayMeasurementsWithoutCurrentPosition();
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
            OnResetAnyMeasurementSelection(nearestMeasurement ?? null);
            DisplayMeasurements();
        }

        public event EventHandler<OzoneInformation> ResetAnyMeasurementSelection;

        protected void OnResetAnyMeasurementSelection(OzoneInformation info)
        {
            var e = ResetAnyMeasurementSelection;

            if (null != e)
                e.Invoke(this, info);
        }

        public void DisplayMeasurementsWithoutCurrentPosition()
        {
            // If we have no geoposition, we need to reset the sort order - if it was set to distance
            if (CurrentSort.SortBy == SortByOptionEnum.Distance)
            {
                CurrentSort = SortByOption.GetDefaultSort();
            }

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

        public void RefreshData(bool forceOzoneDataRefresh=false)
        {
            LoadOzoneDataAsync(forceOzoneDataRefresh);
            LookupPositionAsync();
        }

        public async Task LookupPositionAsync()
        {
            var result = await PerformOperationAsync(_locationService.GetCurrentPosition);

            if (result.Succeeded)
            {
                CurrentLocation = result.Coordinate;
                InferAirQuality();
            }
        }

        public async Task LoadOzoneDataAsync(bool forceOzoneDataRefresh)
        {
            var data = await PerformOperationAsync(async () =>
                await _ozoneDataService.LoadAsync(forceOzoneDataRefresh));

            if (null != data)
            {
                OzoneData = data;
                InferAirQuality();
            }
        }
    }
}
