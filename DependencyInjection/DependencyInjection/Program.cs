using BusinessLogic;
using System;
using DataAccess;

namespace DependencyInjection
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("This is the UI");

			var myRealSqlAdapter = new SqlAdapter();
			var bl = new MyBusinessLogic(myRealSqlAdapter);
			bl.SaveChanges();
		}
	}
}
