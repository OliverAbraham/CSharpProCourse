using System.Text;

namespace FileLocking
{
	class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Opening the file....");
            string path = "Test.CSV";

            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) 
            {
                Console.WriteLine("File is now open, reading the file....");
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b,0,b.Length) > 0) 
                {
                    Console.WriteLine(temp.GetString(b));
                }

                Console.WriteLine("Writing at the end of the file....");
                Byte[] info = new UTF8Encoding(true).GetBytes("This is some more text in the file.\r\n");
                // Add some information to the file.
                fs.Write(info, 0, info.Length);

                Console.WriteLine("file is still open, press any key to continue");
                Console.ReadKey();
            }
        
            Console.WriteLine("file is now closed, press any key to exit");
            Console.ReadKey();
        }



        // Equivalent to the "using" block: This is what it does
        //FileStream fs;
        //try
        //{
        //    fs = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) 
        //}
        //finally
        //{
        //    fs.Close();
        //}
    }
}


