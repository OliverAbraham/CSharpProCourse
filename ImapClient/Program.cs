//
// IMPORTANT !!!!
// add the nuget package MailKit from Jeffrey Stedfast (doku in http://www.mimekit.net)
//

namespace ImapClient
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("IMAP client - read all unread emails from your inbox");

			Console.WriteLine("Enter the password for your email postbox: ");
			var _client = new Abraham.Mail.ImapClient();
			_client._password = EnterPassword();
			_client.Open();


			Console.WriteLine("\n\n\nSelecting the inbox...");
			var inbox = _client.GetFolderByName("inbox");


			Console.Write("Reading the inbox...");
			var emails = _client.GetUnreadMessagesFromFolder(inbox);
			emails.ForEach(x => Console.WriteLine($"    - {x}"));
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
