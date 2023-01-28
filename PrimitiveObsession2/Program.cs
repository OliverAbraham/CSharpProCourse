namespace PrimitiveObsession2
{
    internal class Program
    {
        class Bruttobetrag
        {
            private decimal _value;
            private Bruttobetrag(decimal value) { _value = value; }
            public static Bruttobetrag FromAmount(decimal value) { return new Bruttobetrag(value); }
            public decimal ToAmount() { return _value; }
            public override string ToString() { return _value.ToString(); }
        }
        
        class Nettobetrag
        {
            private decimal _value;
            private Nettobetrag(decimal value) { _value = value; }
            public static Nettobetrag FromAmount(decimal value) { return new Nettobetrag(value); }
            public decimal ToAmount() { return _value; }
            public override string ToString() { return _value.ToString(); }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Verkaufspreis berechnen");

            Nettobetrag nettopreis = Nettobetrag.FromAmount(100.0m);
            Bruttobetrag bruttopreis = BerechneBruttopreis(nettopreis);
            Console.WriteLine($"Der Bruttopreis ist {bruttopreis} Euro");

            decimal steueranteil = BerechneSteueranteil(bruttopreis, nettopreis);
            Console.WriteLine($"Der Steueranteil ist {steueranteil * 100:N2} %");
        }

        private static Bruttobetrag BerechneBruttopreis(Nettobetrag nettopreis)
        {
            var amount = nettopreis.ToAmount() * 1.19m;
            return Bruttobetrag.FromAmount(amount);
        }
 
        private static decimal BerechneSteueranteil(Bruttobetrag bruttopreis, Nettobetrag nettopreis)
        {
            return (bruttopreis.ToAmount()-nettopreis.ToAmount()) / nettopreis.ToAmount();
        }
   }
}