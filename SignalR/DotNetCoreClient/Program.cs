using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DotNetCoreClient
{
    class Program
    {
        static HubConnection connection;

        static void Main(string[] args)
        {
            Console.WriteLine("SignalR Client started");
            Console.WriteLine("Start the hub first, then start 2 or more clients with:");
            Console.WriteLine("dotnet DotNetCoreClient.dll");

            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:61541/DemoHub")
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0,5) * 1000);
                await connection.StartAsync();
            };

            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine($"{user}: {message}");
            });

            connection.StartAsync();
            Console.WriteLine("Connection started");
                
            Console.WriteLine("Enter message text and press Enter:");
            string input;
            do
            {
                input = Console.ReadLine();
                connection.InvokeAsync("SendMessage", "User", input);
            }
            while (input.Length > 0);
        }
    }
}
