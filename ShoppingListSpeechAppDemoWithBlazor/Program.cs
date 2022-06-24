using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace BlazorAppProgressive
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");

			builder.Services.AddSpeechRecognition();

			//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://192.168.0.4:5001") });
			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://www.heise.de") });

			await builder.Build().RunAsync();
		}
	}
}
