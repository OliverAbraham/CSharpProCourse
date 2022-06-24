using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BewerberAufgabenUndFragen
{
	class Program
	{
		// What does this code print out ?

		public static void Main(string[] args)
		{
			Console.WriteLine( string.Join(",", MyMethod()) );
		}

		static IEnumerable<object> MyMethod()
		{
			yield return 1;
			yield return 2;
			yield return 3;
		}
	}
}
