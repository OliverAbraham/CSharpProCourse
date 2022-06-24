using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Drawing;

namespace ChromeDriverDemo
{
    class Program
    {
		#region ------------- Types and constants -------------------------------------------------
		private static readonly string _url = "https://www.wetter.de/deutschland/wetter-lentfoehrden-18219233/wetterbericht-aktuell.html";
		private static readonly int _connectTimeoutInSeconds = 30;
		#endregion


		static void Main(string[] args)
        {
            Console.WriteLine("Chrome driver demo - remote controlling chrome web browser");

			IWebDriver driver = new ChromeDriver();
			driver.Manage().Window.Position = new Point(-2000, 100);
			driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, _connectTimeoutInSeconds);
			driver.Navigate().GoToUrl(_url);
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(_connectTimeoutInSeconds));
			wait.Until(drv => drv.Title.Contains("Wetter"));

			var contents = (driver as ChromeDriver).PageSource;
            Console.WriteLine(contents);

			driver.Close();
			driver.Quit();
		}
    }
}
