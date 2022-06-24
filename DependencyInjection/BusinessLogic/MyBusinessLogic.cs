using DataAccess;
using System;
using Base;

namespace BusinessLogic
{
	public class MyBusinessLogic
	{
		private ISqlAdapter _Adapter;

		public MyBusinessLogic(ISqlAdapter adapter)
		{
			_Adapter = adapter;
		}

		public void SaveChanges()
		{
			_Adapter.ExecuteSQL("insert into blah");
		}
	}
}
