using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirQualityInfo.DataClient;
using AirQualityInfo.DataClient.Models;

namespace AirQualityInfo.WP.ViewModels
{
    public interface IMainViewModel
    {
        DataAggregate Aggregate { get; set; }
        string MesswerteHeader { get; set; }
    }
}
