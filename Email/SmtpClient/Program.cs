//
// IMPORTANT !!!!
// add the nuget package MaiAbraham.Mail from Oliver Abraham (doku in https://github.com/OliverAbraham/Abraham.Mail)
//

using Abraham.Mail;
using MimeKit;

namespace Smtp_Client;

class Program
{
	static void Main()
	{
		Console.WriteLine("SMTP client - Sending an email to an SMTP postbox");

		Console.WriteLine("Enter the password for your email postbox: ");
		var username = "ENTER YOUR EMAIL USERNAME HERE";
		var password = EnterPassword();

		var _client = new Abraham.Mail.SmtpClient()
			.UseHostname("smtp.1blu.de")
			.UseSecurityProtocol(Security.Ssl)
			.UseAuthentication(username, password)
			.Open();


		var from		    = "mail@oliver-abraham.de";
		var to			    = "mail@oliver-abraham.de";
		var subject         = "Test-Email";
		var body            = "Test-Email body";



		// sending a simple email
		//_client.SendEmail(from, to, subject, body);
		//Console.WriteLine("done");



		// sending an email with attachments
		// first create some example files
		File.WriteAllText("myFile1.txt", "hello world!");
		File.WriteAllText("myFile2.txt", "hello world!");

		var attachments = new List<MimeEntity>();
		attachments.Add(_client.CreateFileAttachment("myFile1.txt"));
		attachments.Add(_client.CreateFileAttachment("myFile2.txt"));
		_client.SendEmail(from, to, subject, body, attachments);
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
