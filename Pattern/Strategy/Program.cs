using System;

namespace Strategy
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Strategy pattern demo.");
            
            IStrategy s;
            Console.WriteLine("Create the first strategy and use it:");
            s = new Strategy1();
            DoSomething(s);

            Console.WriteLine("Create the second and use it:");
            s = new Strategy2();
            DoSomething(s);
        }

        static void DoSomething(IStrategy s)
        {
            Console.WriteLine("Now in the strategy. Please not we don't know which strategy we use!");
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
        public void Load() { Console.WriteLine("Strategy1.Load"); }
        public void Save() { Console.WriteLine("Strategy1.Save"); }
    }

    class Strategy2 : IStrategy
    {
        public void Load() { Console.WriteLine("Strategy2.Load"); }
        public void Save() { Console.WriteLine("Strategy2.Save"); }
    }
}
