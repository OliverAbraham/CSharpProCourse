using Newtonsoft.Json;
using REST_client_with_JSON_result;
using RestSharp;
using RestSharp.Authenticators;
using System.Text;

// Note: Be sure that your user account has WebAPI access rights to this Wordpress site!
// Be sure your wordpress site has Basic authentication plugin activated

Console.WriteLine("Doing a REST call to an endpoint that returns JSON, with authentication");

var client = new RestClient();
//var request = new RestRequest("https://www.lentfoehrden.de/wp-json", Method.Get);
var request = new RestRequest("https://www.lentfoehrden.de/wp-json/wp/v2/posts", Method.Get);

// Adding authentication data. If this is failing, we will get StatusCode 401 "unauthorized" as response
var username = "USERNAME";
var password = "PASSWORD";
client.Authenticator = new HttpBasicAuthenticator(username, password);
var test = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));


RestResponse response = await client.ExecuteGetAsync(request);
var content = response.Content; // raw content as string

Console.WriteLine($"Response was             {response.StatusDescription}");
Console.WriteLine($"Response has ContentType {response.ContentType}");
Console.WriteLine($"Response has Encoding    {response.ContentEncoding}");


// NOTE: To deserialize the json you've got, you need the corresponsing C# classes.
// to generate them, you can use the website https://json2csharp.com/
// post the JSON from this request and and the website will generate classes for you

if (response.StatusCode != System.Net.HttpStatusCode.OK)
    return;

var myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(response.Content);

Console.WriteLine($"\n\n\nPosts");
foreach(var post in myDeserializedClass)
    Console.WriteLine($"{post.slug,-100} {post.link}");
