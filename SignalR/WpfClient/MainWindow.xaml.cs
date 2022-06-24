using Microsoft.AspNet.SignalR.Client;
using System.Net.Http;
using System.Windows;

namespace WpfClient
{

    // PROBLEM!!!!!
    // • 3x / 4x-Framework Clients können nicht zu Core-Hubs verbinden und umgekehrt!
	// • Das Protokoll hat sich verändert
	// • Keine Interoperabilität!


    public partial class MainWindow : Window
    {
        private HubConnection Connection;
        private IHubProxy HubProxy;

        public MainWindow()
        {
            InitializeComponent();
            Connect();
        }

        private async void Connect()
        {
            Connection = new HubConnection("http://localhost:61541/DemoHub"); 
            //Connection.Closed += Connection_Closed; 
            HubProxy = Connection.CreateHubProxy("demohub"); 
            //Handle incoming event from server: use Invoke to write to console from SignalR's thread 
            HubProxy.On<string, string>("ReceiveMessage", (name, message) => 
                this.Dispatcher.Invoke(() => Messages.Items.Add($"{name}: {message}"))
                ); 
            
            try 
            { 
                await Connection.Start(); 
            } 
            catch (HttpRequestException) 
            { 
                MessageBox.Show("Unable to connect to server: Start server before connecting clients.");
            } 
        }

        private void Button_Send_Click(object sender, RoutedEventArgs e)
        {
             HubProxy.Invoke("SendMessage", "User", textBoxMessage.Text);
        }
    }
}
