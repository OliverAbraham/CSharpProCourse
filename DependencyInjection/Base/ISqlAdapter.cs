using System;

namespace Base
{
	public interface ISqlAdapter
	{
		void ExecuteSQL(string sql);
		string GetExecutedSql();
	}
}
