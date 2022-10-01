using MailKit;
using MailKit.Search;
using MailKit.Security;

// IMPORTANT !!!!
// The IMAP reader needs a codepage provider to decode different character sets
// for this, add the nuget package "System.Encoding.Text.Codepages"
// and call the Encoding.RegisterProvider method, like this:
// 

namespace Abraham.Mail
{
	public class Pop3Client
    {
		private string	   _hostname = "pop3.1blu.de";
		private int		   _port 	 = 995;
		private bool	   _useSsl   = true;
		private string	   _username = "c263677_1-mail";
		public  string	   _password = "";
		private MailKit.Net.Pop3.Pop3Client _client;


		public Pop3Client()
		{
		}

		public void RegisterCodepageProvider()
		{
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
		}

		public void Open()
		{
            _client = new MailKit.Net.Pop3.Pop3Client();
			_client.Connect (_hostname, _port, (_useSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None));
			_client.Authenticate (_username, _password);
		}

		public void Close()
		{
			_client.Disconnect (true);
		}

		public List<Message> GetAllMessages()
        {
			return GetMessages(0, _client.Count);
		}

		public List<Message> GetMessages(int startIndex, int count)
        {
			var results = new List<Message>();

			var messages = _client.GetMessages(startIndex, count);
			foreach(var message in messages)
				results.Add(new Message(new UniqueId(), message));

			return results;
        }
	}
}
