using Polly;
using System;
using System.Threading;

namespace PollyTest
{
	class Program
	{
		// see https://github.com/App-vNext/Polly
		// see http://www.thepollyproject.org
		//
		// Polly is a .NET resilience and transient-fault-handling library that allows developers
		// to express policies such as Retry, Circuit Breaker, Timeout, Bulkhead Isolation, and
		// Fallback in a fluent and thread-safe manner.
		//
		// Polly targets .NET Standard 1.1 (coverage: .NET Framework 4.5-4.6.1, .NET Core 1.0,
		// Mono, Xamarin, UWP, WP8.1+) and .NET Standard 2.0+ (coverage: .NET Framework 4.6.1,
		// .NET Core 2.0+, and later Mono, Xamarin and UWP targets).

		static void Main(string[] args)
		{
			Console.WriteLine("Testing the polly library with a simple retry pattern");

			var policy = Policy
				.Handle<MyDatabaseException>()
				.Retry();

			Console.WriteLine("Simulating a sequence of SQL commands, where every 5th call fails.");
			policy.Execute(
				delegate()
				{
					Console.WriteLine($"Sequence is started from the beginning");
					do
					{
						Do_something_and_throw_exception_every_fifth_time();
						Thread.Sleep(1000);
					} while (!Console.KeyAvailable);

				});
		}

		private static int _CallCounter = 0;

		private static void Do_something_and_throw_exception_every_fifth_time()
		{
			_CallCounter++;
			Console.WriteLine($"Call no. {_CallCounter}...");
			if ((_CallCounter % 5) == 0)
			{
				Console.WriteLine($"Throwing an exception for demonstration!");
				throw new MyDatabaseException();
			}
		}

		private class MyDatabaseException : Exception {}
	}
}
