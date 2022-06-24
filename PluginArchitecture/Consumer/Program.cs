using System;
using System.IO;
using System.Reflection;
using PluginBaseLibrary;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("Plugin Demo - Oliver Abraham");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("\n");


            string Dll = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "MyFirstPlugin.dll";
            Console.WriteLine("Loading the plugin library from " + Dll);
            var PluginAssembly = Assembly.LoadFrom(Dll);


            Console.WriteLine("Creating an instance of the plugin:");
            var MyEntryClass = PluginAssembly.GetType("MyFirstPlugin.PluginMain");
            IPlugin MyPlugin = (IPlugin) Activator.CreateInstance(MyEntryClass);


            Console.WriteLine("Using the instance:");
            MyPlugin.DoSomething();


            Console.WriteLine("Back in Main");
            Console.ReadKey();
        }
    }
}
