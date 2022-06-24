using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Abraham.Hausnet
{
    public class HausnetClient
    {
        #region ------------- Types and constants -------------------------------------------------
        #endregion



        #region ------------- Properties ----------------------------------------------------------

        public string HomeautomationServerUrl { get; internal set; }
        public string HomeautomationPassword  { get; internal set; }

        #endregion



        #region ------------- Events --------------------------------------------------------------

        public delegate void LoggingEventHandler(string message);
        public event LoggingEventHandler  OnLoggingEvent;

        #endregion



        #region ------------- Fields --------------------------------------------------------------
        #endregion



        #region ------------- Init ----------------------------------------------------------------
        #endregion



        #region ------------- Methods -------------------------------------------------------------

        internal void SendControllerStatus(string currentStatus)
        {
	        throw new NotImplementedException();
        }

        public void Change_data_object_value(string doname, string value)
        {
            Send_command_to_hausnet_server(new Command(doname, value));
        }

        #endregion



        #region ------------- Implementation ------------------------------------------------------

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

        private void Send_status_to_hausnet_server(int controllerID, string status)
        {
	        //Log($"Processing Status notification {status}");

	        string Url = $"{HomeautomationServerUrl}/api.htm?controller&idctl={controllerID}&values={status}&ID={HomeautomationPassword}";
	        string Reply = Send_get_request_to_homeautomation_server(Url);
	        bool Success = Reply.Contains("<body>OK");
	        if (!Success)
		        Log($"Processing Data Object ERROR!  Server replied with {Reply}");
        }

        private void Send_command_to_hausnet_server(Command command)
        {
            //Log($"Processing Data Object Change {command.Name} to {command.Value}");

            string Url = HomeautomationServerUrl + "/api.htm?setdo&do={0}&value={1}&ID={2}";
            string Command = string.Format(Url, command.Name, command.Value, HomeautomationPassword);
            string Reply = Send_get_request_to_homeautomation_server(Command);
            bool Success = Reply.Contains("<body>OK");
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

        private void Log(string message)
        {
            if (OnLoggingEvent != null)
                OnLoggingEvent(message);
        }

		#endregion

	}
}
