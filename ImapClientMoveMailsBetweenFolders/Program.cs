namespace ImapClientMoveMailsBetweenFolders
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("IMAP client - move an email from your inbox to a different folder");

			//
			// IMPORTANT !!!!
			// add the nuget package MailKit from Jeffrey Stedfast (doku in http://www.mimekit.net)
			//

			var _client = new ImapClient();
			Console.WriteLine("Enter the password for your email postbox: ");
			_client._password = EnterPassword();
			_client.Open();


			Console.WriteLine("\n\n\nThese are your mailbox folders:");
			var folders = _client.GetAllFolders();
			folders.ForEach(x => Console.WriteLine($"    - {x.Name}"));


			Console.WriteLine("\n\n\nReading the inbox...");
			var inbox = _client.GetFolderByName(folders, "inbox");
			var emails = _client.GetAllMessagesFromFolder(inbox);

			Console.WriteLine("\n\n\nThese are the last 5 emails:");
			var lastFiveEmails = emails.OrderByDescending(x => x.Msg.Date).Take(5).ToList();
			folders.ForEach(x => Console.WriteLine($"    - {x}"));


			Console.WriteLine("\n\n\nThe last email from your inbox is:");
			var lastEmail = emails.Last();
			Console.WriteLine($"    - {lastEmail}");


			Console.Write("\n\n\nEnter the folder name to move the last email to: ");
			var folderName = Console.ReadLine();
			Console.WriteLine();
			
			
			var destination = _client.GetFolderByName(folders, folderName);
			if (destination is null)
				Console.WriteLine("that folder doesn't exist!");


			Console.Write("\n\nMoving the mail to that folder...");
			_client.MoveEmailToFolder(lastEmail, inbox, destination);
			Console.WriteLine("done");
 			_client.Close();
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
