using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackRechner;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        // 1 + 2 = 3
        [TestMethod]
        public void TestMethod1()
        {
            Rechner Sut = new Rechner();
            Sut.Push("1");
            Sut.Push("2");
            Sut.Push("+");
            string Ergebnis = Sut.Berechne();
            Assert.AreEqual("3", Ergebnis);
        }

        // 1 + 2 * 3 = 7
        [TestMethod]
        public void TestMethod2()
        {
            Rechner Sut = new Rechner();
            Sut.Push("1");
            Sut.Push("2");
            Sut.Push("+");
            Sut.Push("3");
            Sut.Push("*");
            string Ergebnis = Sut.Berechne();
            Assert.AreEqual("7", Ergebnis);
        }

        // 1 * 2 + 3 = 5
        [TestMethod]
        public void TestMethod3()
        {
            Rechner Sut = new Rechner();

            Sut.Push("1");
            Sut.Push("2");
            Sut.Push("*");
            Sut.Push("3");
            Sut.Push("+");
            string Ergebnis = Sut.Berechne();
            Assert.AreEqual("5", Ergebnis);
        }
    }
}
