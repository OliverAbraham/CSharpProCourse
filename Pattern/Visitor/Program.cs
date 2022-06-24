using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor
{
    // Gegeben sei eine fachliche Klassenhierarchie, bestehend aus drei Klassen,
    // einer Oberklasse mit zwei abgeleiteten Klassen.
    // Wir wollen die Klassen in XML serialisieren, sind aber der Meinung, dass
    // Der Code zur Konvertierung in XML nicht in diese Klassen hineingehört.
    // Stattdessen verwenden wir das Visitor-Pattern, um den Code in den Besucher auszulagern.
    // Die Fachklassen bekommen nur eine Accept-Methode, um den Besucher "hineinzulassen".
    abstract class Oberklasse
    {
        public void ConvertToXml(IMeinBesucher besucher)
        {
            Accept(besucher);
        }

        public abstract void Accept(IMeinBesucher besucher);
    }

    class UnterA : Oberklasse
    {
        public override void Accept(IMeinBesucher besucher)
        {
            besucher.Visit(this);
        }
    }

    class UnterB : Oberklasse
    {
        public override void Accept(IMeinBesucher besucher)
        {
            besucher.Visit(this);
        }
    }



    // Der Besucher besteht aus einem Interface, dass die Fachklassen hineinlassen
    // und einer Klasse mit je einer "Arbeitsmethode" pro Unterklasse.
    // Die Fachklasse lenkt in der Accept-Methode den Programmfluß auf die
    // jeweils passende Visit-Methode. Die Visit-Methode hat somit den this-Pointer
    // der Unterklasse und kann die Arbeit erledigen.
    interface IMeinBesucher
    {
        void Visit(UnterA x);
        void Visit(UnterB x);
    }

    class MeinXmlConverter : IMeinBesucher
    {
        public void Visit(UnterA x)
        {
            System.Console.WriteLine("Besucher arbeitet in UnterA");
        }
        public void Visit(UnterB x)
        {
            System.Console.WriteLine("Besucher arbeitet in UnterB");
        }
    }



    // Hier nun das Hauptprogramm, dass die Besucher verwendet. Es erzeugt eine Instanz 
    // des Besuchers und ruft damit die Basisklassenmethode "ConvertToXml" auf.
    //
    // Was sind nun die Vorteile ?
    //
    // 1) Wir rufen eine Basisklassenmethode auf und können eine spezifische Implementierung mitgeben
    // 2) Wir haben den spezifischen Code in unseren eigenen Besucher ausgelagert.
    // 3) Wenn jemand die Fachklassen um eine neue Unterklasse "UnterC" erweitert,
    //    sorgt der Compiler dafür, dass er die "Accept"-Methode nicht vergisst.
    // 4) Und dabei kann man die spezifische "Visit"-Implementierung aus nicht vergessen, 
    //    weil man die ja in "Accept" aufrufen muss.
    class Program
    {
        static void Main(string[] args)
        {
            MeinXmlConverter MeinBesucher = new MeinXmlConverter();

            UnterA FachklasseA = new UnterA();
            FachklasseA.ConvertToXml(MeinBesucher);

            UnterB FachklasseB = new UnterB();
            FachklasseB.ConvertToXml(MeinBesucher);
        }
    }
}
