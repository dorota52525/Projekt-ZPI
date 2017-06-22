using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public class MapData
    {
        public List<Measurement> Measurements { get; set; }
        public Dictionary<int,Coordinate> Coordinates { get; set; }
    }
}
