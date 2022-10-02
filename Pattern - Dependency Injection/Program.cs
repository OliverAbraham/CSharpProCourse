namespace Pattern___Dependency_Injection
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Dependency Injection Pattern");

			// creating the current SQL adapter to use
            var adapter = new SqlServerAdapter();
            // could also be switched to this: var adapter = new OracleServerAdapter();

			// injecting the Adapter into my business logic
			var bl = new MyBusinessLogic(adapter); // this is called 'constructor injection'
			bl.DoSomeWork();
        }
    }

	public interface ISqlAdapter
	{
		void ExecuteSQL(string sql);
	}

    public class MyBusinessLogic // note that the business logic does NOT know which database it's using!
	{
		private ISqlAdapter _adapter; // mostly called "DbContext" in real programs

		public MyBusinessLogic(ISqlAdapter adapter)
		{
			_adapter = adapter;
		}

		public void DoSomeWork()
		{
			_adapter.ExecuteSQL("insert into table values(...)");
		}
	}

	public class SqlServerAdapter : ISqlAdapter
	{
		public void ExecuteSQL(string sql)
		{
			Console.WriteLine("Executing in SqlServerAdapter");
		}
	}

	public class OracleAdapter : ISqlAdapter
	{
		public void ExecuteSQL(string sql)
		{
			Console.WriteLine("Executing in OracleAdapter");
		}
	}
}