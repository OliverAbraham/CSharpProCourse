using FluentAssertions;
using Contracts;

namespace UserInterfaceLogic.Tests
{
    [TestClass()]
    public class UserInterfaceLogicTests
    {
        private string actual;
        private CalculatorUI sut;

        private void Setup()
        {
            var engineMock = new CalculatorMock();
            sut = new CalculatorUI(engineMock);
        }

        [TestMethod()]
        public void Should_combine_keys_correctly()
        {
            Setup();

            actual = sut.Process_key_pressure_and_return_new_display_text("0");
            actual.Should().Be("0", "we pressed the null");

            actual = sut.Process_key_pressure_and_return_new_display_text("1");
            actual.Should().Be("1", "we pressed the one");

            actual = sut.Process_key_pressure_and_return_new_display_text("2");
            actual.Should().Be("12", "we pressed one and two");

            actual = sut.Process_key_pressure_and_return_new_display_text("3");
            actual.Should().Be("123", "we pressed one, two and three");
        }

        [TestMethod()]
        public void Should_process_equal_key_correctly1()
        {
            Setup();
            actual = sut.Process_key_pressure_and_return_new_display_text("=");
            actual.Should().Be("0", "we didn't press any digit key");
        }

        [TestMethod()]
        public void Should_process_equal_key_correctly2()
        {
            Setup();
            actual = sut.Process_key_pressure_and_return_new_display_text("\r");
            actual.Should().Be("0", "we didn't press any digit key");
        }

        [TestMethod()]
        public void Should_handle_invalid_operator()
        {
            Setup();
            FluentActions.Invoking(
                delegate 
                {
                    sut.Process_key_pressure_and_return_new_display_text("*");
                })
            .Should().Throw<NotImplementedException>();
        }

        [TestMethod()]
        public void Should_call_add_on_plus_pressure()
        {
            Setup();

            actual = sut.Process_key_pressure_and_return_new_display_text("1");
            actual = sut.Process_key_pressure_and_return_new_display_text("2");
            actual.Should().Be("12", "we pressed one and two");

            actual = sut.Process_key_pressure_and_return_new_display_text("+");
            actual.Should().Be("0", "we pressed an operator key");

            actual = sut.Process_key_pressure_and_return_new_display_text("4");
            actual.Should().Be("4", "we pressed 4");

            actual = sut.Process_key_pressure_and_return_new_display_text("=");
            actual.Should().Be("7777", "this is the result of our calculator engine");
        }

        [TestMethod()]
        public void Should_call_subtract_on_minus_pressure()
        {
            Setup();

            actual = sut.Process_key_pressure_and_return_new_display_text("1");
            actual = sut.Process_key_pressure_and_return_new_display_text("2");
            actual.Should().Be("12", "we pressed one and two");

            actual = sut.Process_key_pressure_and_return_new_display_text("-");
            actual = sut.Process_key_pressure_and_return_new_display_text("4");
            actual = sut.Process_key_pressure_and_return_new_display_text("=");
            actual.Should().Be("8888", "we subtracted 4 from 12");
        }
    }


    /// <summary>
    /// Dummy implementation for testing
    /// </summary>
    public class CalculatorMock : ICalculator
    {
        public string Value { get { return _Value.ToString(); } set { _Value = Convert.ToInt32(value); } }
        public string PreviousValue { get { return _PreviousValue.ToString(); } set { _PreviousValue = Convert.ToInt32(value); } }

        private int _Value;
        private int _PreviousValue;


        public void Add() { _Value = 7777; } // Dummy Implementation
        public void Subtract() { _Value = 8888; } // Dummy Implementation
        public int Add(int zahla, int zahlb) { return 0; } // Dummy Implementation
        public int Subtract(int zahla, int zahlb) { return 0; } // Dummy Implementation

        public void InsertDigitInDisplay(int digit) // Correct Implementation, needed for Unit testing.
        {
            _Value = _Value * 10 + digit;
        }
    }

}
