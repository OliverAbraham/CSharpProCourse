using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace SignalR_Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("SignalR Hub (Server) started.");
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
