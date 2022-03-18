using System;
using CommandLine;

namespace DeleteOldFiles
{

	// IMPORTANT: add NuGet package "CommandLine" to your project !!!
	// IMPORTANT: add NuGet package "CommandLine" to your project !!!
	// IMPORTANT: add NuGet package "CommandLine" to your project !!!


	class Program
	{
		#region ------------- Command line options ------------------------------------------------
		class Options
		{
			[Option('t', "topic", Required = true, HelpText = "This is a required parameter")]
			public string Topic { get; set; }

			[Option('o', "opt", Required = false, HelpText = "This is an optional parameter")]
			public string Opt { get; set; }
		}
		#endregion



		#region ------------- Fields --------------------------------------------------------------
		private static Options _options;
		#endregion



		#region ------------- Init ----------------------------------------------------------------
		static void Main(string[] args)
		{
			ParseCommandLineArguments();
			if (_options == null)
			{
				Console.WriteLine("Missing arguments!");
				return;
			}
			Console.WriteLine($"You started your program with topic '{_options.Topic}'");
			Console.WriteLine($"The optional parameter is '{_options.Opt}'");
			Console.ReadKey();
		}
		#endregion



		#region ------------- Private methods -----------------------------------------------------
		private static void ParseCommandLineArguments()
		{
			string[] args = Environment.GetCommandLineArgs();
			CommandLine.Parser.Default.ParseArguments<Options>(args)
				.WithParsed<Options>(o =>
				{
					_options = o;
				})
				.WithNotParsed<Options>(errs =>
				{
				});
		}
		#endregion
	}
}
