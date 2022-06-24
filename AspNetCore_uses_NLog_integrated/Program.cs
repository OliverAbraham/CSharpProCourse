using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;

// ADD THIS:
using Microsoft.Extensions.Logging;



//-------------------------------------------------------------------------------------------------
//
//        RUN AN ASP.NET CORE APPLICATION WITH INTEGRATED NLOG INSTEAD OF MICROSOFT LOGGER
//
//
//   1. Add 2 Nuget packages: NLog, NLog.Web.AspNetCore
//   2. Add Nuget package Nlog.Config or create your own NLog.config
//   3. Add a using
//   4. Add 1 line for nlog initialization
//   5. Add "ConfigureLogging" to remove the internal log provider
//   6. Add "UseNLog" to add our new provider
//   7. Start application standalone (with kestrel), not in IIS
//
//
// Links:
// https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-5
//
//
// Author:
// Oliver Abraham, mail@oliver-abraham.de, www.oliver-abraham.de
//
//-------------------------------------------------------------------------------------------------




namespace AspNetCore_uses_NLog_integrated
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// ADD THIS:
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})

				// ADD THIS:
				.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
				.UseNLog();
	}
}
