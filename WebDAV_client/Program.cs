using Newtonsoft.Json;
using System.Net;
using System.Text;
using WebDav;

namespace WebDAV_client
{
    internal class Program
    {
        public static IWebDavClient _client;

        static void Main(string[] args)
        {
            Console.WriteLine("Loading and saving a file from/to nextcloud using the WebDAV protocol.");

            Demo().GetAwaiter().GetResult();;
        }

        private static async Task Demo()
        {
            // note: Create a json file with a content like this:
            // {"UserName":"my-username","Password":"my-password"}

            // Load username and password from a text file, to connect to your WebDAV server
            var serializedCredentials = File.ReadAllText(@"C:\Credentials\CSharpProCourse-WebDavClient.json");
            var credentials = (MyCredentials)JsonConvert.DeserializeObject<MyCredentials>(serializedCredentials);


            // set up the client
            var clientParams = new WebDavClientParams
            {
                BaseAddress = new Uri("http://www.abraham-beratung.de/nextcloud/remote.php/dav"),
                Credentials = new NetworkCredential(credentials.UserName, credentials.Password)
            };
            _client = new WebDavClient(clientParams);


            // check if a certain file exists
            var result = await _client.Propfind("files/IT/WebDAV_Client/Demo.txt");
            if (!result.IsSuccessful)
            {
                Console.WriteLine("The file Demo.txt could not be found.");
                return;
            }


            // read the file
            var response = await _client.GetRawFile("files/IT/WebDAV_Client/Demo.txt");
            if (!result.IsSuccessful)
            {
                Console.WriteLine("The file Demo.txt could not be found.");
                return;
            }
            var reader = new StreamReader(response.Stream, Encoding.UTF8, detectEncodingFromByteOrderMarks:true);
            var content = reader.ReadToEnd();
            Console.WriteLine($"Loaded file from nextcloud server. File content is: {content}");


            // just add some text
            Console.WriteLine($"Adding some text");
            content = content + " moretext!";


            // and save back the file
            Console.WriteLine($"Saving to nextcloud...");
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(content);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            var saveResponse = await _client.PutFile("files/IT/WebDAV_Client/Demo.txt", stream, new PutFileParameters());
            if (!saveResponse.IsSuccessful)
            {
                Console.WriteLine("The file Demo.txt could not be saved.");
                return;
            }
            Console.WriteLine($"Saved content '{content}'");
        }
    }

    public class MyCredentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}