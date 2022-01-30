using NLog;

namespace Logging_simple
{
	class Program
    {
		private static Logger _logger;

		static void Main(string[] args) 
        {
            InitLogging();

            var myClass = new MyClassDoingSomething(_logger);
            myClass.DoSomething();
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
