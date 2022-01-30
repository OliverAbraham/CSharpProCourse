using System;

namespace WeatherDisplay
{
	class Program
	{
		#region ------------- Fields --------------------------------------------------------------
		static string _url = "https://www.wetter.de/deutschland/wetter-lentfoehrden-18219233.html?q=Lentf%C3%B6hrden%2C%2024632%2C%20Schleswig-Holstein";
		#endregion



		#region ------------- Init ----------------------------------------------------------------
		static void Main(string[] args)
		{
			var page = DownloadPage(_url);
			var temperature = ExtractTemperatureFromPage(page);
			PrintTemperature(temperature);
		}
		#endregion



		#region ------------- Implementation ------------------------------------------------------
		private static string DownloadPage(string url)
		{
			var client = new SimpleHttpClient();
			var page = client.DownloadString(url);
			return page;
		}

		private static string ExtractTemperatureFromPage(string page)
		{
			var _startToken = "<div class=\"weather-daybox-base__minMax__max\">";
			var _endToken   = "</div>";
			var temperature = "---";

			int startPosition = page.IndexOf(_startToken) + _startToken.Length;
			if (startPosition > -1)
			{
				temperature = page.Substring(startPosition);
				int PosEnde = temperature.IndexOf(_endToken);
				if (PosEnde > -1)
					temperature = temperature.Substring(0, PosEnde);
				temperature = temperature.Replace("\n", "");
				temperature = temperature.Trim();
				temperature = temperature.Replace("&deg;", "°").Replace("&#xB0;", "°");
			}

			return temperature;
		}

		private static void PrintTemperature(string temperature)
		{
			Console.WriteLine("The temperature in my town is: {0}", temperature);
			Console.WriteLine("Press any key to end");
			Console.ReadKey();
		}
		#endregion
	}
}
