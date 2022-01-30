using Abraham.Http;
using Abraham.String;
using Abraham.Weather;
using System;
using System.Windows;
using System.Windows.Threading;

namespace HtmlProcessingWithHtmlAgilityPack
{
	/// <summary>
	/// View model for the weather display
	/// </summary>
	public partial class MainWindow : Window
	{
		#region ------------- Fields --------------------------------------------------------------
		private static string   _url  = "https://www.wetter.de/deutschland/wetter-lentfoehrden-18219233.html?q=Lentf%C3%B6hrden%2C%2024632%2C%20Schleswig-Holstein";
        private static string   _town = "Lentfoehrden";
        private DispatcherTimer _timer;
        private const int SECONDS = 1000 * 10000;
		#endregion



		#region ------------- Init ----------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();
            StartTimer();
        }
		#endregion



		#region ------------- Implementation ------------------------------------------------------
        private void StartTimer()
        {
            _timer = new DispatcherTimer(new TimeSpan(1 * SECONDS),    // At the beginning after a second
                                        DispatcherPriority.Normal,
                                        new EventHandler(TimerElapsed),
                                        this.Dispatcher);
            _timer.Start();
        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            LoadWeather();
            _timer.Interval = new TimeSpan(30 * SECONDS);  // then every 30 seconds
        }

        private void LoadWeather()
		{
			var page = LoadPage();
			var temperature = ExtractTemperatureFromPage(page);
			Display(temperature);
		}

		private static string LoadPage()
		{
			var client = new SimpleHttpClient();
			string page = client.DownloadString(_url);
			return page;
		}

		private string ExtractTemperatureFromPage(string page)
        {
			var weatherLogic = new WeatherLogic();
			var temperature = weatherLogic.ExtractTemperatureFromPage(page);
            return temperature;
        }

		private void Display(string temperature)
		{
			Town.Content = $"The weather in { _town}:";
			Temperature.Content = temperature;
		}
		#endregion
    }
}
