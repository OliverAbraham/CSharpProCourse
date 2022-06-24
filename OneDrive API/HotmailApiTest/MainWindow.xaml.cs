using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.WindowsForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotmailApiTest
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IOneDriveClient Client { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Connect();
        }

        private static async Task Connect()
        {
            string   clientId  = "000000004817ED58";
            string   returnUrl = "https://login.live.com/oauth20_desktop.srf";
            string[] scopes    = { "onedrive.readwrite", "wl.offline_access", "wl.signin" };

            Client = OneDriveClient.GetMicrosoftAccountClient(
                         clientId,
                         returnUrl,
                         scopes, 
                         webAuthenticationUi: new FormsWebAuthenticationUi());


            try
            {
                if (!Client.IsAuthenticated)
                {
                    await Client.AuthenticateAsync();
                }

                //await LoadFolderFromPath();

                await Get_Calendar_Items();
            }
            catch (OneDriveException exception)
            {
                // Swallow authentication cancelled exceptions
                if (!exception.IsMatch(OneDriveErrorCode.AuthenticationCancelled.ToString()))
                {
                    if (exception.IsMatch(OneDriveErrorCode.AuthenticationFailure.ToString()))
                    {
                        MessageBox.Show("Authentication failed", "Authentication failed");
                        var httpProvider = Client.HttpProvider as HttpProvider;
                        httpProvider.Dispose();
                        Client = null;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private static async Task Get_Calendar_Items()
        {
            LiveConnectSession session = AuthClient.Session; //  AuthClient.ExchangeAuthCodeAsync(result.AuthorizeCode);
            //LiveConnectClient liveConnectClient = new LiveConnectClient(session);
            //LiveOperationResult meRs = await liveConnectClient.GetAsync("me");
            //dynamic meData = meRs.Result;
            //string Name = meData.name;

            //LiveDownloadOperationResult meImgResult = await liveConnectClient.DownloadAsync("me/picture");
            //Image Image = Image.FromStream(meImgResult.Stream);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnCreateContact_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    var contact = new Dictionary<string, object>();
            //    contact.Add("first_name", "Roberto");
            //    contact.Add("last_name", "Tamburello");
            //    LiveConnectClient liveClient = new LiveConnectClient(this.session);
            //    LiveOperationResult operationResult = await liveClient.PostAsync("me/contacts", contact);
            //    dynamic result = operationResult.Result;
            //    this.infoTextBlock.Text = string.Join(" ", "Contact:", result.name, "ID:", result.id);
            //}
            //catch (LiveConnectException exception)
            //{
            //    this.infoTextBlock.Text = "Error creating contact: " + exception.Message;
            //}
        }
    }
}
