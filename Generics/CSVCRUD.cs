using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Enumeration;
using System.Text;
using System.Xml.Linq;

namespace GenericLogic
{
    class CSVCRUD<T>  where T:ICrud, new()
    {
        public T Row;

        public void Save(T row)
        {
            Row = row;
            var test = Row.ToString();

            int PrimaryKey = Row.GetPrimaryKey();
            string AlternateKey = Row.GetAlternateKey();

            string MyRowAsString = Row.GetCompleteRow();
            File.AppendAllText("demo.csv", MyRowAsString + "\n");
        }

        internal List<T> ReadFile(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            List<T> Result = new List<T>();
            foreach (var line in lines)
            {
                T dataRow = new T();
                dataRow.ConvertLineToDataRow(line);
                Result.Add(dataRow);
            }

            return Result;
        }
    }
}
