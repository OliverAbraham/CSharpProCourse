using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;

namespace GenericLogic
{
    class Company : ICrud
    {
        // PK
        public int ID;

        // AlternateKey
        public string Kennung;

        // Some other fields
        public string Description;
        public string Name;

        public void ConvertLineToDataRow(string line)
        {
            string[] parts     = line.Split(';');
            Company NewRow     = new Company();
            NewRow.ID          = Convert.ToInt32(parts[0]);
            NewRow.Kennung     = parts[1];
            NewRow.Description = parts[2];
            NewRow.Name        = parts[3];
        }

        public string GetAlternateKey()
        {
            return Kennung;
        }

        public string GetCompleteRow()
        {
            return ID.ToString() + ";" + Description + ";" + Name;
        }

        public int GetPrimaryKey()
        {
            return ID;
        }
    }
}
