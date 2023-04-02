using BusinessLogic;
using UserInterfaceLogic;

namespace Calculator_Commandline
{
    internal class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("Taschenrechner: (e für ende)");

			var engine = new Calculator();
            var ui = new CalculatorUI(engine); // <--- Dependency Injection
			
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