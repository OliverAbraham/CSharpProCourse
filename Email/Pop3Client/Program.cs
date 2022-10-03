//
// IMPORTANT !!!!
// add the nuget package MaiAbraham.Mail from Oliver Abraham (doku in https://github.com/OliverAbraham/Abraham.Mail)
//

using Abraham.Mail;

namespace Pop3_Client;

class Program
{
	static void Main()
	{
		Console.WriteLine("POP3 client - read all emails from your inbox");

		Console.WriteLine("Enter the password for your email postbox: ");
		var password = EnterPassword();

		var _client = new Abraham.Mail.Pop3Client()
			.UseHostname("pop3.1blu.de")
			.UseSecurityProtocol(Security.Ssl)
			.UseAuthentication("c263677_1-mail", password)
			.Open();


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
