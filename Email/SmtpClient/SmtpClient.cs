using MailKit;
using MailKit.Search;
using MailKit.Security;
using MimeKit;

// IMPORTANT !!!!
// The IMAP reader needs a codepage provider to decode different character sets
// for this, add the nuget package "System.Encoding.Text.Codepages"
// and call the Encoding.RegisterProvider method, like this:
// 

namespace Abraham.Mail
{
	public class SmtpClient
    {
		private string	   _hostname = "smtp.1blu.de";
		private int		   _port 	 = 465;
		private bool	   _useSsl   = true;
		private string	   _username = "c263677_1-mail";
		public  string	   _password = "";
		private MailKit.Net.Smtp.SmtpClient _client;


		public SmtpClient()
		{
		}

		public void RegisterCodepageProvider()
		{
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
		}

		public void Open()
		{
            _client = new MailKit.Net.Smtp.SmtpClient();
			_client.Connect (_hostname, _port, (_useSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None));
			_client.Authenticate (_username, _password);
		}

		public void Close()
		{
			_client.Disconnect (true);
		}

		public void SendEmail(string from, string to, string subject, string body)
        {
			var message         = new MimeKit.MimeMessage();
			message.From		.Add(new MimeKit.MailboxAddress(from, from));
			message.To			.Add(new MimeKit.MailboxAddress(to, to));
			message.Subject     = subject;
			
			var builder			= new MimeKit.BodyBuilder();
			builder.TextBody    = body;
			//builder.Attachments.Add (...);
			message.Body        = builder.ToMessageBody();
			SendEmail(message);
		}

		public void SendEmail(MimeMessage message)
        {
			_client.Send(message);
        }
	}
}
