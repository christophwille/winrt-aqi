﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirQualityInfo.DataClient.Models;

namespace AirQualityInfo.WP.ViewModels
{
    public interface IMainViewModel
    {
        bool UpdateInProgress { get; set; }
        GeoCoordinate CurrentLocation { get; set; }
        ObservableCollection<OzoneInformation> Stations { get; set; }
        string FilterDisplay { get; set; }
    }
}