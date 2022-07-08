using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using Abraham.Threading;
using System.Collections.Concurrent;
using System.Configuration;

namespace Azure4Alexa.HomeAutomation
{
    public class HomeAutomationGateway
    {
        #region ------------- Eigenschaften -------------------------------------------------------

        public static HomeAutomationGateway Instance
        {
            get
            {
                return _Singleton;
            }
        }
        private static readonly HomeAutomationGateway _Singleton = new HomeAutomationGateway();

        #endregion



        #region ------------- Felder --------------------------------------------------------------

        private string HomeautomationServerUrl = ""; // i.e. http://localhost:89
        private string HomeautomationPassword  = "";

        private ThreadExtensions _Thread;

        private class Command
        {
            public string Name { get; set; }
            public string Value { get; set; }

            public Command(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }

        private ConcurrentQueue<Command> _CommandQueue;

        #endregion



        #region ------------- Init ----------------------------------------------------------------

        public HomeAutomationGateway()
        {
            Log("HomeAutomationGateway created.");
        }

        #endregion



        #region ------------- Methoden ------------------------------------------------------------

        public void Start()
        {
            Log("HomeAutomationGateway started.");
            ReadConfiguration();
            _CommandQueue = new ConcurrentQueue<Command>();
            _Thread = new ThreadExtensions(EventprocessorThread);
            _Thread.thread.Start();
        }

        private void ReadConfiguration()
        {
            HomeautomationServerUrl = ConfigurationManager.AppSettings["HomeautomationServerUrl"];
            HomeautomationPassword  = ConfigurationManager.AppSettings["HomeautomationPassword"];
        }

        public void Shutdown()
        {
            Log("HomeAutomationGateway stopping thread(s)...");
            _Thread.SendStopSignalAndWait();
            Log("HomeAutomationGateway stopped.");
        }

        public void Switch_data_object(string name, string value)
        {
            Log($"HomeAutomationGateway switch {name} to {value}.");
            _CommandQueue.Enqueue(new Command(name, value));
        }

        #endregion



        #region ------------- Implementation ------------------------------------------------------
    
        private void EventprocessorThread()
        {
            while (_Thread.Run)
            {
                Command Cmd;
                if (_CommandQueue.TryDequeue(out Cmd))
                {
                    Send_command_to_hausnet_server(Cmd);
                }

                if (!_Thread.Run)
                    break;
                Thread.Sleep(100);
            }
        }

        private void Send_command_to_hausnet_server(Command command)
        {
            Log($"Processing Data Object Change {command.Name} to {command.Value}");

            string Url = HomeautomationServerUrl + "/api.htm?setdo&do={0}&value={1}&ID={2}";
            string Command = string.Format(Url, command.Name, command.Value, HomeautomationPassword);
            string Reply = Send_get_request_to_homeautomation_server(Command);
            bool Success = Reply.StartsWith("OK");
            if (!Success)
                Log($"Processing Data Object ERROR!  Server replied with {Reply}");
        }
    
        private string Send_get_request_to_homeautomation_server(string url)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.CookieContainer = new CookieContainer();
            httpRequest.AllowAutoRedirect = true;
            string Seiteninhalt = "";

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                byte[] httpHeaderData = httpResponse.Headers.ToByteArray();
                Stream httpContentData = httpResponse.GetResponseStream();
                using (httpContentData)
                {
                    Encoding enc = Encoding.UTF8;
                    int AnzahlGelesen;
                    byte[] seiteninhalt = new byte[10000];
                    do
                    {
                        AnzahlGelesen = httpContentData.Read(seiteninhalt, 0, seiteninhalt.Length);
                        Seiteninhalt += enc.GetString(seiteninhalt, 0, AnzahlGelesen);
                    }
                    while (AnzahlGelesen > 0);
                }
            }
            httpResponse.Close();
            return Seiteninhalt;
        }

        private void Log(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }
        #endregion
   }
}