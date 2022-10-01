using MailKit;
using MailKit.Search;
using MailKit.Security;
using MimeKit;

namespace ImapClient
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("IMAP client - read all emails from your inbox");

			//
			// IMPORTANT !!!!
			// add the nuget package MailKit from Jeffrey Stedfast (doku in http://www.mimekit.net)
			//


			// IMPORTANT !!!!
			// The IMAP reader needs a codepage provider to decode different character sets
			// for this, add the nuget package "System.Encoding.Text.Codepages"
			// and call the Encoding.RegisterProvider method, like this:
			// 
			RegisterCodepageProvider();



			Console.Write("Enter the password for your email postbox: ");
			var password = EnterPassword();

			Console.Write("Reading the inbox...");
			var emails = FetchAllMessages("imap.1blu.de", 993, true, "c263677_1-mail", password);

			foreach (var email in emails)
				Console.WriteLine($"{email.Date,-35} {email.From,-60} {email.Subject}");
		}


		/// <summary>
		/// Example showing:
		///  - how to fetch all messages from an IMAP mailbox
		/// </summary>
		/// <param name="hostname">Hostname of the server. For example: pop3.live.com</param>
		/// <param name="port">Host port to connect to. Normally: 110 for plain POP3, 995 for SSL POP3</param>
		/// <param name="useSsl">Whether or not to use SSL to connect to server</param>
		/// <param name="username">Username of the user on the server</param>
		/// <param name="password">Password of the user on the server</param>
		/// <returns>All Messages from the inbox</returns>
		public static List<MimeMessage> FetchAllMessages(string hostname, int port, bool useSsl, string username, string password)
        {
			var results = new List<MimeMessage>();

			using (var client = new MailKit.Net.Imap.ImapClient ())
			{
				client.Connect (hostname, port, (useSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None));
				client.Authenticate (username, password);
				client.Inbox.Open (FolderAccess.ReadOnly);

				var mailID = client.Inbox.Search (SearchQuery.All);

				foreach (var uid in mailID) 
					results.Add(client.Inbox.GetMessage (uid));
		
				client.Disconnect (true);
			}		
			
			return results;
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
