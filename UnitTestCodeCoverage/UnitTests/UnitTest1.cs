using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZuTestendeKlassenbibliothek;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Method1_Test1()
        {
            int Ergebnis = Class1.Methode1(3);
            int Erwartet = 6;
            Assert.AreEqual(Ergebnis, Erwartet);
        }

        [TestMethod]
        public void Method1_Test2()
        {
            int Ergebnis = Class1.Methode1(20);
            int Erwartet = 60;
            Assert.AreEqual(Ergebnis, Erwartet);
        }
    }
}
