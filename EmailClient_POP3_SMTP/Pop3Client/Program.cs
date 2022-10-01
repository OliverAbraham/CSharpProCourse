using OpenPop.Mime;
using OpenPop.Pop3;

namespace Pop3_Client
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("POP3 client - alle Emails aus Posteingang lesen");

			//
			// IMPORTANT !!!!
			// add the nuget package OpenPop.NET
			//


			// IMPORTANT !!!!
			// The POP3 reader needs a codepage provider to decode different character sets
			// for this, add the nuget package "System.Encoding.Text.Codepages"
			// and call the Encoding.RegisterProvider method, like this:
			// 
			RegisterCodepageProvider();



			Console.Write("Enter the password for your email postbox: ");
			var password = EnterPassword();

			Console.Write("Reading the inbox...");
			var emails = FetchAllMessages("pop3.1blu.de", 995, true, "c263677_1-mail", password);

			foreach (var email in emails)
				Console.WriteLine($"{email.Headers.Date,-40} {email.Headers.Subject}");
		}


		/// <summary>
		/// Example showing:
		///  - how to fetch all messages from a POP3 server
		/// </summary>
		/// <param name="hostname">Hostname of the server. For example: pop3.live.com</param>
		/// <param name="port">Host port to connect to. Normally: 110 for plain POP3, 995 for SSL POP3</param>
		/// <param name="useSsl">Whether or not to use SSL to connect to server</param>
		/// <param name="username">Username of the user on the server</param>
		/// <param name="password">Password of the user on the server</param>
		/// <returns>All Messages on the POP3 server</returns>
		public static List<Message> FetchAllMessages(string hostname, int port, bool useSsl, string username, string password)
        {
            // The client disconnects from the server when being disposed
            using(Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(hostname, port, useSsl);

                // Authenticate ourselves towards the server
                client.Authenticate(username, password);

                // Get the number of messages in the inbox
                int messageCount = client.GetMessageCount();

                // We want to download all messages
                List<Message> allMessages = new List<Message>(messageCount);

                // Messages are numbered in the interval: [1, messageCount]
                // Ergo: message numbers are 1-based.
                // Most servers give the latest message the highest number
                for (int i = messageCount; i > 0; i--)
                {
                    try
					{
                        var message = client.GetMessage(i);
                        allMessages.Add(message);
					}
                    catch (Exception ex)
					{
                        Console.WriteLine($"Message no. {i} cannot be fetched!");
					}
                }

                // Now return the fetched messages
                return allMessages;
            }
        }

		public static void RegisterCodepageProvider()
		{
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
		}

		private static string EnterPassword()
		{
			var password = "";
			while (true)
			{
				var key = Console.ReadKey(intercept: true);
				if (key.KeyChar == '\r')
					break;
				password += key.KeyChar;
				Console.Write('*');
			}

			return password;
		}
	}
}
