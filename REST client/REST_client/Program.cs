using RestSharp;

Console.WriteLine("Doing simple REST calls with RestSharp nuget package");

var client = new RestClient("http://lentfoehrden.de");
// client.Authenticator = new HttpBasicAuthenticator(username, password);

var request = new RestRequest("/wp-json/wp/v2/pages/71", Method.Get);
//request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
//request.AddUrlSegment("id", "123"); // replaces matching token in request.Resource

// easily add HTTP Headers
//request.AddHeader("header", "value");

// add files to upload (works with compatible verbs)
//request.AddFile(path);

//// execute the request
//IRestResponse response = client.Execute(request);
//var content = response.Content; // raw content as string

// deserialize result
// return content type is sniffed but can be explicitly set via RestClient.AddHandler();
//IRestResponse<WordpressPageResponse> response2 = client.Execute<WordpressPageResponse>(request);
//var ContentAsString = response2.Data.content;
//
//// Convert to data fields
//var DateCollection = AlexaDate.DeserializeContent(ContentAsString.rendered);
//
//foreach (var Date in DateCollection)
//    Console.WriteLine($"Date: {Date.Datum} Description: {Date.Terminbezeichnung}");

//// easy async support
//client.ExecuteAsync(request, response => {
//    Console.WriteLine(response.Content);
//});

//// async with deserialization
//var asyncHandle = client.ExecuteAsync<Person>(request, response => {
//    Console.WriteLine(response.Data.Name);
//});

//// abort the request on demand
//asyncHandle.Abort();
