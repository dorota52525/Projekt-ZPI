using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary;
using RabbitMQ.Client;

namespace MeasurementsSender
{
    class Program
    {
        public static void Main(string[] args)
        {
            var measurementsDataWorker = new MeasurementsDataWorker();
            measurementsDataWorker.SendMeasurements().Wait();
        }
    }
}