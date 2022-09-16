using RestSharp;

namespace PushbulletSender
{




	// IMPORTANT:
	// add nuget package "RestSharp" to your project!




	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Sending a message to my Pushbullet app on my phone");

			SendCommandToPushbulletDevice(
				APIKey: "___ENTER___YOUR___DEVICE___KEY___HERE___", 
				Device: "ujEnGWlP2MmsjAiVsKnSTs", 
				Title : "Hey", 
				Body  : "I've got something to say!");

			Console.WriteLine("Message was sent.");
		}

        private static void SendCommandToPushbulletDevice(string APIKey, string Device, string Title, string Body)
        {
            string URL = "https://api.pushbullet.com/v2/pushes";

            // Build HTTP POST command
            MyMessage body = new MyMessage()
			{
                type        = "note",
                title       = Title,
                body        = Body,
                device_iden = Device
			};
            var client = new RestClient();
            var request = new RestRequest(URL)
				.AddJsonBody(body)
				.AddHeader("Access-Token", APIKey);
            var response = client.PostAsync<MyMessage>(request).GetAwaiter().GetResult();
            //if (response.StatusCode != System.Net.HttpStatusCode.OK)
			//	throw new Exception("Problem sending message, HTTP status code " + response.StatusCode.ToString());
        }

        public class MyMessage
		{
			public string type        { get; set; }
			public string title       { get; set; }
			public string body        { get; set; }
			public string device_iden { get; set; }
		}
	}
}
