using System;
using System.Data.SqlClient;

namespace AdoNetDatabaseConnect
{
	class Program
	{
		private static string connectionString = "Data Source=HO1900109;Initial Catalog=CTS_Integration_Tests; Integrated Security=false; uid=CTS_Integration_Tests; pwd=start";

		static void Main(string[] args)
		{
			var connection = new SqlConnection(connectionString);
			connection.Open();

			Console.WriteLine($"Reading data from table BANK...");
			string sqlCommand = "SELECT * FROM DEMO.BANK";
			var sql = new SqlCommand(sqlCommand, connection);
			var results = sql.ExecuteReader();

			Console.WriteLine($"hasRows: {results.HasRows}");
			Console.WriteLine($"field count: {results.FieldCount}");

			while (results.Read())
			{
				var spalte1 = results["BSPNR"  ].ToString();
				var spalte2 = results["BSPNAME"].ToString();
				Console.WriteLine($"BSPNR: {spalte1,-5}   BSPNAME: {spalte2}");
			}
		}
	}
}

