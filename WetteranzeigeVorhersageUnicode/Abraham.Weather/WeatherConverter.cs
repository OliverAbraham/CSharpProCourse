using HtmlAgilityPack;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Abraham.Weather
{
	public class WeatherConverter
    {
        #region ------------- Methods -------------------------------------------------------------

        public List<Forecast> ExtractWeatherDataFromPage(string inhalt)
        {
            var results = new List<Forecast>();

            var weather = ExtractWeatherData(inhalt);

            for (int i=0; i<weather.HoursArray.GetLength(0); i++)
            {
                if (weather.HoursArray[i] == null ||
                    weather.IconsArray[i] == null ||
                    weather.TempsArray[i] == null)
                    break;

                var forecast = new Forecast();
                forecast.Hour = ConvertWeatherTime(weather.HoursArray[i]);
                forecast.Temp = ConvertTemperature(weather.TempsArray[i]);
                
                (forecast.Icon, 
                 forecast.IconDescription,
                 forecast.WeatherDescription) = ConvertWeatherIcon(weather.IconsArray[i]);

                results.Add(forecast);
            }
            return results;
        }

        public string ConvertIconToUnicode(Forecast.WeatherIcon icon)
        {
            switch (icon)
            {
                case Forecast.WeatherIcon.Cloud:               return char.ConvertFromUtf32(0x2601);
                case Forecast.WeatherIcon.CloudWithLightning:  return char.ConvertFromUtf32(0x26C8);
                case Forecast.WeatherIcon.CloudWithRain:       return char.ConvertFromUtf32(0x2614); // 0x1F327);
                case Forecast.WeatherIcon.CloudWithSnow:       return char.ConvertFromUtf32(0x2603); // 0x1F328);
                case Forecast.WeatherIcon.MediumCloud:         return char.ConvertFromUtf32(0x26C5);
                case Forecast.WeatherIcon.SmallCloud:          return char.ConvertFromUtf32(0x26C5); // 0x1F324);
                case Forecast.WeatherIcon.Sun:                 return char.ConvertFromUtf32(0x2600);
                case Forecast.WeatherIcon.SunCloudRain:        return char.ConvertFromUtf32(0x26C5); // 0x1F326);
                case Forecast.WeatherIcon.ThunderCloudAndRain: return char.ConvertFromUtf32(0x26C8);
                case Forecast.WeatherIcon.Moon:                return char.ConvertFromUtf32(0x263D); // 0x1F319);
                case Forecast.WeatherIcon.Snow:                return char.ConvertFromUtf32(0x2603);
                case Forecast.WeatherIcon.Fog:                 return char.ConvertFromUtf32(0x2601); // 0x1F32B);
                case Forecast.WeatherIcon.Unknown: default:    return char.ConvertFromUtf32(0x26C4);                
            }
        }

        public double FindTemperatureForTime(List<Forecast> entries, DateTime time)
        {
            return FindEntry(entries, time).Temp;
        }

        public Forecast.WeatherIcon FindIconForTime(List<Forecast> entries, DateTime time)
        {
            return FindEntry(entries, time).Icon;
        }

        public Forecast FindEntry(List<Forecast> entries, DateTime time)
        {
            var entry = (from e in entries where e.Hour.Hour == time.Hour select e).FirstOrDefault();
            if (entry == null)
                throw new Exception("time not found in weather data");
            return entry;
        }
        #endregion



        #region ------------- Implementation ------------------------------------------------------

        private HtmlParts ExtractWeatherData(string inhalt)
        {
            var Doc = new HtmlDocument();
            Doc.LoadHtml(inhalt);

            var results = new HtmlParts();
            results.HoursArray = new string[29];
            results.IconsArray = new string[29];
            results.TempsArray = new string[29];
            var hoursCounter = 0;
            var iconsCounter = 0;
            var tempsCounter = 0;

            var Body = Doc.DocumentNode.SelectSingleNode("//body");
            if (Body != null)
            {
                // XML Selector to find the div we need
                //var DayBoxes1 = Body.SelectNodes($".//div[contains(@class, 'box-slider__container')]"); 
                //var DayBoxes2 = Body.SelectNodes(".//div[@class='box-slider__container']");
                var DayBoxes = Body.SelectNodes(".//div[@class='weather-daysegment-table']");

                foreach (var box in DayBoxes)
                {
                    var Hours = box.SelectNodes(".//h4[@class='weather-daysegment-table__hour']");
                    var Icons = box.SelectNodes(".//div[@class='weather-icon weather-daysegment-table__icon']");
                    var Temps = box.SelectNodes(".//main[@class='weather-daysegment-table__main']");

                    var hours = ConvertInnerHtml(Hours);
                    var icons = ConvertInnerHtml(Icons);
                    var temps = ConvertInnerHtml(Temps);
                    hours.CopyTo(results.HoursArray, hoursCounter);
                    icons.CopyTo(results.IconsArray, iconsCounter);
                    temps.CopyTo(results.TempsArray, tempsCounter);
                    hoursCounter += hours.Length;
                    iconsCounter += icons.Length;
                    tempsCounter += temps.Length;
                }
            }
            return results;
        }

        private string[] ConvertInnerHtml(HtmlNodeCollection nodes)
        {
            var TextArray = new string[nodes.Count];
            int i = 0;
            foreach (var node in nodes)
                TextArray[i++] = (!string.IsNullOrWhiteSpace(node.InnerText)) ? node.InnerText : node.InnerHtml;
            return TextArray;
        }

        private DateTime ConvertWeatherTime(string html)
        {
            html = html.Trim(new char[] { '\n', '\r', ' ', '\t' });
            var time = DateTime.Parse(html + ":00");
            if (time.Hour >= 22)
                return time.AddHours(-22);
            else
                return time.AddHours(2);
        }

        private double ConvertTemperature(string html)
        {
            if (html == null)
                return double.NaN;

            if (html.StartsWith("&"))
                return double.NaN;

            var index = html.IndexOf("Gefühlt");
            if (index >= 0)
                html = html.Substring(0 + "Gefühlt".Length, 10);

            index = html.IndexOf("°C");
            if (index >= 0)
                html = html.Substring(0, index);

            if (Double.TryParse(html, out double value))
                return value;
            else
                return 0;
        }

        private (Forecast.WeatherIcon, string, string) ConvertWeatherIcon(string html)
        {
            var icon  = GetTag(html, "src=");
            var text  = GetTag(html, "alt=");
            var title = GetTag(html, "title=");
            
            var cIcon = ConvertImageSourceToIcon(icon, title);
            return (cIcon, text, title);
        }

        private string GetTag(string html, string tag)
        {
            if (html == null)
                return "";

            int start = html.IndexOf(tag);
            if (start >= 0)
            {
                start += tag.Length+1;
                if (start >= html.Length)
                    return "";
                int end = html.IndexOf("\"", start);
                if (end > start)
                    return html.Substring(start, end-start);
            }
            return "";
        }

        private Forecast.WeatherIcon ConvertImageSourceToIcon(string icon, string title)
        {
            icon = icon.ToLower();
            title = title.ToLower();

            if (title.Contains("klarer nachthimmel"))                   return Forecast.WeatherIcon.Moon;
            if (title.Contains("sonne geht"))                           return Forecast.WeatherIcon.Sun;
            if (title.Contains("sonne"))                                return Forecast.WeatherIcon.Sun;
            if (title.Contains("bewölkt") && title.Contains("sonne"))   return Forecast.WeatherIcon.SmallCloud;
            if (title.Contains("meistens bewölkt"))                     return Forecast.WeatherIcon.MediumCloud;
            if (title.Contains("bewölkt"))                              return Forecast.WeatherIcon.MediumCloud;
            if (title.Contains("nebel"))                                return Forecast.WeatherIcon.Fog;
            if (title.Contains("regen"))                                return Forecast.WeatherIcon.CloudWithRain;
            if (title.Contains("regnerisch"))                           return Forecast.WeatherIcon.CloudWithRain;
            if (title.Contains("gewitter"))                             return Forecast.WeatherIcon.ThunderCloudAndRain;
            if (title.Contains("sturm"))                                return Forecast.WeatherIcon.ThunderCloudAndRain;
            if (title.Contains("schnee"))                               return Forecast.WeatherIcon.Snow;

            // alte icons:
            if (icon.Contains("sonne_woelkchen"))                       return Forecast.WeatherIcon.SmallCloud;
            if (icon.Contains("sonne_wolke"))                           return Forecast.WeatherIcon.MediumCloud;
            if (icon.Contains("sonne_auf_unter"))                       return Forecast.WeatherIcon.Sun;
            if (icon.Contains("mond"))                                  return Forecast.WeatherIcon.Moon;
            if (icon.Contains("schnee"))                                return Forecast.WeatherIcon.Snow;
            if (icon.Contains("nebel"))                                 return Forecast.WeatherIcon.Fog;
            if (icon.Contains("wolke_regen"))                           return Forecast.WeatherIcon.CloudWithRain;
            if (icon.Contains("sonne"))                                 return Forecast.WeatherIcon.Sun;
            if (icon.Contains("wolke.38262afa.svg"))                    return Forecast.WeatherIcon.Cloud;
            if (icon.Contains("wolke.graupel"))                         return Forecast.WeatherIcon.Cloud;
            if (icon.Contains("wolke"))                                 return Forecast.WeatherIcon.MediumCloud;
            
            File.AppendAllText("WeatherConverter-problems.log", $"{DateTime.Now} - Cannot convert icon: {icon} alt text: {title}\n");
            Console.WriteLine($"{DateTime.Now} - Cannot convert icon: {icon} alt text: {title}\n");
            return Forecast.WeatherIcon.Unknown;
        }
        #endregion
    }
}
