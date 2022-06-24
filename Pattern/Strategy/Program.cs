using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategy
{
    class Program
    {
        static void Main(string[] args)
        {
            IStrategy s;
            Console.WriteLine("Wir erzeugen zunächst die erste Strategie und benutzen die Methoden:");
            s = new Strategy1();
            Mach_was_damit(s);

            Console.WriteLine("Jetzt schalten wir auf die zweite Strategie um:");
            s = new Strategy2();
            Mach_was_damit(s);
            Console.ReadKey();
        }

        static void Mach_was_damit(IStrategy s)
        {
            Console.WriteLine("Hier in Methode 'Mach_was_damit' wissen wir nicht mehr, womit wir arbeiten:");
            s.Load();
            s.Save();
        }
    }

    interface IStrategy
    {
        void Load();
        void Save();
    }

    class Strategy1 : IStrategy
    {
        public void Load()
        {
            Console.WriteLine("Strategy1.Load");
        }

        public void Save()
        {
            Console.WriteLine("Strategy1.Save");
        }
    }

    class Strategy2 : IStrategy
    {
        public void Load()
        {
            Console.WriteLine("Strategy2.Load");
        }

        public void Save()
        {
            Console.WriteLine("Strategy2.Save");
        }
    }
}
