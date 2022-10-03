//
// IMPORTANT !!!!
// add the nuget package MaiAbraham.Mail from Oliver Abraham (doku in https://github.com/OliverAbraham/Abraham.Mail)
//

using Abraham.Mail;

namespace Smtp_Client;

class Program
{
	static void Main()
	{
		Console.WriteLine("SMTP client - Sending an email to an SMTP postbox");

		Console.WriteLine("Enter the password for your email postbox: ");
		var password = EnterPassword();

		var _client = new Abraham.Mail.SmtpClient()
			.UseHostname("smtp.1blu.de")
			.UseSecurityProtocol(Security.Ssl)
			.UseAuthentication("c263677_1-mail", password)
			.Open();


		var from		    = "mail@oliver-abraham.de";
		var to			    = "mail@oliver-abraham.de";
		var subject         = "Test-Email";
		var body            = "Test-Email body";

		_client.SendEmail(from, to, subject, body);
		Console.WriteLine("done");
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
