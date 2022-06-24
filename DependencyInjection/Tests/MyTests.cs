using System.Diagnostics;
using Base;
using BusinessLogic;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class MyTests
	{
		[TestMethod]
		public void TestMethod1()
		{
			ISqlAdapter adapterMock = new SqlAdapterMock();
			var sut = new MyBusinessLogic(adapterMock);
			sut.SaveChanges();

			Assert.AreEqual(adapterMock.GetExecutedSql(), "insert into blah");
		}
	}

	internal class SqlAdapterMock : ISqlAdapter
	{
		public string SuppliedSql;

		public void ExecuteSQL(string sql)
		{
			SuppliedSql = sql;
		}

		public string GetExecutedSql()
		{
			return SuppliedSql;
		}
	}
}
