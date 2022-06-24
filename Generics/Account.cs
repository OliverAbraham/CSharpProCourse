using System;
using System.Collections.Generic;
using System.Text;

namespace GenericLogic
{
    class Account : ICrud
    {
        public int MyPrimaryKey;

        public void ConvertLineToDataRow(string line)
        {
            throw new NotImplementedException();
        }

        public string GetAlternateKey()
        {
            throw new NotImplementedException();
        }

        public string GetCompleteRow()
        {
            throw new NotImplementedException();
        }

        public int GetPrimaryKey()
        {
            return MyPrimaryKey;
        }
    }
}
