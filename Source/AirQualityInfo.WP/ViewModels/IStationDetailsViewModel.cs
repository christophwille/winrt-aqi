using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirQualityInfo.DataClient.Models;

namespace AirQualityInfo.WP.ViewModels
{
    public interface IStationDetailsViewModel
    {
        OzoneInformation Station { get; set; }
    }
}
