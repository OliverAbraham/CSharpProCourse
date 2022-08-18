using Microsoft.OneDrive.Sdk;
//using Microsoft.OneDrive.Sdk.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneDriveApiTest.WinForms
{
    public partial class Form1 : Form
    {
        public static IOneDriveClient Client { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Connect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private static async Task Connect()
        {
 //           string   clientId  = "000000004817ED58";
 //           string   returnUrl = "https://login.live.com/oauth20_desktop.srf";
 //           string[] scopes    = { "onedrive.readwrite", "wl.offline_access", "wl.signin" };
 //
 //           Client = OneDriveClient.GetMicrosoftAccountClient(
 //                        clientId,
 //                        returnUrl,
 //                        scopes, webAuthenticationUi: new FormsWebAuthenticationUi());
 //
 //
 //           try
 //           {
 //               if (!Client.IsAuthenticated)
 //               {
 //                   await Client.AuthenticateAsync();
 //               }
 //
 //               //var drive = await Client.Drive.Request().GetAsync();
 //
 //               await LoadFolderFromPath();
 //
 //               //UpdateConnectedStateUx(true);
 //           }
 //           catch (OneDriveException exception)
 //           {
 //               // Swallow authentication cancelled exceptions
 //               if (!exception.IsMatch(OneDriveErrorCode.AuthenticationCancelled.ToString()))
 //               {
 //                   if (exception.IsMatch(OneDriveErrorCode.AuthenticationFailure.ToString()))
 //                   {
 //                       MessageBox.Show("Authentication failed", "Authentication failed",MessageBoxButtons.OK);
 //                       var httpProvider = Client.HttpProvider as HttpProvider;
 //                       httpProvider.Dispose();
 //                       Client = null;
 //                   }
 //                   else
 //                   {
 //                       throw;
 //                   }
 //               }
 //           }
        }


        private static async Task LoadFolderFromPath(string path = null)
        {
 //           if (Client == null)
 //               return;
 //
 //           try
 //           {
 //               Item folder;
 //
 //               var expandValue = Client.ClientType == ClientType.Consumer
 //                   ? "thumbnails,children(expand=thumbnails)"
 //                   : "thumbnails,children";
 //
 //               if (path == null)
 //               {
 //                   folder = await Client.Drive.Root.Request().Expand(expandValue).GetAsync();
 //               }
 //               else
 //               {
 //                   folder =
 //                       await
 //                           Client.Drive.Root.ItemWithPath("/" + path)
 //                               .Request()
 //                               .Expand(expandValue)
 //                               .GetAsync();
 //               }
 //
 //               ProcessFolder(folder);
 //           }
 //           catch (Exception)
 //           {
 //           }
        }

        private static void ProcessFolder(Item folder)
        {
            if (folder != null)
            {
                LoadProperties(folder);

                if (folder.Folder != null && 
                    folder.Children != null && 
                    folder.Children.CurrentPage != null)
                {
                    LoadChildren(folder.Children.CurrentPage);
                }
            }
        }

        private static void LoadProperties(Item item)
        {
            //this.SelectedItem = item;
            //objectBrowser.SelectedItem = item;
        }

        private static void LoadChildren(IList<Item> items)
        {
            string Dateien = "";

            foreach (var obj in items)
            {
                Dateien += obj.Name + "\n";
            }

            MessageBox.Show("Dateien:\n\n" + Dateien);
        }
    }
}
