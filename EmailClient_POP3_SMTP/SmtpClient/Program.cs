//
// IMPORTANT !!!!
// add the nuget package MailKit from Jeffrey Stedfast (doku in http://www.mimekit.net)
//

namespace Smtp_Client
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("SMTP client - Sending an email to an SMTP postbox");

			Console.WriteLine("Enter the password for your email postbox: ");
			var _client = new Abraham.Mail.SmtpClient();
			_client._password = EnterPassword();
			_client.Open();


			var message         = new MimeKit.MimeMessage();
			message.From		.Add(new MimeKit.MailboxAddress("mail@oliver-abraham.de", "mail@oliver-abraham.de"));
			message.To			.Add(new MimeKit.MailboxAddress("mail@oliver-abraham.de", "mail@oliver-abraham.de"));
			message.Subject     = "Test-Email";
			var builder			= new MimeKit.BodyBuilder();
			builder.TextBody    = "Test-Email body"; //builder.Attachments.Add (...);
			message.Body        = builder.ToMessageBody();

			_client.SendEmail(message);
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
}
