using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinesweeperLogic;

namespace MinesweeperSpickzettel
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.GetLength(0) < 2)
            {
                Usage();
                return;
            }

            try
            {
			    string Input_filename = args[0];
			    string Output_filename = args[1];
                var Engine = new MinesweeperEngine();
                Engine.Read_playground_from_file(Input_filename);
                Engine.Create_cheatsheet();
                Engine.Write_cheatsheet_to_disk(Output_filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error loading or writing files");
                Console.WriteLine(ex.ToString());
            }
		}

        private static void Usage()
        {
            Console.WriteLine(@"
Usage:
Minesweeper cheatsheet [input file] [output file]
");
        }
    }
}
