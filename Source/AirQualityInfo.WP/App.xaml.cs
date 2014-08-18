using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using AirQualityInfo.DataClient;
using AirQualityInfo.DataClient.Mocks;
using AirQualityInfo.WP.Common;
using AirQualityInfo.WP.Services;
using AirQualityInfo.WP.ViewModels;
using AirQualityInfo.WP.Views;
using Caliburn.Micro;

namespace AirQualityInfo.WP
{
     public sealed partial class App
    {
        private WinRTContainer container;
        private INavigationService navigationService;

        public App()
        {
            InitializeComponent();
        }

        protected override void Configure()
        {
            LogManager.GetLog = t => new DebugLog(t);

            container = new WinRTContainer();
            container.RegisterWinRTServices();

            //container.RegisterInstance(typeof(IFavoritesRepository), null, new DefaultFavoritesRepository());
            //container.RegisterPerRequest(typeof(IMessageService), null, typeof(DefaultMessageService));

            container.RegisterPerRequest(typeof(IHttpClient), null, typeof(MockHttpClient));
            container.RegisterPerRequest(typeof(IOzoneDataService), null, typeof(OzoneDataService));
            container.RegisterPerRequest(typeof(ILocationService), null, typeof(LocationService));

            container
                .PerRequest<MainViewModel>()
                .PerRequest<StationDetailsViewModel>()
                .PerRequest<AboutViewModel>(); 
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            navigationService = container.RegisterNavigationService(rootFrame);
            SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = container.GetInstance(service, key);
            if (instance != null)
                return instance;
            throw new Exception("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Initialize();

            PrepareViewFirst();

            var resumed = false;

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                resumed = navigationService.ResumeState();

                // Restore the saved session state only when appropriate.
                try
                {
                    await SuspensionManager.RestoreAsync();
                }
                catch (SuspensionManagerException)
                {
                    // Something went wrong restoring state.
                    // Assume there is no state and continue.
                }
            }

            if (!resumed)
                DisplayRootView<MainView>();
        }

        protected async override void OnSuspending(object sender, SuspendingEventArgs e)
        {
            navigationService.SuspendState();

            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}