using ExcelDataReader;
using System;
using System.IO;

namespace Excel_XLSX_Library
{
    // read this:
    // https://github.com/ExcelDataReader/ExcelDataReader

    // Important note on .NET Core
    // By default, ExcelDataReader throws a NotSupportedException "No data is available for encoding 1252." 
    // on .NET Core.
    // 
    // To fix, add a dependency to the package System.Text.Encoding.CodePages and then add code to register 
    // the code page provider during application initialization (f.ex in Startup.cs):
    // System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
    // 
    // This is required to parse strings in binary BIFF2-5 Excel documents encoded with DOS-era code pages. 
    // These encodings are registered by default in the full .NET Framework, but not on .NET Core.

	class Program
	{
		static void Main(string[] args)
		{
            var filename = "Demodatei.xlsx";
			Console.WriteLine($"Reading demo file 'filename' and extracting some cell values");

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    //// --------------------- VARIANT 1 ------------------------------
                    // 1. Use the reader methods
                    do
                    {
                        while (reader.Read())
                        {
                            var txtA = reader.GetString(0);
                            var txtB = reader.GetString(1);
							Console.WriteLine($"Spalte A: {txtA,20}   B: {txtB,20}");
                        }
                    } while (reader.NextResult());


                    //// --------------------- VARIANT 2 ------------------------------
                    //// 2. Use the AsDataSet extension method
                    //var result = reader.AsDataSet();
                    //// The result of each spreadsheet is in result.Tables
                }
            }
		}
	}
}
