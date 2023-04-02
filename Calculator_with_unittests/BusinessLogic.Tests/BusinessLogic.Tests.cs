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
        public void Should_add_correctly()
        {
            var sut = new Calculator();

            int actual = sut.Add(3,4);

            int expected = 7;
            actual.Should().Be(expected, "this is the sum");
        }


        /// <summary>
        /// Tests if the subtraction works
        /// Expected reaction: returns the difference of the operands
        /// </summary>
        [TestMethod()]
        public void Should_subtract_correctly()
        {
            var sut = new Calculator();

            int actual = sut.Subtract(3, 4);

            int expected = -1;
            actual.Should().Be(expected, "this is the difference");
        }


        ///// <summary>
        ///// Tests if the multiplication works
        ///// Expected reaction: returns the product of the operands
        ///// </summary>
        //[TestMethod()]
        //public void Should_multiply_correctly()
        //{
        //    var sut = new Calculator();
        //
        //    int actual = sut.Multiply(3,4);
        //
        //    int expected = 12;
        //    actual.Should().Be(expected, "weil 3 mal 4 nunmal 12 ergibt");
        //}

    }
}

