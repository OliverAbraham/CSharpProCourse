//using Microsoft.OneDrive.Sdk;
//using Microsoft.OneDrive.Sdk.WindowsForms;
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

namespace OneDriveApiTest
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public static IOneDriveClient Client { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Connect();
        }

//        private static async Task Connect()
//        {
//            string   clientId  = "000000004817ED58";
//            string   returnUrl = "https://login.live.com/oauth20_desktop.srf";
//            string[] scopes    = { "onedrive.readwrite", "wl.offline_access", "wl.signin" };
//
//            Client = OneDriveClient.GetMicrosoftAccountClient(
//                         clientId,
//                         returnUrl,
//                         scopes, 
//                         webAuthenticationUi: new FormsWebAuthenticationUi());
//
//
//            try
//            {
//                if (!Client.IsAuthenticated)
//                {
//                    await Client.AuthenticateAsync();
//                }
//
//                //var drive = await Client.Drive.Request().GetAsync();
//
//                await LoadFolderFromPath();
//            }
//            catch (OneDriveException exception)
//            {
//                // Swallow authentication cancelled exceptions
//                if (!exception.IsMatch(OneDriveErrorCode.AuthenticationCancelled.ToString()))
//                {
//                    if (exception.IsMatch(OneDriveErrorCode.AuthenticationFailure.ToString()))
//                    {
//                        MessageBox.Show("Authentication failed", "Authentication failed");
//                        var httpProvider = Client.HttpProvider as HttpProvider;
//                        httpProvider.Dispose();
//                        Client = null;
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//            }
//        }

//        async Task LoadFolderFromPath(string path = null)
//        {
//            if (Client == null)
//                return;
//
//            try
//            {
//                Item folder;
//
//                var expandValue = Client.ClientType == ClientType.Consumer
//                    ? "thumbnails,children(expand=thumbnails)"
//                    : "thumbnails,children";
//
//                if (path == null)
//                {
//                    folder = await Client.Drive.Root.Request().Expand(expandValue).GetAsync();
//                }
//                else
//                {
//                    folder =
//                        await
//                            Client.Drive.Root.ItemWithPath("/" + path)
//                                .Request()
//                                .Expand(expandValue)
//                                .GetAsync();
//                }
//
//                ProcessFolder(folder);
//            }
//            catch (Exception)
//            {
//            }
//        }

//       private static void ProcessFolder(Item folder)
//       {
//           if (folder != null)
//           {
//               LoadProperties(folder);
//
//               if (folder.Folder != null && 
//                   folder.Children != null && 
//                   folder.Children.CurrentPage != null)
//               {
//                   LoadChildren(folder.Children.CurrentPage);
//               }
//           }
//       }
//
//       private static void LoadProperties(Item item)
//       {
//           //this.SelectedItem = item;
//           //objectBrowser.SelectedItem = item;
//       }
//
//       private static void LoadChildren(IList<Item> items)
//       {
//           string Dateien = "";
//
//           foreach (var obj in items)
//           {
//               Dateien += obj.Name + "\n";
//           }
//
//           MessageBox.Show("Dateien:\n\n" + Dateien);
//       }
   }
}
