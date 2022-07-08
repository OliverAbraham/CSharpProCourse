using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Geschäftslogik;
using System;

namespace Taschenrechner.Tests
{
    [TestClass()]
    public class CalculatorUITests
    {
        private string Actual;
        private CalculatorUI Objekt;

        private void Setup()
        {
            Calculator DummyEngine = new Calculator();
            Objekt = new CalculatorUI(DummyEngine);
        }

        [TestMethod()]
        public void Should_combine_keys_correctly()
        {
            Setup();

            Actual = Objekt.Process_key_pressure_and_return_new_display_text("0");
            Actual.Should().Be("0", "weil wir die Null gedrückt haben");

            Actual = Objekt.Process_key_pressure_and_return_new_display_text("1");
            Actual.Should().Be("1", "weil wir die Eins gedrückt haben");

            Actual = Objekt.Process_key_pressure_and_return_new_display_text("2");
            Actual.Should().Be("12", "weil wir Eins und Zwei gedrückt haben");

            Actual = Objekt.Process_key_pressure_and_return_new_display_text("3");
            Actual.Should().Be("123", "weil wir Eins, Zwei und Drei gedrückt haben");
        }

        [TestMethod()]
        public void Should_process_equal_key_correctly()
        {
            Setup();
            Actual = Objekt.Process_key_pressure_and_return_new_display_text("=");
            Actual.Should().Be("0", "weil wir noch keine Ziffern-Taste gedrückt haben");
        }

        [TestMethod()]
        public void Should_add_on_plus_pressure()
        {
            Actual = Objekt.Process_key_pressure_and_return_new_display_text("1");
            Actual = Objekt.Process_key_pressure_and_return_new_display_text("2");
            Actual.Should().Be("12", "weil wir Eins und Zwei gedrückt haben");
            Actual = Objekt.Process_key_pressure_and_return_new_display_text("+");
            Actual = Objekt.Process_key_pressure_and_return_new_display_text("4");
            Actual = Objekt.Process_key_pressure_and_return_new_display_text("=");
            Actual.Should().Be("16", "weil wir Eins und Zwei gedrückt haben");
        }
    }


    /// <summary>
    /// Rechenlogik für den Taschenrechner
    /// </summary>
    public class DummyCalculator : ICalculator
    {
        public string Value { get { return _Value.ToString(); } set { _Value = Convert.ToInt32(value); } }
        public string PreviousValue { get { return _PreviousValue.ToString(); } set { _PreviousValue = Convert.ToInt32(value); } }

        private int _Value;
        private int _PreviousValue;


        public void Add() {} // Dummy Implementation
        public void Subtract() {} // Dummy Implementation
        public int Add(int zahla, int zahlb) { return 0; } // Dummy Implementation
        public int Subtract(int zahla, int zahlb) { return 0; } // Dummy Implementation

        public void InsertDigitInDisplay(int digit) // Correct Implementation, needed for Unit testing.
        {
            _Value = _Value * 10 + digit;
        }
    }

}
