using System;
using System.Collections.Generic;
using System.Text;

namespace BewerberAufgabenUndFragen
{
	class WhatsTheProblemInThisClass1
	{
		public List<int> MyItems { get; set; }

		public WhatsTheProblemInThisClass1()
		{
			MyItems = new List<int>();
		}

		public int GetCount()
		{
			return MyItems.Count;
		}
	}
}
