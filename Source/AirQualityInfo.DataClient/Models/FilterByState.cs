using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityInfo.DataClient.Models
{
    public class FilterByState
    {
        public FilterByState(string id, string name)
        {
            Id = id;
            StateDisplayString = name;
        }

        public string Id { get; set; }
        public string StateDisplayString { get; set; }

        private static FilterByState _defaultFilter = null;
        public static FilterByState GetDefaultFilter()
        {
            if (null == _defaultFilter) 
                _defaultFilter = new FilterByState("", "Alle Bundesländer");

            return _defaultFilter;
        }

        public static List<FilterByState> GetStates()
        {
            var list = new List<FilterByState>();

            list.Add(GetDefaultFilter());
            list.Add(new FilterByState("W", "Wien"));
            list.Add(new FilterByState("B", "Burgenland"));
            list.Add(new FilterByState("NÖ", "Niederösterreich"));
            list.Add(new FilterByState("OÖ", "Oberösterreich"));
            list.Add(new FilterByState("S", "Salzburg"));
            list.Add(new FilterByState("ST", "Steiermark"));
            list.Add(new FilterByState("K", "Kärnten"));
            list.Add(new FilterByState("T", "Tirol"));
            list.Add(new FilterByState("V", "Vorarlberg"));

            return list;
        }
    }
}
