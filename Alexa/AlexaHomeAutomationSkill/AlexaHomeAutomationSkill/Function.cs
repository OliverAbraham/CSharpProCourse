using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using System.IO;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AlexaHomeAutomationSkill
{
    #region Business Logic
    public class Function
	{
		#region ------------- Configuration -------------------------------------------------------
		private const string _ServiceURL = "https://sqs.eu-west-1.amazonaws.com";
        private const string _Queue1Url  = "https://sqs.eu-west-1.amazonaws.com/358331695088/AbrahamAlexaQueue";
        private const string _Queue2Url  = "https://sqs.eu-west-1.amazonaws.com/358331695088/AbrahamAlexaQueueToLambda";
		private const string _Tempfile   = "/tmp/master_data_cache.dat";
		private bool         _Debug      = false;
		#endregion



		#region ------------- Properties ----------------------------------------------------------
		#endregion



		#region ------------- Fields --------------------------------------------------------------
		private AmazonSQSClient					_sqsClient;
        private string							_LogfileDateFormat = "yyyy-MM-dd HH:mm:ss fff";
        private Dictionary<string,Datenobjekt>	_DataObjects;
        private DateTime						_LastUpdateOfDataObjects;
		#endregion



		#region ------------- Init ----------------------------------------------------------------
        public Function()
        {
			_LastUpdateOfDataObjects = new DateTime(1901, 1, 1);
            _DataObjects = new Dictionary<string, Datenobjekt>();
			//_DataObjects.Add(new Datenobjekt()
			//{
			//	FriendlyName = "Arbeitszimmer",
			//	Name         = "AZ_DECKENLAMPE",
			//	Description  = "Arbeitszimmer Deckenlampe",
			//	Value        = "0"
			//});
			//_DataObjects.Add(new Datenobjekt()
			//{
			//	FriendlyName = "Deckenfluter",
			//	Name         = "AZ_DECKENFLUTER",
			//	Description  = "Arbeitszimmer Deckenfluter",
			//	Value        = "0"
			//});

			if (Environment.GetEnvironmentVariable("DEBUG") == "1")
                _Debug = true;
        }
		#endregion



		#region ------------- Methods -------------------------------------------------------------
        public object FunctionHandler(object json_input, ILambdaContext context)
        {
            try
            {
                if (!Convert_and_check_parameters(json_input, context, out Directive input))
                    return null;

				if (!Init_message_queue_client())
					return null;

                if (!Read_master_data())
					return null;

                if (input.Header.Namespace == "Alexa.Discovery")
                    return Execute(Process_Discovery_Messages(input));

                if (input.Header.Namespace == "Alexa")
                    return Execute(Process_ReportState_Messages(input));

                if (input.Header.Namespace == "Alexa.PowerController")
                    return Execute(Process_PowerController_Messages(input));
            }
            catch (Exception ex)
            {
                LogError(ex.ToString());
            }
            finally
            {
                LogDebug("Finish Time: " + DateTime.Now.ToString(_LogfileDateFormat));
            }
            return null;
        }
		#endregion



		#region ------------- Implementation ------------------------------------------------------
		#region ------------- Alexa Messages ------------------------------------------------------

        private bool Convert_and_check_parameters(object json_input, ILambdaContext context, out Directive input)
        {
            input = null;
            Log("Start Time: " + DateTime.Now.ToString(_LogfileDateFormat));
            LogDebug("DEBUG Switch: " + Environment.GetEnvironmentVariable("DEBUG"));
            LogDebug("DEBUG Input is  : " + json_input.ToString());

            if (context == null)
                return Fail("context is null!");

            AlexaRequest Request = JsonConvert.DeserializeObject<AlexaRequest>(json_input.ToString());
            if (Request == null)
                return Fail("Request is null!");

            input = Request.Directive;
            if (input == null)
                return Fail("input is null!");

            if (input.Header == null)
                return Fail("input.Header is null!");

            LogDebug($"DEBUG: Namespace {input.Header.Namespace} / Name {input.Header.Name}");
            return true;
        }

		private bool Init_message_queue_client()
		{
            LogDebug(nameof(Init_message_queue_client));
            _sqsClient = new AmazonSQSClient(new AmazonSQSConfig { ServiceURL = _ServiceURL });
            if (_sqsClient == null)
				return Fail("Error - no connection to queue!");
			else
				return true;
		}

		private object Process_Discovery_Messages(Directive input)
		{
			if (input.Header.Name != "Discover")
				throw new Exception("Unknown Discovery Message {input.Header.Name} in this namespace!");

			var Response = new DiscoverDevicesResponse();

			#region ############################# NEU #############################
			//Response.Event.Header = input.Header;
			//Response.Event.Header.Name = "Discover.Response";
			//Response.Event.Payload = Create_list_of_endpoints_from(_DataObjects);
			#endregion ############################################################

			#region ############################# ALT #############################
            Response.Event.Header      = input.Header;
            Response.Event.Header.Name = "Discover.Response";
            Response.Event.Payload     = new PayloadEndpoints();

            var Monitor                = new PayloadEndpoint();
            Monitor.EndpointId         = "W_DECKENLAMPE";
            Monitor.ManufacturerName   = "Oliver Abraham";
            Monitor.FriendlyName       = "Wohnzimmer Deckenlampe";
            Monitor.Description        = "Wohnzimmer Deckenlampe";
            Monitor.DisplayCategories  = new List<string>();
            Monitor.DisplayCategories.Add("SWITCH");

            Monitor.Cookie = new Cookie();
            Monitor.Cookie.key1 = "arbitrary key/value pairs for skill to reference this endpoint.";
            Monitor.Cookie.key2 = "There can be multiple entries";
            Monitor.Cookie.key3 = "but they should only be used for reference purposes.";
            Monitor.Cookie.key4 = "This is not a suitable place to maintain current endpoint state.";

            Monitor.Capabilities = new List<Capability>();
            var Cap = new Capability()
            {
                Type = "AlexaInterface",
                Interface = "Alexa",
                Version = "3"

            };
            Monitor.Capabilities.Add(Cap);

            Cap = new Capability()
            {
                Type = "AlexaInterface",
                Interface = "Alexa.PowerController",
                Version = "3"
            };
            Cap.Properties = new Properties();
            Cap.Properties.Supported = new List<Supplement>();
            Cap.Properties.Supported.Add(new Supplement() { Name = "powerState" });
            Cap.Properties.Retrievable = true;
            Monitor.Capabilities.Add(Cap);

            Response.Event.Payload.Endpoints.Add(Monitor);
			#endregion ############################################################

			return Response;
		}

		private PayloadEndpoints Create_list_of_endpoints_from(Dictionary<string,Datenobjekt> _objects)
		{
			var List = new PayloadEndpoints();

			foreach (var Object in _objects.Values)
				List.Endpoints.Add(Create_Endpoint(Object));

			return List;
		}

		private PayloadEndpoint Create_Endpoint(Datenobjekt _object)
        {
            var Endpoint               = new PayloadEndpoint();
            Endpoint.ManufacturerName  = "Abraham";
            Endpoint.EndpointId        = _object.Name;
            Endpoint.FriendlyName      = _object.FriendlyName;
            Endpoint.Description       = _object.Description;
            Endpoint.DisplayCategories.Add("SWITCH");
            //Endpoint.Cookie            = Create_Cookie();
            Endpoint.Capabilities.Add(Create_Alexa_Capability());
            Endpoint.Capabilities.Add(Create_PowerController_Capability());
            return Endpoint;
        }

        private static Capability Create_Alexa_Capability()
        {
            return new Capability()
            {
                Type      = "AlexaInterface",
                Interface = "Alexa",
                Version   = "3"
            };
        }

        private static Capability Create_PowerController_Capability()
        {
            var Cap = new Capability()
            {
                Type      = "AlexaInterface",
                Interface = "Alexa.PowerController",
                Version   = "3"
            };
            Cap.Properties = new Properties();
            Cap.Properties.Supported = new List<Supplement>();
            Cap.Properties.Supported.Add(new Supplement() { Name = "powerState" });
            Cap.Properties.Retrievable = true;
            return Cap;
        }

        private static Cookie Create_Cookie()
        {
            return new Cookie()
            {
                key1 = "arbitrary key/value pairs for skill to reference this endpoint.",
                key2 = "There can be multiple entries",
                key3 = "but they should only be used for reference purposes.",
                key4 = "This is not a suitable place to maintain current endpoint state."
            };
        }

        private object Process_ReportState_Messages(Directive input)
        {
            if (input.Header.Name != "ReportState")
                throw new Exception("Unknown Alexa Message " + input.Header.Name);

			string Name = input.Endpoint.EndpointId;
			string AlexaValue = "???";
			if (_DataObjects.ContainsKey(Name))
				AlexaValue = (_DataObjects[Name].Value != "0") ? "ON" : "OFF";


            var Response = new ReportStateResponse();
            var Prop                             = new StateReportProperty();
            Prop.Namespace                       = "Alexa.PowerController";
            Prop.Name                            = "powerState";
			Prop.Value                           = AlexaValue;
            Prop.TimeOfSample                    = DateTime.Now;
            Prop.UncertaintyInMilliseconds       = 10000;
            Response.Context.Properties.Add(Prop);

            Response.Event.Header.MessageId      = input.Header.MessageId;
            Response.Event.Header.Namespace      = "Alexa";
            Response.Event.Header.Name           = "ChangeReport";
            Response.Event.Header.PayloadVersion =  "3";

            Response.Event.Endpoint.Scope.Type   =  "BearerToken";
            Response.Event.Endpoint.Scope.Token  =  "access-token-from-Amazon";
            Response.Event.Endpoint.EndpointId   =  Name;//_DataObjects[Name].Name;

            Response.Event.Payload.Change.Cause.Type = "PHYSICAL_INTERACTION";
            Response.Event.Payload.Properties.Add(Prop); // ACHTUNG: HIER DOPPELTER INHALT!
            return Response;
        }

        private object Process_PowerController_Messages(Directive input)
        {
			string Name = input.Endpoint.EndpointId;

            if (!(input.Header.Name == "TurnOff" || 
                  input.Header.Name == "TurnOn"))
            {
                Log("Unknown Command: " + input.Header.Name);
                return null;
            }

			string Value = "???";
			string AlexaValue = "???";
			if (input.Header.Name == "TurnOff")
			{
				Value                  = "0";
				AlexaValue             = "OFF";
			}
			else if (input.Header.Name == "TurnOn")
			{
				Value                  = "1";
				AlexaValue             = "ON";
			}

			if (_DataObjects.ContainsKey(Name))
			{
				_DataObjects[Name].Value = Value;
			}


            LogDebug("Sending the request to the home automation server via queue 1...");
            var QueueMsg = new HomeAutomationMessage() 
            { 
                Type = input.Header.Name,
                ObjectName = input.Endpoint.EndpointId
            };
            var Packet = new QueueMessagePacket() { HomeAutomationMessage = QueueMsg };
            _sqsClient.SendMessageAsync(_Queue1Url, JsonConvert.SerializeObject(Packet));
            LogDebug("sent! ");


            // Responding to Alexa
            var Prop                               = new StateReportProperty();
            Prop.Namespace                         = "Alexa.PowerController";
            Prop.Name                              = "powerState";
            Prop.Value                             = AlexaValue;
            Prop.TimeOfSample                      = DateTime.Now;
            Prop.UncertaintyInMilliseconds         = 60000;

            var Response = new ReportStateResponse();
            Response.Context.Properties.Add(Prop);

            Response.Event.Header.Namespace        = "Alexa";
            Response.Event.Header.Name             = "Response";
            Response.Event.Header.PayloadVersion   =  "3";
            Response.Event.Header.MessageId        = input.Header.MessageId;
            Response.Event.Header.CorrelationToken = input.Header.CorrelationToken;

            Response.Event.Endpoint.Scope.Type     =  "BearerToken";
            Response.Event.Endpoint.Scope.Token    =  "access-token-from-Amazon";
            Response.Event.Endpoint.EndpointId     =  Name;
            return Response;
        }

	    #endregion

		#region ------------- Master data management ----------------------------------------------

		private bool Read_master_data()
        {
			try
			{
				LogDebug("Reading Master data...");
				if (File.Exists(_Tempfile))
				{
					LogDebug("Master data file exists!");
					Read_master_data_from_local_cache();
				}
				else
				{
					LogDebug("file doesn't exist, trying to create...");
					Read_master_data_from_home_automation_server();
					Write_master_data_to_local_cache();
					LogDebug("created!");
				}
				return true;
			}
			catch (Exception ex)
			{
				LogError("File access error: " + ex.ToString());
				LogError("Deleting master data file");
				File.Delete(_Tempfile);
				return false;
			}
		}

		private void Write_master_data_to_local_cache()
		{
			List<Datenobjekt> ListOfObjects = _DataObjects.Values.ToList();
			string MasterData = JsonConvert.SerializeObject(ListOfObjects);
			File.WriteAllText(_Tempfile, MasterData);
		}

		private void Read_master_data_from_local_cache()
		{
			string MasterData = File.ReadAllText(_Tempfile);
			List<Datenobjekt> DataObjects = JsonConvert.DeserializeObject<List<Datenobjekt>>(MasterData);
			Update_all_data_objects(DataObjects);
		}

		private async void Read_master_data_from_home_automation_server()
        {
			try
			{
				if (_LastUpdateOfDataObjects >= DateTime.Now.AddSeconds(-5))
				{
					LogDebug("No update of data object, because last action was within the last five seconds");
					return;
				}

				Log("Reading master data from home automation server");
				LogDebug("Reading queue attributes from queue 2.");
				List<string> attributeNames = new List<string>();
				attributeNames.Add("ApproximateNumberOfMessages");
				Task<GetQueueAttributesResponse> AttrResult = _sqsClient.GetQueueAttributesAsync(_Queue2Url, attributeNames);
				GetQueueAttributesResponse Attributes = AttrResult.Result;
				LogDebug("ApproximateNumberOfMessages = " + Attributes.ApproximateNumberOfMessages);
				if (Attributes.ApproximateNumberOfMessages == 0)
					return;

				LogDebug("Reading all data objects from queue 2.");
				Task<ReceiveMessageResponse> RecResult = _sqsClient.ReceiveMessageAsync(_Queue2Url);
				ReceiveMessageResponse Receiver = RecResult.Result;
				LogDebug($"Receiver = {Receiver}");
				if (Receiver == null)
				{
					Log("Error - no data!");
					return;
				}
				LogDebug("Response: " + Receiver.ToString() + "  Count: " + Receiver.Messages.Count);

				// Leave the last message in the queue
				int NumberOfMessages = Receiver.Messages.Count;
				foreach (var Message in Receiver.Messages)
				{
					LogDebug("Received: " + Message.Body);
					var Packet = JsonConvert.DeserializeObject<QueueMessagePacket>(Message.Body);
					Process_a_message_from_home_automation_server(Packet);
						
					if (NumberOfMessages > 1)
					{
						_sqsClient.DeleteMessageAsync(new DeleteMessageRequest(_Queue2Url, Message.ReceiptHandle));
						NumberOfMessages--;
					}
				}

				if (_DataObjects.Count == 0)
					LogError("Error - no data objects found in queue 2!");
				else
					LogDebug($"{_DataObjects.Count} data objects !");
			}
			catch (Exception ex)
			{
				LogError("Exception in Read_master_data_from_home_automation_server: " + ex.ToString());
			}
        }

		private bool Process_a_message_from_home_automation_server(QueueMessagePacket packet)
		{
			string Data = packet.HomeAutomationMessage.ObjectName;
			List<Datenobjekt> DataObjects = JsonConvert.DeserializeObject<List<Datenobjekt>>(Data);
			if (DataObjects == null)
				return Fail("No Data!");

			return Update_all_data_objects(DataObjects);
		}

		private bool Update_all_data_objects(List<Datenobjekt> dataObjects)
		{
			foreach (Datenobjekt Do in dataObjects)
				Update_one_data_object(Do);
			return true;
		}

		private void Update_one_data_object(Datenobjekt Do)
		{
			if (!_DataObjects.ContainsKey(Do.Name))
			{
				_DataObjects.Add(Do.Name, new Datenobjekt()
				{
					FriendlyName      = Do.FriendlyName,
					Name              = Do.Name,
					Description       = Do.Description,
					Value             = Do.Value
				});
			}
			else
			{
				Datenobjekt Existing  = _DataObjects[Do.Name];
				Existing.FriendlyName = Do.FriendlyName;
				Existing.Description  = Do.Description;
				Existing.Value        = Do.Value;
			}
		}

		#endregion

		#region ------------- Logging -------------------------------------------------------------

        private object Execute(object response)
        {
			return ExecuteAndLogReturnValue(response);
		}

        private object ExecuteAndLogReturnValue(object response)
        {
            LogDebug("return: " + JsonConvert.SerializeObject(response));
            return response;
        }

        private void LogDebug(string message)
        {
			if (_Debug) 
				Log(message);
		}
		
        private void LogError(string message)
        {
            Log("---------------- Error: " + message);
        }

        private void Log(string message)
        {
            LambdaLogger.Log(message + Environment.NewLine);
        }

        private bool Fail(string message)
        {
            LambdaLogger.Log("ERROR: " + message + Environment.NewLine);
            return false;
        }
	    #endregion
		#endregion
    }
    #endregion

    #region Common Messages
    public class AlexaRequest
    {
       [JsonProperty("directive")]
       public Directive Directive { get; set; }
    }

    public class Directive
    {
        [JsonProperty("header")]
        public DirectiveHeader Header { get; set; }

        [JsonProperty("endpoint")]
        public PayloadEndpoint Endpoint { get; set; }

        [JsonProperty("payload")]
        public DirectivePayload Payload { get; set; }
    }

    public class DirectiveHeader
    {

        [JsonProperty("messageId")]
        public string MessageId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("payloadVersion")]
        public string PayloadVersion { get; set; }

        [JsonProperty("correlationToken")]
        public string CorrelationToken { get; set; }
    }

    public class DirectivePayload
    {

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("appliance")]
        public Appliance Appliance { get; set; }
    }

    public class Appliance
    {
        [JsonProperty("applianceId")]
        public string ApplianceId { get; set; }
    }
    #endregion

    #region Discovery Messages

    public class DiscoverDevicesResponse
    {
        [JsonProperty("event")]
        public DiscoverDevicesEvent Event { get; set; }

        public DiscoverDevicesResponse()
        {
            Event = new DiscoverDevicesEvent();
        }
    }

    public class DiscoverDevicesEvent
    {

        [JsonProperty("header")]
        public DirectiveHeader Header { get; set; }

        [JsonProperty("payload")]
        public PayloadEndpoints Payload { get; set; }
    }

    public class PayloadEndpoints
    {

        [JsonProperty("endpoints")]
        public IList<PayloadEndpoint> Endpoints { get; set; }

        public PayloadEndpoints()
        {
            Endpoints = new List<PayloadEndpoint>();
        }
    }

    public class PayloadEndpoint
    {
        [JsonProperty("endpointId")]
        public string EndpointId { get; set; }

        [JsonProperty("manufacturerName")]
        public string ManufacturerName { get; set; }

        [JsonProperty("friendlyName")]
        public string FriendlyName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("displayCategories")]
        public List<string> DisplayCategories { get; set; }

        [JsonProperty("cookie")]
        public Cookie Cookie { get; set; }

        [JsonProperty("capabilities")]
        public IList<Capability> Capabilities { get; set; }

        public PayloadEndpoint()
        {
            DisplayCategories = new List<string>();
            Capabilities      = new List<Capability>();
        }
    }

    public class Cookie
    {
        public string key1 { get; set; }
        public string key2 { get; set; }
        public string key3 { get; set; }
        public string key4 { get; set; }
    }

    public class Capability
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("interface")]
        public string Interface { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }
    }

    public class Properties
    {
        [JsonProperty("supported")]
        public IList<Supplement> Supported { get; set; }

        [JsonProperty("retrievable")]
        public bool Retrievable { get; set; }
    }

    public class Supplement
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    #endregion

    #region State Report Messages
    public class ReportStateResponse
    {
        [JsonProperty("context")]
        public StateResponseContext Context { get; set; }

        [JsonProperty("event")]
        public ReportStateEvent Event { get; set; }

        public ReportStateResponse()
        {
            Context = new StateResponseContext();
            Event = new ReportStateEvent();
        }
    }

    public class StateResponseContext
    {
        [JsonProperty("properties")]
        public List<StateReportProperty> Properties { get; set; }

        public StateResponseContext()
        {
            Properties = new List<StateReportProperty>();
        }
    }

    public class StateReportProperty
    {
        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("timeOfSample")]
        public DateTime TimeOfSample { get; set; }

        [JsonProperty("uncertaintyInMilliseconds")]
        public int UncertaintyInMilliseconds { get; set; }
    }

    public class ReportStateEvent
    {

        [JsonProperty("header")]
        public ReportStateEventHeader Header { get; set; }

        [JsonProperty("endpoint")]
        public ReportStateEventEndpoint Endpoint { get; set; }

        [JsonProperty("payload")]
        public ReportStateEventPayload Payload { get; set; }

        public ReportStateEvent()
        {
            Header   = new ReportStateEventHeader();
            Endpoint = new ReportStateEventEndpoint();
            Payload  = new ReportStateEventPayload();
        }
    }

    public class ReportStateEventHeader
    {

        [JsonProperty("messageId")]
        public string MessageId { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("payloadVersion")]
        public string PayloadVersion { get; set; }

        [JsonProperty("correlationToken")]
        public string CorrelationToken { get; set; }
    }

    public class ReportStateEventEndpoint
    {

        [JsonProperty("scope")]
        public ReportStateEventEndpointScope Scope { get; set; }

        [JsonProperty("endpointId")]
        public string EndpointId { get; set; }

        public ReportStateEventEndpoint()
        {
            Scope = new ReportStateEventEndpointScope();
        }
    }

    public class ReportStateEventEndpointScope
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }

    public class ReportStateEventPayload
    {
        [JsonProperty("change")]
        public ReportStateEventPayloadChange Change { get; set; }

        [JsonProperty("properties")]
        public List<StateReportProperty> Properties { get; set; }

        public ReportStateEventPayload()
        {
            Change = new ReportStateEventPayloadChange();
            Properties = new List<StateReportProperty>();
        }
    }

    public class ReportStateEventPayloadChange
    {
        [JsonProperty("cause")]
        public ReportStateEventPayloadChangeCause Cause { get; set; }

        public ReportStateEventPayloadChange()
        {
            Cause = new ReportStateEventPayloadChangeCause();
        }
    }

    public class ReportStateEventPayloadChangeCause
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    #endregion

    #region Home automation messages
    public class QueueMessagePacket
    {
        [JsonProperty("message")]
        public HomeAutomationMessage HomeAutomationMessage  { get; set; }

        public QueueMessagePacket()
        {
            HomeAutomationMessage = new HomeAutomationMessage();
        }
    }

    public class HomeAutomationMessage
    {
        [JsonProperty("type")]
        public string Type  { get; set; }

        [JsonProperty("objectName")]
        public string ObjectName { get; set; }
    }
    #endregion

    #region Home automation messages
    public class Datenobjekt
    {
        public string Name  { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }
    #endregion
}
