using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Q42.WinRT;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AirQualityInfo
{
    public sealed partial class AboutControl : UserControl
    {
        public AboutControl()
        {
            this.InitializeComponent();

            VersionInformationTextBlock.Text = "Version " + Util.GetAppVersion();
        }

        private void Support_OnClick(object sender, RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri(Constants.SupportUrl));
        }

        private void PrivacyStatement_OnClick(object sender, RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri(Constants.PrivacyPolicyUrl));
        }
    }
}
