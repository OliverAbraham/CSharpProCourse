using System;

namespace BadBractises
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Virtual methods of hell - CPO Bad Practise");
			var logger = new Logger();
			var myinstance = new DerivedClass(logger);
		}
	}

	class DerivedClass : BaseClass
	{
		private Logger _logger;
		public DerivedClass(Logger logger) : base(logger)
		{
			_logger = logger;
		}

		protected override void LoadDto()
		{
			_logger.Log("I'm now in LoadDto!");
		}
	}

	class BaseClass
	{
		public BaseClass(Logger logger)
		{
			LoadDto();
		}

		protected virtual void LoadDto()
		{
		}
	}

	class Logger
	{
		public void Log(string message) { Console.WriteLine(message); }
	}
}
