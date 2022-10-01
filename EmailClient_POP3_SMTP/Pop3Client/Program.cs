//
// IMPORTANT !!!!
// add the nuget package MailKit from Jeffrey Stedfast (doku in http://www.mimekit.net)
//

namespace Pop3_Client
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("POP3 client - read all emails from your inbox");

			Console.WriteLine("Enter the password for your email postbox: ");
			var _client = new Abraham.Mail.Pop3Client();
			_client._password = EnterPassword();
			_client.Open();


			Console.Write("Reading the inbox...");
			var emails = _client.GetAllMessages();
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
