using System;

namespace UI0
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("Geben sie die erste Zahl ein:");
			var zahl1 = Console.ReadLine();
			Int32.TryParse(zahl1, out int summand1);

			Console.WriteLine("Geben sie die zweite Zahl ein:");
			var zahl2 = Console.ReadLine();
			Int32.TryParse(zahl2, out int summand2);

			var ergebnis = summand1 + summand2;

			Console.WriteLine($"Ergebnis: {ergebnis}");			
			Console.ReadKey();
        }
    }
}
