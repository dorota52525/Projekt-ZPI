using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public class TableMeasurement
    {
        public DateTimeOffset Date { get; set; }
        public double? C6H6 { get; set; }
        public double? PM10 { get; set; }
        public double? SO2 { get; set; }
        public double? NO2 { get; set; }
        public double? CO { get; set; }
        public double? PM25 { get; set; }
        public double? O3 { get; set; }
        public double AqIndex { get; set; }
    }
}
