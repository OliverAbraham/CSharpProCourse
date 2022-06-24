using System;
using PluginBaseLibrary;

namespace MyFirstPlugin
{
    public class PluginMain : IPlugin
    {
        public void DoSomething()
        {
            Console.WriteLine("************** Hi I'm your plugin! *****************");
        }
    }
}
