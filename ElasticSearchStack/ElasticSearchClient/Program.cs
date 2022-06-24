using Elasticsearch.Net;
using System;
using System.Text;

namespace ElasticSearchClient
{
    class Program
    {
        static ConnectionConfiguration _Settings;
        static ElasticLowLevelClient _Client;

        static void Main(string[] args)
        {
            Console.WriteLine("ElasticSearchClient Core");

            _Settings = new ConnectionConfiguration(new Uri("http://ho1900112:9200"))
            .RequestTimeout(TimeSpan.FromMinutes(2));
            _Client = new ElasticLowLevelClient(_Settings);

            Add("testoa1", "message1");
            Console.ReadLine();
        }

        private static void Add(string firstName, string lastName)
        {
            var person = new Person
            {
                FirstName = firstName,
                LastName = lastName
            };

            PostData postData = PostData.Serializable(person);
            var indexResponse = _Client.Index<BytesResponse>("people2", "person", postData, null);
            byte[] responseBytes = indexResponse.Body;
            string Response = Encoding.UTF8.GetString(responseBytes);
            Console.WriteLine($"Response: {Response}");
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
