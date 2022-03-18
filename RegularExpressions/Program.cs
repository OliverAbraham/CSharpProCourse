using System;
using System.Text.RegularExpressions;

namespace RegexTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestBIC("HYVEDEMM430");
            TestBIC("1HYVEDEMM430");
            TestBIC("1");
            TestBIC("");

            TestIBAN("DE39700500000003929278");
            TestIBAN("1DE39700500000003929278");
            TestIBAN("1");
            TestIBAN("");
            Console.ReadKey();
        }

        private static void TestBIC(string Test)
        {
            Console.WriteLine("Korrekte BIC:  {0} = {1}", Test, Is_BIC_valid(Test));
        }

        private static void TestIBAN(string Test)
        {
            Console.WriteLine("Korrekte IBAN: {0} = {1}", Test, Is_IBAN_valid(Test));
        }

        private static bool Is_BIC_valid(string bic)
        {
            return Regex.IsMatch(bic, "^[A-Z]{6,6}[A-Z2-9][A-NP-Z0-9]([A-Z0-9]{3,3}){0,1}$");
        }

        private static bool Is_IBAN_valid(string iban)
        {
            return Regex.IsMatch(iban, "^[A-Z]{2,2}[0-9]{2,2}[a-zA-Z0-9]{1,30}$");
        }
    }
}
