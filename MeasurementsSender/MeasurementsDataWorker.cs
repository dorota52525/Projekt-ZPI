using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary;
using Newtonsoft.Json;

namespace MeasurementsSender
{
    class MeasurementsDataWorker
    {
        HttpClient client = new HttpClient();

        public MeasurementsDataWorker()
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task SendMeasurements()
        {
            var timeWait = 10 * 60 * 1000;

            while (true)
            {
                try
                {
                    HttpResponseMessage measurementResponse = await client.GetAsync("http://powietrze.gios.gov.pl/pjp/current/getAQIDetailsList?param=AQI");
                    measurementResponse.EnsureSuccessStatusCode();
                    string responseBody = await measurementResponse.Content.ReadAsStringAsync();
                    var measurements = JsonConvert.DeserializeObject<List<MeasurementRaw>>(responseBody);

                   

                    foreach (var measurement in measurements)
                    {
                        var measurementJson = JsonConvert.SerializeObject(measurement);
                        var queryString = new StringContent(measurementJson, Encoding.UTF8, "application/json");

                        var saveResponse = await client.PostAsync("http://localhost:34470/save", queryString);
                        saveResponse.EnsureSuccessStatusCode();
                    }
                    Console.WriteLine("Zakonczono wysylanie");
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }

                Thread.Sleep(timeWait);
            }
        }
    }
}
