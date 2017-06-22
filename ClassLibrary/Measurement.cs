using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace ClassLibrary
{
    public class Measurement
    {
        public ObjectId _id { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; }
        public int AqIndex { get; set; }
        public DateTimeOffset Date { get; set; }
        public ValuesAir Values { get; set; }

        protected bool Equals(Measurement other)
        {
            return StationId == other.StationId && string.Equals(StationName, other.StationName) && AqIndex == other.AqIndex && Equals(Values, other.Values);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Measurement) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StationId;
                hashCode = (hashCode * 397) ^ (StationName != null ? StationName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ AqIndex;
                hashCode = (hashCode * 397) ^ (Values != null ? Values.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Measurement left, Measurement right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Measurement left, Measurement right)
        {
            return !Equals(left, right);
        }
    }
}