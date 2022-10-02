namespace Pattern___Nullobject_Pattern
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Nullobject pattern demo");

            // Calling a method on a worker class that uses a logger
            // we give it our logger we want to use
            var firstInstance = new MyClass();
            firstInstance.Logger = delegate(string message) { Console.WriteLine(message); };
            firstInstance.DoSomething();

            // Creating a new instance of our worker class, but this time without assigning a logger
            // now the class has to use the Nullobject
            var secondInstance = new MyClass();
            secondInstance.DoSomething();

            // whats the win?
            // class "MyClass" doesn't have to deal with null checks for Logger.
            // it can be sure Logger is always usable
        }
    }

    internal class MyClass
    {
        public delegate void LoggerDelegate(string message);
        public LoggerDelegate Logger    
        { 
            get 
            { 
                return _logger; 
            }
            set 
            { 
                if (value is not null) 
                    _logger = value; 
                else 
                    _logger = delegate(string message) { /* this is the nullobject that does nothing */ };
            }
        }
        private LoggerDelegate _logger;

        public MyClass()
        {
            Logger = null; // set the null object
        }

        internal void DoSomething()
        {
            Logger("Hi, I'm in DoSomething!");
        }
    }
}