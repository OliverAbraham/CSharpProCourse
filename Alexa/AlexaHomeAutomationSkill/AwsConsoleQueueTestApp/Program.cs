using System;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using Hausnet;

namespace AwsConsoleQueueTestApp
{
    class Program
    {
        #region ------------- Configuration -------------------------------------------------------
        private static string _HomeautomationServerUrl;
        private static string _HomeautomationPassword;
        private static string _ServiceURL;
        private static string _Queue1Url;
		private static string _Queue2Url;
		private static int    _PushIntervalInMinutes;
		private static bool   _LogToConsole;
		private static bool   _LogToFile;
		private static string _LogfileName;
		#endregion



		#region ------------- Properties ----------------------------------------------------------
		#endregion



		#region ------------- Fields --------------------------------------------------------------
		private static AmazonSQSClient QueueClient;
		#endregion



		#region ------------- Init ----------------------------------------------------------------
        
		public static void Main(string[] args)
		{
			do
			{
				try
				{
					ReadConfiguration();
					Greeting();
					Endless_Loop();
				}
				catch (Exception ex)
				{
					Log("Exception in Main loop: " + ex.ToString());
					Log("Waiting 10 seconds...");
					Thread.Sleep(10000);
					Log("Trying to reconnect to AWS");
				}
			}
			while (true);
		}

		private static void Greeting()
		{
			Log("---------------------------------------------------------------");
			Log("Alexa Home Automation Gateway");
			Log("Oliver Abraham, www.oliver-abraham.de, 20.02.2018");
			Log("---------------------------------------------------------------");
		}

		#endregion



		#region ------------- Methods -------------------------------------------------------------
		#endregion



		#region ------------- Implementation ------------------------------------------------------

		#region Communication with Alexa
		private static void Endless_Loop()
		{
			Log("Connecting to Amazon SQS at address: " + _ServiceURL);
			QueueClient = new AmazonSQSClient(new AmazonSQSConfig { ServiceURL = _ServiceURL });
			if (QueueClient == null)
			{
				Log("Error!");
				return;
			}

			var receiveMessageRequest = new ReceiveMessageRequest(_Queue1Url);
			receiveMessageRequest.WaitTimeSeconds = 20;
			Log("Now waiting for messages from Alexa lambda function ...");

			DateTime NextPushMessage = DateTime.Now;
			for (; ;)
			{
				Push_messages_to_alexa_with_queue2   (ref NextPushMessage);
				Read_messages_from_alexa_with_queue1 (receiveMessageRequest);
			}
		}

		private static void Push_messages_to_alexa_with_queue2(ref DateTime NextPushMessage)
		{
			if (NextPushMessage < DateTime.Now)
			{
				Log("Sending an update of all data object to amazon...");
				Transfer_data_objects_from_home_automation_server_to_amazon();
				NextPushMessage = DateTime.Now.AddMinutes(_PushIntervalInMinutes);
				Log("ok.");
			}
		}

		private static void Read_messages_from_alexa_with_queue1(ReceiveMessageRequest receiveMessageRequest)
		{
			Log("Waiting...");
			var Receiver = QueueClient.ReceiveMessage(receiveMessageRequest);

			foreach (var Message in Receiver.Messages)
			{
				Log("Received: " + Message.Body);
				QueueMessagePacket AlexaMessage = JsonConvert.DeserializeObject<QueueMessagePacket>(Message.Body);

				Process_a_message_from_alexa(AlexaMessage);

				QueueClient.DeleteMessage(new DeleteMessageRequest(_Queue1Url, Message.ReceiptHandle));
			}
		}

		private static void Process_a_message_from_alexa(QueueMessagePacket Request)
		{
			if (Request.HomeAutomationMessage.Type == "TurnOn")
				Send_command_to_hausnet_server(Request.HomeAutomationMessage.ObjectName, "1");
			else if (Request.HomeAutomationMessage.Type == "TurnOff")
				Send_command_to_hausnet_server(Request.HomeAutomationMessage.ObjectName, "0");
		}

		#endregion
		
		#region Gateway to the home automation server

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

        private static void Transfer_data_objects_from_home_automation_server_to_amazon()
        {
            Log("Reading data object states ...");

            Query_the_home_automation_server(out string HtmlData);

            Convert_html_data_to_list(HtmlData, out List<Datenobjekt> DataObjects);

            DataObjects = Filter_list(DataObjects);

            Set_TsUpdate_for_all_rows(DataObjects);

			Send_data_to_lambda_with_queue2(DataObjects);
        }

		private static List<Datenobjekt> Filter_list(List<Datenobjekt> dataObjects)
		{
			return (from d in dataObjects where d.Name.StartsWith("W_") select d).ToList();
		}

		private static void Query_the_home_automation_server(out string htmlData)
        {
            string Url = _HomeautomationServerUrl + "/api.htm?getalldo&ID={0}";
            string Command = string.Format(Url, _HomeautomationPassword);
            htmlData = Send_get_request_to_homeautomation_server(Command);
            bool Success = htmlData.Contains("<body>OK-");
            if (!Success)
                throw new Exception ($"Processing Data Object ERROR!  Server replied with {htmlData}");

            htmlData = htmlData.Replace("<body>OK-", "<body>");
            Log($"OK");
        }

        private static void Convert_html_data_to_list(string input, out List<Datenobjekt> output)
        {
            output = new List<Datenobjekt>();

            input = input.Replace("<html><title></title><body>", "");
            input = input.Replace("</body></html>", "");
            input = input.Replace("<ArrayOfDatenobjekt xmlns=\"http://schemas.datacontract.org/2004/07/Hausnet\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">",
                                  "<ArrayOfDatenobjekt>");
            input = input.Replace("<ArrayOfDatenobjekt>", "");
            input = input.Replace("</ArrayOfDatenobjekt>", "");

            do
            {
                input = input.Trim(new char[]{' ', '\n', '\r' });
                string Datenobjekt = GetNextTag("<Datenobjekt>", ref input);
                if (Datenobjekt.Length > 0)
                {
                    var Do = new Datenobjekt();
                    Do.Name         = GetNextTag("<m_Name>", ref Datenobjekt);
                    Do.Value        = GetNextTag("<m_Wert>", ref Datenobjekt);
                    Do.FriendlyName = GetNextTag("<m_SchönerName>", ref Datenobjekt);
                    output.Add(Do);
                }
            }
            while (input.Length > 0);
        }

        private static string GetNextTag(string tag, ref string input)
        {
            string EndTag = tag.Insert(1,"/");
            int Start = input.IndexOf(tag);
            if (Start == -1)
            {
                input = "";
                return "";
            }
            int End = input.IndexOf(EndTag, Start + tag.Length);
            if (End == -1)
            {
                input = "";
                return "";
            }

            int TagLength = End-Start+EndTag.Length;
            string Tag = input.Substring(Start + tag.Length, TagLength-tag.Length-EndTag.Length);
            input = input.Remove(Start, TagLength);
            return Tag;
        }

        private static void Set_TsUpdate_for_all_rows(List<Datenobjekt> dataObjects)
        {
            var Now = DateTime.Now;
            foreach (var Do in dataObjects)
                Do.TsUpdate = Now;
        }
		
        private static void Send_data_to_lambda_with_queue2(List<Datenobjekt> dataObjects)
        {
            Log("Sending all data object to amazon queue 2...");
            string SerializedData = JsonConvert.SerializeObject(dataObjects);
            var QueueMsg = new HomeAutomationMessage("All Data Objects", SerializedData);
            var Packet = new QueueMessagePacket(QueueMsg);
            var Response = QueueClient.SendMessageAsync(_Queue2Url, JsonConvert.SerializeObject(Packet));
            Log("sent! ");
        }

        private static void Send_command_to_hausnet_server(string doname, string value)
        {
            Send_command_to_hausnet_server(new Command(doname, value));
        }

        private static void Send_command_to_hausnet_server(Command command)
        {
            Log($"Processing Data Object Change {command.Name} to {command.Value}");

            string Url = _HomeautomationServerUrl + "/api.htm?setdo&do={0}&value={1}&ID={2}";
            string Command = string.Format(Url, command.Name, command.Value, _HomeautomationPassword);
            string Reply = Send_get_request_to_homeautomation_server(Command);
            bool Success = Reply.Contains("<body>OK");
            if (!Success)
                Log($"Processing Data Object ERROR!  Server replied with {Reply}");
            else
                Log($"OK");
        }
    
        private static string Send_get_request_to_homeautomation_server(string url)
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
        #endregion

		#region Configuration and Logging
        private static void ReadConfiguration()
        {
            Log("Reading configuration ...");
            _HomeautomationServerUrl = ConfigurationManager.AppSettings["HomeautomationServerUrl"];
            _HomeautomationPassword  = ConfigurationManager.AppSettings["HomeautomationPassword"];
            _ServiceURL              = ConfigurationManager.AppSettings["AmazonServiceUrl"];
            _Queue1Url               = ConfigurationManager.AppSettings["AmazonQueue1Url"];
            _Queue2Url               = ConfigurationManager.AppSettings["AmazonQueue2Url"];
			_PushIntervalInMinutes   = Convert.ToInt32(ConfigurationManager.AppSettings["PushIntervalInMinutes"]);
			_LogToConsole            = Convert.ToBoolean(ConfigurationManager.AppSettings["LogToConsole"]);
			_LogToFile               = Convert.ToBoolean(ConfigurationManager.AppSettings["LogToFile"]);
			_LogfileName             = ConfigurationManager.AppSettings["LogfileName"];
            Log("OK");
        }
		private static void Log(string message)
		{
			if (_LogToConsole) 
			{
				Console.WriteLine(message);
			}
			
			if (_LogToFile) 
			{
				string Line = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}    {message}\r\n";
				File.AppendAllText(_LogfileName, Line);
			}
		}
		#endregion

 		#endregion
   }

    #region Home automation messages
    public class QueueMessagePacket
    {
        [JsonProperty("message")]
        public HomeAutomationMessage HomeAutomationMessage  { get; set; }

        public QueueMessagePacket()
        {
            HomeAutomationMessage = new HomeAutomationMessage();
        }

        public QueueMessagePacket(HomeAutomationMessage message)
        {
            HomeAutomationMessage = message;
        }
    }

    public class HomeAutomationMessage
    {
        [JsonProperty("type")]
        public string Type  { get; set; }

        [JsonProperty("objectName")]
        public string ObjectName { get; set; }

		public HomeAutomationMessage()
		{
		}

		public HomeAutomationMessage(string type, string all_data_objects)
		{
            Type = type;
            ObjectName = all_data_objects;
		}
    }
    #endregion
}