namespace Calculator_Commandline_Simple
{
    internal class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("Enter the first number:");
			var zahl1 = Console.ReadLine();
			Int32.TryParse(zahl1, out int summand1);

			Console.WriteLine("Enter the second number:");
			var zahl2 = Console.ReadLine();
			Int32.TryParse(zahl2, out int summand2);

			var ergebnis = summand1 + summand2;

			Console.WriteLine($"Result: {ergebnis}");			
			Console.ReadKey();
        }
    }
}