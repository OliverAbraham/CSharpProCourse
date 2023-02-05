using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using WebDav;

namespace NextcloudClientWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<NextcloudFile> Files = new ObservableCollection<NextcloudFile>();
        private WebDavClient _client;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            NextcloudFilesDataGrid.ItemsSource = Files;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadFilesFromNextcloud();
        }

        private async Task LoadFilesFromNextcloud()
        {
            // note: Create a json file with a content like this:
            // {"UserName":"my-username","Password":"my-password"}

            // Load username and password from a text file, to connect to your WebDAV server
            var serializedCredentials = File.ReadAllText(@"C:\Credentials\CSharpProCourse-WebDavClient.json");
            var credentials = (MyCredentials)JsonConvert.DeserializeObject<MyCredentials>(serializedCredentials);


            var clientParams = new WebDavClientParams
            {
                BaseAddress = new Uri("http://www.abraham-beratung.de/nextcloud/remote.php/dav"),
                Credentials = new NetworkCredential(credentials.UserName, credentials.Password)
            };
            _client = new WebDavClient(clientParams);


            // get all files in a folder
            var result = await _client.Propfind("files/IT/Schulungen");
            if (!result.IsSuccessful)
                return;

            foreach(var resource in result.Resources)
            {
                var decodedUri = System.Web.HttpUtility.UrlDecode(resource.Uri);
                Files.Add(new NextcloudFile(decodedUri));
            }
        }
    }

    public class NextcloudFile
    {
        public NextcloudFile(string filename)
        {
            Filename = filename;
        }

        public string Filename { get; set; }
    }

    public class MyCredentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
