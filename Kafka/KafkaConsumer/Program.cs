using System;
using Confluent.Kafka;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Abraham.ProgramSettings;
using System.Collections.Generic;

namespace KafkaConsumer
{
	class Program
	{
		#region ------------- Fields --------------------------------------------------------------
		#region Configuration

		private class Configuration
		{
			public string BootstrapServers   { get; set; }
			public string GroupId            { get; set; }
			public string Topic              { get; set; }
		}

		private static Configuration _Config;
		private static ProgramSettingsManager<Configuration> _ConfigurationManager;

		#endregion
		#endregion



		#region ------------- Init ----------------------------------------------------------------
		static void Main(string[] args)
		{
			Console.WriteLine("Kafka Consumer");
			if (!ReadConfiguration())
				return;

            var config = new ConsumerConfig
            {
                BootstrapServers = _Config.BootstrapServers,
                GroupId          = _Config.GroupId,
                AutoOffsetReset  = AutoOffsetReset.Earliest,
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                var topics = new List<string>() { _Config.Topic };
                consumer.Subscribe(topics);

                var cancellationToken = new CancellationToken(false);
                bool cancelled = false;
                while (!cancelled)
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    Console.WriteLine($"{consumeResult.Value}");
                }

                consumer.Close();
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
