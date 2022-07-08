using Abraham.Internet;
using Abraham.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WetteranzeigeCore
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
            Starte_Timer();
		}

        DispatcherTimer MeinTimer;
        string Homepage              = "https://www.wetter.de/deutschland/wetter-lentfoehrden-18219233.html";
        const string Ort             = "Lentföhrden";
        const string Ortsangabe      = "Das Wetter in {0}:";

        void Starte_Timer()
        {
            MeinTimer = new DispatcherTimer(new TimeSpan(1 * 1000 * 10000),    // in einer Sekunde
                                            DispatcherPriority.Normal,
                                            new EventHandler(MeinTimer_Elapsed),
                                            this.Dispatcher);
            MeinTimer.Start();
        }

        void MeinTimer_Elapsed(object sender, EventArgs e)
        {
            Update_Wetter();
            MeinTimer.Interval = new TimeSpan(30 * 1000 * 10000);  // wieder in 30 Sekunden
        }

        void Update_Wetter()
        {
            var httpClient = new HttpClient ();
            string pageContent = httpClient.DownloadFromUrl(Homepage);
            
            var converter = new WeatherConverter();
            var weatherData = converter.ExtractWeatherDataFromPage(pageContent);

            double Temperature = converter.FindTemperatureForTime(weatherData, DateTime.Now);
            
            Ortsname  .Content = String.Format(Ortsangabe, Ort);
            Temperatur.Content = $"{Temperature} °";

            col1.Content = converter.ConvertIconToUnicode(converter.FindIconForTime(weatherData, new DateTime(2000,1,1, 7,0,0)));
            col2.Content = converter.ConvertIconToUnicode(converter.FindIconForTime(weatherData, new DateTime(2000,1,1,13,0,0)));
            col3.Content = converter.ConvertIconToUnicode(converter.FindIconForTime(weatherData, new DateTime(2000,1,1,19,0,0)));
            col4.Content = converter.ConvertIconToUnicode(converter.FindIconForTime(weatherData, new DateTime(2000,1,1,23,0,0)));

            temp1.Content = converter.FindTemperatureForTime(weatherData, new DateTime(2000,1,1, 7,0,0)) + " °";
            temp2.Content = converter.FindTemperatureForTime(weatherData, new DateTime(2000,1,1,13,0,0)) + " °";
            temp3.Content = converter.FindTemperatureForTime(weatherData, new DateTime(2000,1,1,19,0,0)) + " °";
            temp4.Content = converter.FindTemperatureForTime(weatherData, new DateTime(2000,1,1,23,0,0)) + " °";
        }
	}
}
