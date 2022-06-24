using Confluent.Kafka;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NetFxProducer
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Kafka Producer");

            var config = new ProducerConfig
            {
                BootstrapServers = "192.168.0.4:9092",
                ClientId = Dns.GetHostName(),
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
			{
                var message = new Message<Null, string> { Value="hello world" };

                try
                {
                    var dr = await producer.ProduceAsync("quickstart-events", message);
                    Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
			}

            while(!Console.KeyAvailable)
                Thread.Sleep(1000);
            Console.WriteLine($"finished");
            Console.ReadKey();
		}
	}
}
