using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ClassLibrary
{
    public class ValuesAir
    {
        [JsonProperty(PropertyName = "C6H6")]
        public double? C6H6 { get; set; }
        [JsonProperty(PropertyName = "PM10")]
        public double? PM10 { get; set; }
        [JsonProperty(PropertyName = "SO2")]
        public double? SO2 { get; set; }
        [JsonProperty(PropertyName = "NO2")]
        public double? NO2 { get; set; }
        [JsonProperty(PropertyName = "CO")]
        public double? CO { get; set; }
        [JsonProperty(PropertyName = "PM2.5")]
        public double? PM25 { get; set; }
        [JsonProperty(PropertyName = "O3")]
        public double? O3 { get; set; }

        protected bool Equals(ValuesAir other)
        {
            return C6H6.Equals(other.C6H6) && PM10.Equals(other.PM10) && SO2.Equals(other.SO2) && NO2.Equals(other.NO2) && CO.Equals(other.CO) && PM25.Equals(other.PM25) && O3.Equals(other.O3);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ValuesAir) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = C6H6.GetHashCode();
                hashCode = (hashCode * 397) ^ PM10.GetHashCode();
                hashCode = (hashCode * 397) ^ SO2.GetHashCode();
                hashCode = (hashCode * 397) ^ NO2.GetHashCode();
                hashCode = (hashCode * 397) ^ CO.GetHashCode();
                hashCode = (hashCode * 397) ^ PM25.GetHashCode();
                hashCode = (hashCode * 397) ^ O3.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(ValuesAir left, ValuesAir right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValuesAir left, ValuesAir right)
        {
            return !Equals(left, right);
        }
    }
}
