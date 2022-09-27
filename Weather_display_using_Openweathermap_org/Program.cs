using Newtonsoft.Json;
using RestSharp;

namespace Weather_display_using_Openweathermap_org
{
    internal class Program
    {
        private static string apiKey = File.ReadAllText(@"C:\Cloud\Openweathermap-ApiKey.txt");

        static void Main()
        {
            Console.WriteLine("Weather display using the API of Openweathermap.org");

            GetCurrentWeather();

            // Uncomment this to make periodic calls:
            //var scheduler = new Abraham.Scheduler.Scheduler()
            //    .UseIntervalMinutes(1)
            //    .UseAction( ()=> GetCurrentWeather() )
            //    .Start();
            //
            //Console.WriteLine("Press any key to end");
            //Console.ReadKey();
            //scheduler.Stop();                
        }

        private static void GetCurrentWeather()
        {
            var httpClient = new RestClient();
            var request = new RestRequest($"https://api.openweathermap.org/data/3.0/onecall?lat=53.8667&lon=9.8833&lang=de&units=metric&appid={apiKey}", Method.Get);
            var response = httpClient.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"Request wasn't successful.\nMore info:{response.StatusDescription}");
                return;
            }

            var myWeatherData = JsonConvert.DeserializeObject<Root>(response.Content);
            Console.WriteLine($"Current temperature: {myWeatherData.current.temp} °C");
        }
    }
}