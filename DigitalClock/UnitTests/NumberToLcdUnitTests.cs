using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberToLcd;
using FluentAssertions;

namespace UnitTests
{
    [TestClass]
    public class NumberToLcdUnitTests
    {
        [TestMethod]
        public void Test_digit_0()
        {
            NumberToLcdConverter Sut = new NumberToLcdConverter();
            Sut.Convert(0);
            Sut.OutputLines[0].Should().Be(" - ");
            Sut.OutputLines[1].Should().Be("| |");
            Sut.OutputLines[2].Should().Be("   ");
            Sut.OutputLines[3].Should().Be("| |");
            Sut.OutputLines[4].Should().Be(" - ");
        }

        [TestMethod]
        public void Test_digit_1()
        {
            NumberToLcdConverter Sut = new NumberToLcdConverter();
            Sut.Convert(1);
            Sut.OutputLines[0].Should().Be("   ");
            Sut.OutputLines[1].Should().Be("  |");
            Sut.OutputLines[2].Should().Be("   ");
            Sut.OutputLines[3].Should().Be("  |");
            Sut.OutputLines[4].Should().Be("   ");
        }

        [TestMethod]
        public void Test_digit_2()
        {
            NumberToLcdConverter Sut = new NumberToLcdConverter();
            Sut.Convert(2);
            Sut.OutputLines[0].Should().Be(" - ");
            Sut.OutputLines[1].Should().Be("  |");
            Sut.OutputLines[2].Should().Be(" - ");
            Sut.OutputLines[3].Should().Be("|  ");
            Sut.OutputLines[4].Should().Be(" - ");
        }

        [TestMethod]
        public void Test_digit_3()
        {
            NumberToLcdConverter Sut = new NumberToLcdConverter();
            Sut.Convert(3);
            Sut.OutputLines[0].Should().Be(" - ");
            Sut.OutputLines[1].Should().Be("  |");
            Sut.OutputLines[2].Should().Be(" - ");
            Sut.OutputLines[3].Should().Be("  |");
            Sut.OutputLines[4].Should().Be(" - ");
        }

        [TestMethod]
        public void Test_digit_4()
        {
            NumberToLcdConverter Sut = new NumberToLcdConverter();
            Sut.Convert(4);
            Sut.OutputLines[0].Should().Be("   ");
            Sut.OutputLines[1].Should().Be("| |");
            Sut.OutputLines[2].Should().Be(" - ");
            Sut.OutputLines[3].Should().Be("  |");
            Sut.OutputLines[4].Should().Be("   ");
        }

        [TestMethod]
        public void Test_digit_5()
        {
            NumberToLcdConverter Sut = new NumberToLcdConverter();
            Sut.Convert(5);
            Sut.OutputLines[0].Should().Be(" - ");
            Sut.OutputLines[1].Should().Be("|  ");
            Sut.OutputLines[2].Should().Be(" - ");
            Sut.OutputLines[3].Should().Be("  |");
            Sut.OutputLines[4].Should().Be(" - ");
        }

        [TestMethod]
        public void Test_digit_6()
        {
            NumberToLcdConverter Sut = new NumberToLcdConverter();
            Sut.Convert(6);
            Sut.OutputLines[0].Should().Be(" - ");
            Sut.OutputLines[1].Should().Be("|  ");
            Sut.OutputLines[2].Should().Be(" - ");
            Sut.OutputLines[3].Should().Be("| |");
            Sut.OutputLines[4].Should().Be(" - ");
        }

        [TestMethod]
        public void Test_digit_7()
        {
            NumberToLcdConverter Sut = new NumberToLcdConverter();
            Sut.Convert(7);
            Sut.OutputLines[0].Should().Be(" - ");
            Sut.OutputLines[1].Should().Be("  |");
            Sut.OutputLines[2].Should().Be("   ");
            Sut.OutputLines[3].Should().Be("  |");
            Sut.OutputLines[4].Should().Be("   ");
        }

        [TestMethod]
        public void Test_digit_8()
        {
            NumberToLcdConverter Sut = new NumberToLcdConverter();
            Sut.Convert(8);
            Sut.OutputLines[0].Should().Be(" - ");
            Sut.OutputLines[1].Should().Be("| |");
            Sut.OutputLines[2].Should().Be(" - ");
            Sut.OutputLines[3].Should().Be("| |");
            Sut.OutputLines[4].Should().Be(" - ");
        }

        [TestMethod]
        public void Test_digit_9()
        {
            NumberToLcdConverter Sut = new NumberToLcdConverter();
            Sut.Convert(9);
            Sut.OutputLines[0].Should().Be(" - ");
            Sut.OutputLines[1].Should().Be("| |");
            Sut.OutputLines[2].Should().Be(" - ");
            Sut.OutputLines[3].Should().Be("  |");
            Sut.OutputLines[4].Should().Be(" - ");
        }
    }
}
