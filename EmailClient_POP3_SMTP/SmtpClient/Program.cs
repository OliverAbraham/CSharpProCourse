using System.Net;
using System.Net.Mail;

namespace Smtp_Client
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Console.WriteLine("Sending emails to an SMTP postbox");
				SendEmail();
				Console.WriteLine("Email was sent");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		public static void SendEmail()
		{
			var to              = "mail@oliver-abraham.de";
			var from            = "mail@oliver-abraham.de";
			MailMessage message = new MailMessage(from, to);
			message.Subject     = "Test-Email";
			message.Body        = "Test-Email body";

			var server          = "smtp.1blu.de";
			var port            = 25;// 587;//465;
			var userName        = "c263677_1-mail";
			Console.Write("Enter the password for your email postbox: ");
			var password        = EnterPassword();


			// Credentials are necessary if the server requires the client 
			// to authenticate before it will send email on the client's behalf.
			var client = new SmtpClient(server, port);
			client.UseDefaultCredentials = true;
			client.EnableSsl = false;// true;
			client.Credentials = new NetworkCredential(userName, password);
			client.Send(message);
		}

		private static string EnterPassword()
		{
			var password = "";
			while (true)
			{
				var key = Console.ReadKey(intercept: true);
				if (key.KeyChar == '\r')
					break;
				password += key;
				Console.Write('*');
			}
			Console.WriteLine();
			Console.WriteLine();

			return password;
		}
	}
}
