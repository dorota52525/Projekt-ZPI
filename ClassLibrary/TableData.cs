using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public class TableData
    {
        public Dictionary<int, Coordinate> StationDates { get; set; }
        public List<TableMeasurement> HistoricalData { get; set; }

        public TableData()
        {
            StationDates = new Dictionary<int, Coordinate>();
            HistoricalData = new List<TableMeasurement>();
        }
    }

}
