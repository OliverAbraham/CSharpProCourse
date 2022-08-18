using System;
using Confluent.Kafka;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Abraham.ProgramSettingsManager;

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

		private static Configuration _config;
		private static ProgramSettingsManager<Configuration> _manager;

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
                BootstrapServers = _config.BootstrapServers,
                GroupId          = _config.GroupId,
                AutoOffsetReset  = AutoOffsetReset.Earliest,
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                var topics = new List<string>() { _config.Topic };
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
