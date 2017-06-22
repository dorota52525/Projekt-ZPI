using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using ZpiProject.Model;

namespace ZpiProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowMap()
        {
            var measurementsService = new MeasurementsService();
            var mapData = new MapData();
            mapData.Measurements = measurementsService.GetAllLastMeasurements();
            mapData.Coordinates = measurementsService.GetCoordinates();         

            return View(mapData);
        }

        [HttpPost]
        [Route("/save")]
        public void Save([FromBody] MeasurementRaw measurementRaw)
        {
            var measurementsService = new MeasurementsService();
            measurementsService.Save(measurementRaw);
        }


        public IActionResult ShowFilterView()
        {
            return View();
        }

        public IActionResult ShowChart(DateTimeOffset? startDate,
            DateTimeOffset? endDate,
            int? stationId,
            List<string> filterElements)
        {

            var measurementsService = new MeasurementsService();
            var chartData = new ChartData();
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            ViewBag.StationId = stationId;
            ViewBag.FilterElements = filterElements;
          


            if (stationId == null)
            {
                chartData.StationDates = measurementsService.GetCoordinates();
            }
            else
            {
                chartData.HistoricalData = measurementsService.GetChartMeasurements(startDate.Value, endDate.Value, stationId.Value, filterElements);
                chartData.StationDates = measurementsService.GetCoordinates();
                
            }
            return View(chartData);
        }

        public IActionResult ShowTable(DateTimeOffset? startDate,
            DateTimeOffset? endDate,
            int? stationId,
            List<string> filterElements)
        {
            var measurementsService = new MeasurementsService();
            var tableData = new TableData();
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            ViewBag.StationId = stationId;
            ViewBag.FilterElements = filterElements;

            if (stationId == null)
            {
                tableData.StationDates = measurementsService.GetCoordinates();
            }
            else
            {
                tableData.HistoricalData = measurementsService.GetMeasurements(startDate.Value, endDate.Value, stationId.Value, filterElements);
                tableData.StationDates = measurementsService.GetCoordinates();
            }
            return View(tableData);
        }
    }
}
