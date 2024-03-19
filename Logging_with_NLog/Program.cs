using NLog;
using System;
using System.Threading.Tasks;

namespace Logging_simple
{
	class Program
    {
		private static Logger _logger;

		static void Main(string[] args) 
        {
            Console.WriteLine("Press any key to end this program!");

            InitLogging();

            do
            {
                var myClass = new MyClassDoingSomething(_logger);
                myClass.DoSomething();
                Task.Delay(5000).Wait();
            }
            while (!Console.KeyAvailable);
        }

        private static void InitLogging()
        {
			// ATTENTION: Go to Properties of nlog.config and set it to "copy if newer", to have it in output directory!
            _logger = LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
        }
    }

    class MyClassDoingSomething
    {
		private Logger _logger;

		public MyClassDoingSomething(Logger logger)
		{
            _logger = logger;
		}

        public void DoSomething()
        {
            _logger.Log(LogLevel.Info, "I've got something to say!");
            _logger.Log(LogLevel.Debug, "This is a debug message!");
        }
    }
}
