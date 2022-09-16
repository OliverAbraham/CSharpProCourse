using RestSharp;

Console.WriteLine("Doing a simple REST call with RestSharp nuget package.");

var client = new RestClient();
var request = new RestRequest("https://www.lentfoehrden.de/veranstaltungen/", Method.Get);

RestResponse response = await client.ExecuteGetAsync(request);

Console.WriteLine($"Response was {response.StatusDescription}");
if (response.StatusCode == System.Net.HttpStatusCode.OK)
    Console.WriteLine($"Raw HTML from website:\n{response.Content}");
