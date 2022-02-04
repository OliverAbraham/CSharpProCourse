using AngleSharp;

Console.WriteLine("Parsing HTML with the 'AngleSharp' library");


var testfile = @"
<!DOCTYPE html>
<html lang=""de"">
<head><title>Wetter Hamburg Freie und Hansestadt - Wettervorhersage für Hamburg | wetter.de</title></head>
<body data-guj-zone=""orte"">
    <div role=""article"" winddirection=""234.8"" class=""base-box weather-daybox-base weather-daybox-plain base-box--level-0"">
    <header>
    <div class=""weather-daybox-base__shortSummary"">
        <h2 id=""tag1"" datetime=""2022-02-04"" class=""weather-daybox-base__shortSummary__date""></h2> 
        <div class=""weather-daybox-base__symbolRight"">
            <div class=""weather-daybox-base__minMax"">
                <div class=""weather-daybox-base__minMax__max"">8</div>
            </div>
        </div>
    </div>
</body>
</html>
";


//Load the string into AngleSharp using a stream
var config = Configuration.Default; // from AngleSharp
var context = BrowsingContext.New(config);
var document = await context.OpenAsync(req => req.Content(testfile));

// find and fetch the temperature
var body = document.GetElementsByTagName("body")[0];
var temperatureDivision = body.GetElementsByClassName("weather-daybox-base__minMax__max");
var temperature = temperatureDivision[0].InnerHtml;

Console.WriteLine($"The temperature is: {temperature}");

