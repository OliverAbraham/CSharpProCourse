//
// IMPORTANT !!!!
// add the nuget package MaiAbraham.Mail from Oliver Abraham (doku in https://github.com/OliverAbraham/Abraham.Mail)
//

using Abraham.Mail;

namespace ImapClient;

class Program
{
	static void Main()
	{
		Console.WriteLine("IMAP client - read all unread emails from your inbox");

		Console.WriteLine("Enter the password for your email postbox: ");
		var password = EnterPassword();

		var _client = new Abraham.Mail.ImapClient()
			.UseHostname("imap.1blu.de")
			.UseSecurityProtocol(Security.Ssl)
			.UseAuthentication("c263677_1-mail", password)
			.Open();


		Console.WriteLine("\n\n\nSelecting the inbox...");
		var inbox = _client.GetFolderByName("inbox");


		Console.Write("Reading the inbox...");
		var emails = _client.GetUnreadMessagesFromFolder(inbox).ToList();
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
