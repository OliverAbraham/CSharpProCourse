using System;
using Confluent.Kafka;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Abraham.ProgramSettings;

namespace KafkaProducer
{
	class Program
	{
		#region ------------- Fields --------------------------------------------------------------
		#region Configuration

		private class Configuration
		{
			public string BootstrapServers   { get; set; }
			public int    TransactionTimeout { get; set; }
			public int    MessageTimeout     { get; set; }
			public int    RequestTimeout     { get; set; }
			public string Topic              { get; set; }
		}

		private static Configuration _Config;
		private static ProgramSettingsManager<Configuration> _ConfigurationManager;

		#endregion
		#endregion



		#region ------------- Init ----------------------------------------------------------------
		static async Task Main(string[] args)
		{
			Console.WriteLine("Kafka Producer");
			if (!ReadConfiguration())
				return;

            var config = new ProducerConfig
            {
                BootstrapServers     = _Config.BootstrapServers,
                ClientId             = Dns.GetHostName(),
        		TransactionTimeoutMs = _Config.TransactionTimeout * 1000,
		        MessageTimeoutMs     = _Config.MessageTimeout * 1000,    
		        RequestTimeoutMs     = _Config.RequestTimeout * 1000
            };

			var messagetext = args[0];
			Console.WriteLine($"Sending message '{messagetext}' to topic {_Config.Topic}");

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
			{
                var message = new Message<Null, string> { Value=messagetext };

                try
                {
                    var dr = await producer.ProduceAsync(_Config.Topic, message);
                    Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
			}
		}
		#endregion



		#region ------------- Private methods -----------------------------------------------------
		private static bool ReadConfiguration()
		{
			try
			{
				_ConfigurationManager = new ProgramSettingsManager<Configuration>("appsettings.hjson");
				_Config = _ConfigurationManager.Load();
				if (_Config == null)
				{
					Console.WriteLine("No valid configuration found!\nExpecting file '{Filename}'");
					return false;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("There was a problem reading the configuration data.\n" + ex.ToString());
				return false;
			}
			return true;
		}

		private static void WriteConfiguration()
		{
			_ConfigurationManager.Save(_Config);
		}
        #endregion
	}
}
