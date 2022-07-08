using Geschäftslogik;
using System;
using Taschenrechner;

namespace UI1
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Taschenrechner: (e für ende)");

			var calculatorEngine = new Calculator();
            var ui = new CalculatorUI(calculatorEngine); // <--- Dependency Injection
			
			while (true)
			{
				if (Console.KeyAvailable)
				{
					var key = Console.ReadKey();
					if (key.KeyChar == 'e')
						break;
					var ausgabe = ui.Process_key_pressure_and_return_new_display_text(key.KeyChar.ToString());
					Console.Write($"\r{ausgabe}          \r");
				}
			}
		}
	}
}
