using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityInfo.WP.Common
{
    public interface IStateEnabledViewModel
    {
        void LoadState(Dictionary<string, object> statePageState);
        void SaveState(Dictionary<string, object> statePageState);
    }
}
