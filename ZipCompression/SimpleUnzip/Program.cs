using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abraham.Compression;

namespace SimpleUnzip
{
    class Program
    {
        static void Main(string[] args)
        {
            string Input_filename  = @"..\..\..\Testdata\Testarchiv.zip";
            string Output_filename = "Testfile.txt";

            using (var zip = ZipArchive.OpenOnFile(Input_filename))
            {
                var file = zip.GetFile(Output_filename);
                if (file.FolderFlag) 
                    return;
                var text = new StreamReader(file.GetStream()).ReadToEnd();
                File.WriteAllText("Testfile.txt", text);
                Console.WriteLine("File successfully unzipped");
            }
        }
    }
}