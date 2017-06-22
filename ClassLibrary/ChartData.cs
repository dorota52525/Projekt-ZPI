using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public class ChartData
    {
        public Dictionary<int, Coordinate> StationDates { get; set; }
        public Dictionary<string, List<ChartPoint>> HistoricalData { get; set; }

        public ChartData()
        {
            StationDates = new Dictionary<int, Coordinate>();
            HistoricalData = new Dictionary<string, List<ChartPoint>>();
        }
    }

}
