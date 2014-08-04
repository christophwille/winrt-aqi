using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AirQualityInfo.Common;
using AirQualityInfo.DataClient.Models;
using AirQualityInfo.Models;
using AirQualityInfo.ViewModels;
using Callisto.Controls;
using GalaSoft.MvvmLight;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace AirQualityInfo
{
    public sealed partial class MainPage : AirQualityInfo.Common.LayoutAwarePage
    {
        public MainPageViewModel ViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            if (!ViewModelBase.IsInDesignModeStatic)
            {
                ViewModel = new MainPageViewModel();
                DataContext = ViewModel;
            }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            ViewModel.LoadOzoneDataAsync();
            ViewModel.LookupPositionAsync();
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void FilterBy_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var menu = new Menu();
            var states = FilterByState.GetStates();

            foreach (var state in states)
            {
                var mnuItem = new MenuItem
                                  {
                                      Text = state.StateDisplayString,
                                      Tag = state
                                  };
                
                mnuItem.Tapped += filterByClicked;
                menu.Items.Add(mnuItem);
            }

            DisplayFlyout(menu, filterByControl);
        }

        private void SortBy_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var menu = new Menu();
            var sortings = SortByOption.GetSortings();

            foreach (var sortby in sortings)
            {
                var mnuItem = new MenuItem
                {
                    Text = sortby.SortDisplayString,
                    Tag = sortby
                };

                mnuItem.Tapped += sortByClicked;
                menu.Items.Add(mnuItem);
            }

            DisplayFlyout(menu, sortByControl);
        }

        private void DisplayFlyout(Menu menu, FrameworkElement placementTarget)
        {
            var flyout = new Callisto.Controls.Flyout();
            flyout.Placement = PlacementMode.Bottom;
            flyout.HorizontalAlignment = HorizontalAlignment.Left;
            flyout.HorizontalContentAlignment = HorizontalAlignment.Left;
            flyout.PlacementTarget = placementTarget;
            flyout.Content = menu;
            flyout.IsOpen = true;
        }

        private void filterByClicked(object sender, RoutedEventArgs e)
        {
            var state = ((MenuItem)sender).Tag as FilterByState;

            ViewModel.CurrentFilter = state;
            ViewModel.DisplayMeasurements();
        }

        private void sortByClicked(object sender, RoutedEventArgs e)
        {
            var sortby = ((MenuItem)sender).Tag as SortByOption;

            ViewModel.CurrentSort = sortby;
            ViewModel.DisplayMeasurements();
        }

        private void MeasurementsGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as OzoneInformation;
            ViewModel.SelectedMeasurement = item;
        }
    }
}
