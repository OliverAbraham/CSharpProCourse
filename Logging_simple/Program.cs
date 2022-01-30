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
            _logger = new Logger();
        }
    }

    class MyClassDoingSomething
    {
		private ILogger _logger;

		public MyClassDoingSomething(ILogger logger)
		{
            _logger = logger;
		}

        public void DoSomething()
        {
            _logger.Log("I've got something to say!");
        }
    }
}
