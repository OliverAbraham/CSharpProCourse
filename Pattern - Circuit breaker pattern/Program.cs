namespace PollyTest;

class Program
{
	static void Main()
	{
		Console.WriteLine("Circuit breaker pattern - simpliest form");

		do
		{
			Do_something_with_a_circuit_breaker();
			
			if (_exceptionCounter >= 3)
			{
				Console.WriteLine("The circuit breaker has tripped. This loop will now be ended for safety reasons!");
				break;
			}

			Thread.Sleep(1000);
		} while (!Console.KeyAvailable);
	}

	static int _exceptionCounter = 0;
	private static void Do_something_with_a_circuit_breaker()
	{
		try
		{
			Do_something_and_simulate_a_fault_every_fifth_time();
		}
		catch (MyDatabaseException)
		{
			_exceptionCounter++;
		}
	}

	private static bool CircuitBreakerHasTripped()
	{
		return _exceptionCounter >= 3;
	}



	private static int _counter = 0;

	private static void Do_something_and_simulate_a_fault_every_fifth_time()
	{
		_counter++;
		Console.WriteLine($"Call no. {_counter}...");
		if ((_counter % 5) == 0)
		{
			Console.WriteLine($"Throwing an exception for demonstration!");
			throw new MyDatabaseException();
		}
	}

	private class MyDatabaseException : Exception {}
}
