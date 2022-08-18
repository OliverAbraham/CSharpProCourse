using System;
using Confluent.Kafka;
using System.Net;
using System.Threading.Tasks;
using Abraham.ProgramSettingsManager;

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

		private static Configuration _config;
		private static ProgramSettingsManager<Configuration> _manager;

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
                BootstrapServers     = _config.BootstrapServers,
                ClientId             = Dns.GetHostName(),
        		TransactionTimeoutMs = _config.TransactionTimeout * 1000,
		        MessageTimeoutMs     = _config.MessageTimeout * 1000,    
		        RequestTimeoutMs     = _config.RequestTimeout * 1000
            };

			var messagetext = args[0];
			Console.WriteLine($"Sending message '{messagetext}' to topic {_config.Topic}");

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
			{
                var message = new Message<Null, string> { Value=messagetext };

                try
                {
                    var dr = await producer.ProduceAsync(_config.Topic, message);
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
				_manager = new ProgramSettingsManager<Configuration>().UseFilename("appsettings.hjson");
				_manager.Load();
				_config = _manager.Data;
				if (_config == null)
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
        #endregion
	}
}
