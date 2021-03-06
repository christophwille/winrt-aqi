﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AirQualityInfo.WP.Common
{
    public class StateEnabledPage : Page
    {
        private readonly StateHelper _stateHelper;

        public StateEnabledPage()
        {
            this._stateHelper = new StateHelper(this);
            this._stateHelper.LoadState += this.StateHelper_LoadState;
            this._stateHelper.SaveState += this.StateHelper_SaveState;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this._stateHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this._stateHelper.OnNavigatedFrom(e);
        }

        private void StateHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var vm = DataContext as IStateEnabledViewModel;

            if (null != vm)
            {
                vm.LoadState(e.PageState);
            }
        }

        private void StateHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            var vm = DataContext as IStateEnabledViewModel;

            if (null != vm)
            {
                vm.SaveState(e.PageState);
            }
        }
    }
}
