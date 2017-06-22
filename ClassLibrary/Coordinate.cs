using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace ClassLibrary
{
    public class Coordinate
    {
        public ObjectId _id { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; }
        public double Latitude { get; set; } //x
        public double Longitude { get; set; } //y
    }
}
