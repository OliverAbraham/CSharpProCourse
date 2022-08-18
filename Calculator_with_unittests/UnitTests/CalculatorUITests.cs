using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Geschäftslogik;
using System;

namespace Taschenrechner.Tests
{
    [TestClass()]
    public class CalculatorUITests
    {
        private string actual;
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

            actual = Objekt.Process_key_pressure_and_return_new_display_text("0");
            actual.Should().Be("0", "we pressed the null");

            actual = Objekt.Process_key_pressure_and_return_new_display_text("1");
            actual.Should().Be("1", "we pressed the one");

            actual = Objekt.Process_key_pressure_and_return_new_display_text("2");
            actual.Should().Be("12", "we pressed one and two");

            actual = Objekt.Process_key_pressure_and_return_new_display_text("3");
            actual.Should().Be("123", "we pressed one, two and three");
        }

        [TestMethod()]
        public void Should_process_equal_key_correctly()
        {
            Setup();
            actual = Objekt.Process_key_pressure_and_return_new_display_text("=");
            actual.Should().Be("0", "we didn't press any digit key");
        }

        [TestMethod()]
        public void Should_add_on_plus_pressure()
        {
            actual = Objekt.Process_key_pressure_and_return_new_display_text("1");
            actual = Objekt.Process_key_pressure_and_return_new_display_text("2");
            actual.Should().Be("12", "we pressed one and two");

            actual = Objekt.Process_key_pressure_and_return_new_display_text("+");
            actual = Objekt.Process_key_pressure_and_return_new_display_text("4");
            actual = Objekt.Process_key_pressure_and_return_new_display_text("=");
            actual.Should().Be("16", "we added 12 and 4");
        }

        [TestMethod()]
        public void Should_subtract_on_minus_pressure()
        {
            actual = Objekt.Process_key_pressure_and_return_new_display_text("1");
            actual = Objekt.Process_key_pressure_and_return_new_display_text("2");
            actual.Should().Be("12", "we pressed one and two");

            actual = Objekt.Process_key_pressure_and_return_new_display_text("-");
            actual = Objekt.Process_key_pressure_and_return_new_display_text("4");
            actual = Objekt.Process_key_pressure_and_return_new_display_text("=");
            actual.Should().Be("8", "we subtracted 4 from 12");
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
