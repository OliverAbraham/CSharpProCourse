namespace PrimitiveObsession1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Verkaufspreis berechnen");

            decimal nettopreis = 100.0m;
            decimal bruttopreis = BerechneBruttopreis(nettopreis);
            Console.WriteLine($"Der Bruttopreis ist {bruttopreis} Euro");

            decimal steueranteil = BerechneSteueranteil(nettopreis, bruttopreis);
            Console.WriteLine($"Der Steueranteil ist {steueranteil * 100:N2} %");
        }

        private static decimal BerechneBruttopreis(decimal nettopreis)
        {
            return nettopreis * 1.19m;
        }
 
        private static decimal BerechneSteueranteil(decimal bruttopreis, decimal nettopreis)
        {
            return (bruttopreis-nettopreis) / nettopreis;
        }
   }
}