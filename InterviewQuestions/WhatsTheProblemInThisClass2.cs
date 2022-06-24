using System;
using System.Collections.Generic;
using System.Text;

namespace BewerberAufgabenUndFragen
{
	class WhatCouldHappenHere
	{
		public delegate void MyLoggerType(string message);

		public MyLoggerType Logger { get; set; }

		public WhatCouldHappenHere()
		{
			Logger = delegate(string message) { };
		}

		public void Method()
		{
			if (Logger != null)
				Logger("I'm doing something!");
		}
	}
}
