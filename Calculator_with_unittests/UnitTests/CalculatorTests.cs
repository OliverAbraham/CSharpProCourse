using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace Geschäftslogik.Tests
{
	[TestClass()]
    public class CalculatorTests
    {
        /// <summary>
        /// Testet, ob die Addition funktioniert.
        /// Erwartete Reaktion: Es kommt die Summe der Operanden heraus.
        /// </summary>
        [TestMethod()]
        public void Should_add_correctly()
        {
            Calculator Objekt = new Calculator();

            int Actual = Objekt.Add(3,4);

            int Expected = 7;
            Actual.Should().Be(Expected, "weil das beim Addieren herauskommen muss");
        }


        /// <summary>
        /// Testet, ob die Subtraktion funktioniert.
        /// Erwartete Reaktion: Es kommt die Differenz der Operanden heraus.
        /// </summary>
        [TestMethod()]
        public void Should_subtract_correctly()
        {
            Calculator Objekt = new Calculator();

            int Actual = Objekt.Subtract(3, 4);

            int Expected = -1;
            Actual.Should().Be(Expected, "weil das beim Subtrahieren herauskommen muss");
        }


        ///// <summary>
        ///// Testet, ob die Multiplikation funktioniert.
        ///// Erwartete Reaktion: Es kommt die Differenz der Operanden heraus.
        ///// </summary>
        //[TestMethod()]
        //public void Should_multiply_correctly()
        //{
        //    Calculator Objekt = new Calculator();
        //
        //    int Actual = Objekt.Multiply(3,4);
        //
        //    int Expected = 12;
        //    Actual.Should().Be(Expected, "weil 3 mal 4 nunmal 12 ergibt");
        //}

    }
}

