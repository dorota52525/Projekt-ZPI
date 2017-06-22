using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace ZpiProject.Model
{
    public class MeasurementsService
    {
        MongoClient coordinateClient;
        IMongoDatabase coordinateDatabase;
        IMongoCollection<Coordinate> coordinateCollect;

        MongoClient measurementClient;
        IMongoDatabase measurementDatabase;
        IMongoCollection<Measurement> measurementCollect; 

        public MeasurementsService()
        {
            coordinateClient = new MongoClient("mongodb://localhost:27017");
            coordinateDatabase = coordinateClient.GetDatabase("PollutionData");
            coordinateCollect = coordinateDatabase.GetCollection<Coordinate>("Coordinate");

            measurementClient = new MongoClient("mongodb://localhost:27017");
            measurementDatabase = measurementClient.GetDatabase("PollutionData");
            measurementCollect = measurementDatabase.GetCollection<Measurement>("Measurements");
        }

        //do wykresow - filtracja
        //slownik<pierwiastek, lista dat i wartosci  dla tego pierwiastka>
        public Dictionary<string, List<ChartPoint>> GetChartMeasurements(
            DateTimeOffset startDate, 
            DateTimeOffset endDate, 
            int stationId, 
            List<string> filterElements)
        {
            var dictMeasurements = measurementCollect
                .Find(m => m.StationId == stationId && m.Date > startDate && m.Date < endDate)
                .ToList()
                .SelectMany(m => new[]
                {
                    new {Date = m.Date, Value = m.Values.SO2, Element = "SO2"},
                    new {Date = m.Date, Value = m.Values.C6H6, Element = "C6H6"},
                    new {Date = m.Date, Value = m.Values.PM10, Element = "PM10"},
                    new {Date = m.Date, Value = m.Values.NO2, Element = "NO2"},
                    new {Date = m.Date, Value = m.Values.CO, Element = "CO"},
                    new {Date = m.Date, Value = m.Values.PM25, Element = "PM2.5"},
                    new {Date = m.Date, Value = m.Values.O3, Element = "O3"}

                })
                .Where(x => x.Value != null && filterElements.Contains(x.Element))
                .GroupBy(x => x.Element, x => new ChartPoint { Date = x.Date, Value = x.Value.Value })
                .ToDictionary((x => x.Key), (x => x.ToList()));

            return dictMeasurements;
        }

        //do tabel
        public List<TableMeasurement> GetMeasurements(
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            int stationId,
            List<string> filterElements)
        {
            var result = measurementCollect
                .Find(m => m.StationId == stationId && m.Date > startDate && m.Date < endDate)
                .ToList()
                .GroupBy(m => new DateTimeOffset(m.Date.Year, m.Date.Month, m.Date.Day, m.Date.Hour, 0, 0, m.Date.Offset))
                .Select(g => new TableMeasurement {
                    Date = g.Key,
                    AqIndex = g.Select(m => m.AqIndex).Average(),
                    C6H6 = g.Any(m => m.Values.C6H6 != null) ? g.Select(m => m.Values.C6H6).Average() : null,
                    PM10 = g.Any(m => m.Values.PM10 != null) ? g.Select(m => m.Values.PM10).Average() : null,
                    SO2 = g.Any(m => m.Values.SO2 != null) ? g.Select(m => m.Values.SO2).Average() : null,
                    NO2 = g.Any(m => m.Values.NO2 != null) ? g.Select(m => m.Values.NO2).Average() : null,
                    CO = g.Any(m => m.Values.CO != null) ? g.Select(m => m.Values.CO).Average() : null,
                    PM25 = g.Any(m => m.Values.PM25 != null) ? g.Select(m => m.Values.PM25).Average() : null,
                    O3 = g.Any(m => m.Values.O3 != null) ? g.Select(m => m.Values.O3).Average() : null,
                })
                .ToList();

            return result ;
        }

        //bylo do table - wyciagniecie wszystkich pomairow
        public IEnumerable<IGrouping<int, Measurement>> GetAllMeasurements()
        {
            return measurementCollect.Find(_ => true).ToList().GroupBy(m => m.StationId);
        }

        //do map
        public List<Measurement> GetAllLastMeasurements()
        {
            List<Measurement> listMeasurements = new List<Measurement>();
            var listStationIds = coordinateCollect.Find(_ => true).Project(x => x.StationId).ToList();


            foreach (var stationId in listStationIds)
            {
                var filter = Builders<Measurement>.Filter.Eq("StationId", stationId);
                //zmienic nazwe
                var a = measurementCollect.Find(filter).SortBy(x => x._id).ToList().LastOrDefault();
                if (a == null)
                {
                    continue;
                }
                listMeasurements.Add(a);
            }
            return listMeasurements;
        }

        //do map
        public Dictionary<int, Coordinate> GetCoordinates()
        {
            return coordinateCollect.Find(_ => true).ToList().ToDictionary(c => c.StationId);
        }

        //do zapisywania
        private Measurement GetLastMeasurement(int stationId)
        {
            var filter = Builders<Measurement>.Filter.Eq("StationId", stationId);
            var listMeasurements = measurementCollect.Find(filter).SortBy(x => x._id).ToList();

            return listMeasurements.LastOrDefault();
        }

        //do zapisywania
        private bool ShouldSaveMeasurement(Measurement measurement, Measurement lastMeasurement)
        {
            return measurement != lastMeasurement || lastMeasurement.Date.AddHours(1) < measurement.Date;
        }

        //do zapisywania
        public void Save(MeasurementRaw measurementRaw)
        {
            var measurement = CreateMeasurement(measurementRaw);
            var lastMeasurement = GetLastMeasurement(measurement.StationId);

            if (ShouldSaveMeasurement(measurement, lastMeasurement))
            {
                AddMeasurementToDatabase(measurement);
            }
        }

        //do zapisywania
        private void AddMeasurementToDatabase(Measurement measurement)
        {
            measurementCollect.InsertOne(measurement);
        }

        //do zapisywania
        private Measurement CreateMeasurement(MeasurementRaw measurementRaw)
        {
            var measurement = new Measurement();
            measurement.StationId = measurementRaw.StationId;
            measurement.StationName = measurementRaw.StationName;
            measurement.AqIndex = measurementRaw.AqIndex;
            measurement.Values = measurementRaw.Values;
            measurement.Date = DateTime.Now;
            return measurement;
        }

    }
}
