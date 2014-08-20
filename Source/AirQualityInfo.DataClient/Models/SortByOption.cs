using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityInfo.DataClient.Models
{
    public enum SortByOptionEnum
    {
        Alpha,
        Distance,
        OneHourAverage
    }

    public class SortByOption
    {
        public SortByOption(SortByOptionEnum sortby, string name)
        {
            SortBy = sortby;
            SortDisplayString = name;
        }

        public SortByOptionEnum SortBy { get; set; }
        public string SortDisplayString { get; set; }

        private static SortByOption _defaultSortByOption = null;
        public static SortByOption GetDefaultSort()
        {
            if (null == _defaultSortByOption)
                _defaultSortByOption = new SortByOption(SortByOptionEnum.Alpha, "Alphabetisch");

            return _defaultSortByOption;
        }

        private static List<SortByOption> _sortings = null;

        public static List<SortByOption> GetSortings()
        {
            if (null != _sortings) return _sortings;
            var list = new List<SortByOption>();

            list.Add(GetDefaultSort());
            list.Add(new SortByOption(SortByOptionEnum.Distance, "Distanz"));
            list.Add(new SortByOption(SortByOptionEnum.OneHourAverage, "Ein-Stunden Durchschnitt"));
            _sortings = list;

            return list;
        }
    }
}
