using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public class MeasurementRaw
    {
        public int StationId { get; set; }
        public string StationName { get; set; }
        public int AqIndex { get; set; }
        public ValuesAir Values { get; set; }
    }
}
