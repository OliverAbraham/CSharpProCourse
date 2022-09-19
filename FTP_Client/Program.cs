using FluentFTP;

Console.WriteLine("FTP client");

// NOTE: Add nuget package FluentFTP to your project



var username = File.ReadAllText(@"C:\Cloud\FtpClientDemoUsername.txt");
var password = File.ReadAllText(@"C:\Cloud\FtpClientDemoPassword.txt");

// create an FTP client and specify the host, username and password
// (delete the credentials to use the "anonymous" account)
var client = new FtpClient("ciridata.eu", username, password);

// connect to the server and automatically detect working FTP settings
client.AutoConnect();

// get a list of files and directories in the "/htdocs" folder
foreach (FtpListItem item in client.GetListing("/"))
	Console.WriteLine(item.Name);

// upload a file
client.UploadFile(@"Demofile.txt", "/Demofile.txt");

// move the uploaded file
//client.MoveFile("/Demofile.txt", "/subfolder/Demofile.txt");

// download the file again
client.DownloadFile(@"Demofile2.txt", "/Demofile.txt");

// compare the downloaded file with the server
if (client.CompareFile(@"Demofile.txt", "/Demofile.txt") == FtpCompareResult.Equal) { }

// delete the file
client.DeleteFile("/Demofile.txt");

// upload a folder and all its files
//client.UploadDirectory(@"C:\website\videos\", @"/public_html/videos", FtpFolderSyncMode.Update);

// upload a folder and all its files, and delete extra files on the server
//client.UploadDirectory(@"C:\website\assets\", @"/public_html/assets", FtpFolderSyncMode.Mirror);

// download a folder and all its files
//client.DownloadDirectory(@"C:\website\logs\", @"/public_html/logs", FtpFolderSyncMode.Update);

// download a folder and all its files, and delete extra files on disk
//client.DownloadDirectory(@"C:\website\dailybackup\", @"/public_html/", FtpFolderSyncMode.Mirror);

// delete a folder recursively
//client.DeleteDirectory("/htdocs/extras/");

// check if a file exists
//if (client.FileExists("/htdocs/big2.txt")) { }

// check if a folder exists
//if (client.DirectoryExists("/htdocs/extras/")) { }

// upload a file and retry 3 times before giving up
//client.Config.RetryAttempts = 3;
//client.UploadFile(@"C:\MyVideo.mp4", "/htdocs/big.txt", FtpRemoteExists.Overwrite, false, FtpVerify.Retry);

// disconnect! good bye!
client.Disconnect();
