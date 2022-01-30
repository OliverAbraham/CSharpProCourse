using Abraham.Weather;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbrahamWeather_Tests
{
	[TestClass]
	public class AbrahamWeather_Tests
	{
		[TestMethod]
		public void ShouldExtractTemperatureFromHtml()
		{
			// -- Arrange -- 
			var sut = new WeatherLogic(); // sut means "System Under Test" and is a common term
			var page = 
@"
<html>
<head></head>
<body>
	<div class=""something"">someting</div>
	<div class=""weather-daybox-base__minMax__max"">    5    </div>
	<div class=""something"">someting</div>
</body></html>";

			// -- Act -- 
			var actual = sut.ExtractTemperatureFromPage(page);

			// -- Assert
			actual.Should().Be("5 °", "because we want that part of the input");
		}

		[TestMethod]
		public void Should_return_default_when_div_does_not_exist()
		{
			// -- Arrange -- 
			var sut = new WeatherLogic();
			var page =
@"
<html>
<head></head>
<body>
	<div class=""something"">someting</div>
	<div class=""something"">someting</div>
</body></html>";

			// -- Act -- 
			var actual = sut.ExtractTemperatureFromPage(page);

			// -- Assert
			actual.Should().Be("---", "because we want that temperature part of the input");
		}
	}
}