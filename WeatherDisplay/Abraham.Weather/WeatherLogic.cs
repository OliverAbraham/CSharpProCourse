using HtmlAgilityPack;

namespace Abraham.Weather
{
	public class WeatherLogic
    {
        public string ExtractTemperatureFromPage(string page)
        {
            var Doc = new HtmlDocument();
            Doc.LoadHtml(page);

            var body = Doc.DocumentNode.SelectSingleNode("//body");
            if (body != null)
            {
                // XML Selector to find the div we need
                // Documentation for this: https://html-agility-pack.net/documentation

                var divs = body.SelectNodes($".//div[contains(@class, 'weather-daybox-base__minMax__max')]");
                if (divs != null && divs.Count > 0)
                {
                    var temperature = divs[0].InnerHtml;
				    temperature = temperature.Trim('\n').Trim();
                    return temperature + " °";
                }
            }
            return "---";
        }
    }
}
