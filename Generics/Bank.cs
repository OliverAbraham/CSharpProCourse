using System;
using System.Collections.Generic;
using System.Text;

namespace GenericLogic
{
    class Bank : ICrud
    {
        // PK
        public int PK;

        // Alternate Key
        public string Kennung;

        // Foreign key
        public int CompanyId;

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
            throw new NotImplementedException();
        }
    }
}
