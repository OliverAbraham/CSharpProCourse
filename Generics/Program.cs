using System;
using System.Collections.Generic;

namespace GenericLogic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            CSVCRUD<Company> Reader1 = new CSVCRUD<Company>();
            List<Company> MyData = Reader1.ReadFile("companies.csv");

            CSVCRUD<Company> Writer1 = new CSVCRUD<Company>();
            Writer1.Save(new Company());

            CSVCRUD<Account> Writer2 = new CSVCRUD<Account>();
            Writer2.Save(new Account());
        }
    }
}
