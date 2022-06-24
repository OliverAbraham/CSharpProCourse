using Base;
using System;

namespace DataAccess
{
	public class SqlAdapter : ISqlAdapter
	{
		public void ExecuteSQL(string sql)
		{
			Console.WriteLine("Executing");
		}

		public string GetExecutedSql()
		{
			throw new NotImplementedException();
		}
	}
}
