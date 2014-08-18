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

        public static SortByOption GetDefaultSort()
        {
            return new SortByOption(SortByOptionEnum.Alpha, "Alphabetisch");
        }

        public static List<SortByOption> GetSortings()
        {
            var list = new List<SortByOption>();

            list.Add(GetDefaultSort());
            list.Add(new SortByOption(SortByOptionEnum.Distance, "Distanz"));
            list.Add(new SortByOption(SortByOptionEnum.OneHourAverage, "Ein-Stunden Durchschnitt"));

            return list;
        }
    }
}
