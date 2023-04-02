using BusinessLogic;
using FluentAssertions;

namespace Geschäftslogik.Tests
{
	[TestClass()]
    public class BusinessLogicTests
    {
        /// <summary>
        /// Tests if the addition works
        /// Expected reaction: returns the sum of the operands
        /// </summary>
        [TestMethod()]
        public void Should_add_correctly1()
        {
            var sut = new Calculator();

            int actual = sut.Add(3,4);

            int expected = 7;
            actual.Should().Be(expected, "this is the sum");
        }

        [TestMethod()]
        public void Should_add_correctly2()
        {
            var sut = new Calculator();
            sut.Value = "7";
            sut.PreviousValue = "4";

            sut.Add();

            sut.Value.Should().Be("11", "this is the sum of 7 and 4");
        }

        /// <summary>
        /// Tests if the subtraction works
        /// Expected reaction: returns the difference of the operands
        /// </summary>
        [TestMethod()]
        public void Should_subtract_correctly1()
        {
            var sut = new Calculator();

            int actual = sut.Subtract(3, 4);

            int expected = -1;
            actual.Should().Be(expected, "this is the difference");
        }

        [TestMethod()]
        public void Should_subtract_correctly2()
        {
            var sut = new Calculator();
            sut.Value = "4";
            sut.PreviousValue = "7";

            sut.Subtract();

            sut.Value.Should().Be("3", "this is the difference of 7 minus 4");
        }

        [TestMethod()]
        public void Should_insert_digit_correctly()
        {
            var sut = new Calculator();
            sut.Value.Should().Be("0", "this is the start value");
            
            sut.InsertDigitInDisplay(3);
            sut.Value.Should().Be("3", "this is the value after pressing 3");
            
            sut.InsertDigitInDisplay(4);
            sut.Value.Should().Be("34", "this is the value after pressing 3");
            
            sut.InsertDigitInDisplay(5);
            sut.Value.Should().Be("345", "this is the value after pressing 3");
        }
    }
}

