using System;
using System.Collections.Generic;
using System.Text;

namespace GenericLogic
{
    interface ICrud
    {
        int GetPrimaryKey();
        string GetCompleteRow();
        string GetAlternateKey();

        void ConvertLineToDataRow(string line);
    }
}
