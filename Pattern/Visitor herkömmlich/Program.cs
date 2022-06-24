using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor_herkömmlich
{
    // Vergleiche hierzu das Programm "Visitor".
    // Wir erstellen unseren Code ohne Visitor-Muster
    class Oberklasse
    {
        public virtual void ConvertToXml()
        {
            System.Console.WriteLine("Methode in Oberklasse");
        }
    }

    class UnterA : Oberklasse
    {
        public override void ConvertToXml()
        {
            System.Console.WriteLine("Methode in UnterA");
        }
    }

    class UnterB : Oberklasse
    {
        public override void ConvertToXml()
        {
            System.Console.WriteLine("Methode in UnterB");
        }
    }


    // Was ist der Unterschied zum Visitor-Pattern ?
    // Wir haben die gleiche Spezialisierung wie beim Visitor-Pattern, 
    // haben aber den Code in den Fachklassen.
    // Und haben nicht das
    class Program
    {
        static void Main(string[] args)
        {
            Oberklasse Ober = new Oberklasse();
            Ober.ConvertToXml();

            UnterA FachklasseA = new UnterA();
            FachklasseA.ConvertToXml();

            UnterB FachklasseB = new UnterB();
            FachklasseB.ConvertToXml();
        }
    }
}
