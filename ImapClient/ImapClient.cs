using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;

// IMPORTANT !!!!
// The IMAP reader needs a codepage provider to decode different character sets
// for this, add the nuget package "System.Encoding.Text.Codepages"
// and call the Encoding.RegisterProvider method, like this:
// 

namespace Abraham.Mail
{
	public class ImapClient
    {
		private string	   _hostname = "imap.1blu.de";
		private int		   _port 	 = 993;
		private bool	   _useSsl   = true;
		private string	   _username = "c263677_1-mail";
		public  string	   _password = "";
		private MailKit.Net.Imap.ImapClient _client;


		public ImapClient()
		{
		}

		public void RegisterCodepageProvider()
		{
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
		}

		public void Open()
		{
            _client = new MailKit.Net.Imap.ImapClient();
			_client.Connect (_hostname, _port, (_useSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None));
			_client.Authenticate (_username, _password);
		}

		public void Close()
		{
			_client.Disconnect (true);
		}

		public List<IMailFolder> GetAllFolders()
        {
			var results = new List<IMailFolder>();

			var personal = _client.GetFolder(_client.PersonalNamespaces[0]);
			foreach (var folder in personal.GetSubfolders (false))
				results.Add (folder);

			return results;
        }

		public IMailFolder GetFolderByName(string name, bool caseInsensitive = true)
		{
			var allFolders = GetAllFolders();
			return GetFolderByName(allFolders, name);
		}

		public IMailFolder GetFolderByName(List<IMailFolder> folders, string name, bool caseInsensitive = true)
		{
			if (caseInsensitive)
				return folders.Where(x => x.Name.ToUpper() == name.ToUpper()).First();
			else
				return folders.Where(x => x.Name == name).First();
		}

		public List<Message> GetUnreadMessagesFromFolder(IMailFolder folder)
		{
			return GetMessagesFromFolder(folder, SearchQuery.NotSeen);
		}

		public List<Message> GetAllMessagesFromFolder(IMailFolder folder)
        {
			return GetMessagesFromFolder(folder, SearchQuery.All);
        }

		public List<Message> GetMessagesFromFolder(IMailFolder folder, SearchQuery searchQuery)
        {
			var results = new List<Message>();
			folder.Open(FolderAccess.ReadOnly);

			var uniqueIDs = folder.Search (searchQuery);
			foreach (var uniqueID in uniqueIDs) 
				results.Add(new Message(uniqueID, _client.Inbox.GetMessage (uniqueID)));

			folder.Close();
			return results;
        }

		public void MoveEmailToFolder(Message email, IMailFolder source, IMailFolder destination)
		{
			source     .Open(FolderAccess.ReadWrite);
			source.MoveTo(email.UID, destination);
			source     .Close();
		}
	}
}
