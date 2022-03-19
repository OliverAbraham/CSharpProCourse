using System;
using System.IO;

namespace MinesweeperLogic
{
    public class MinesweeperEngine
    {
        #region ------------- Properties ---------------------------------------------------------
        #endregion



        #region ------------- Fields --------------------------------------------------------------

        private int _Playground_dimensions;
        private bool[,] _Playground;
        private int[,] _Cheatsheet;

        #endregion



        #region ------------- Init ----------------------------------------------------------------
        #endregion



        #region ------------- Methods -------------------------------------------------------------
        
        public void Read_playground_from_file(string filename)
        {
            _Playground_dimensions = Detect_playground_dimensions_from_file(filename);
            if (_Playground_dimensions <= 0)
                throw new Exception("Playground dimensions could not be determined from input file.");

            Create_playground();

            Read_file_into_playground_array(filename);
        }

        public void Create_cheatsheet()
        {
            Create_cheatsheet_memory();
            Reset_cheatsheet();
            Fill_cheatsheet();
        }

        public void Write_cheatsheet_to_disk(string filename)
        {
            using (var fs = new FileStream(filename, FileMode.Append, FileAccess.Write))
            {
                //var writer = new StreamWriter(fs);
                //var writer = new StreamWriter(filename, true);
                using (var writer = new StreamWriter(fs, System.Text.Encoding.ASCII))
                {
                    for (int y = 0; y < _Playground_dimensions; y++)
                    {
                        for (int x = 0; x < _Playground_dimensions; x++)
                        {
                            string Cell = _Cheatsheet[x, y].ToString();
                            writer.Write(Cell);
                        }
                        writer.WriteLine();
                    }
                    writer.Close();
                }
                fs.Close();
            }
        }

        #endregion



        #region ------------- Methods for unit testing only ---------------------------------------
        
        public bool Get_playground_cell(int x, int y) => _Playground[x,y];
        public int Get_cheatsheet_cell(int x, int y) => _Cheatsheet[x,y];

        public void Create_cheatsheet_memory__for_unit_testing_only(int dimensions)
        {
            _Playground_dimensions = dimensions;
            Create_cheatsheet_memory();
        }

        public void Set_cheatsheet_cell(int x, int y, int value) 
        { 
            _Cheatsheet[x,y] = value;
        }

        #endregion



        #region ------------- Implementation ------------------------------------------------------
        
        private int Detect_playground_dimensions_from_file(string filename)
        {
            int horizontal_dimension = 0;

            using(var fs = new FileStream(filename, FileMode.Open))
            {
                var reader = new StreamReader(fs, System.Text.Encoding.ASCII, true);

                string line;
                if ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    horizontal_dimension = line.Length;
                }
            }
            return horizontal_dimension;
        }

        private void Read_file_into_playground_array(string filename)
        {
            using(var fs = new FileStream(filename, FileMode.Open))
            {
                var reader = new StreamReader(fs, System.Text.Encoding.ASCII, true);

                int yCoordinate = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    for (int xCoordinate = 0; xCoordinate < line.Length; xCoordinate++)
                    {
                        _Playground[xCoordinate, yCoordinate] = (line[xCoordinate] == '*');
                    }
                    yCoordinate++;
                }
            }
        }

        private void Create_playground()
        {
            _Playground = new bool[_Playground_dimensions, _Playground_dimensions];
        }

        private void Create_cheatsheet_memory()
        {
            _Cheatsheet = new int[_Playground_dimensions, _Playground_dimensions];
        }

        private void Reset_cheatsheet()
        {
            for (int y = 0; y < _Playground_dimensions; y++)
            {
                for (int x = 0; x < _Playground_dimensions; x++)
                {
                    _Cheatsheet[x, y] = 0;
                }
            }
        }

        /// <summary>
        /// Algorithm: Foreach each playground cell that contains a mine,
        /// increment this and the surrounding fields in the cheatsheet by 1
        /// </summary>
        private void Fill_cheatsheet()
        {
            for (int y = 0; y < _Playground_dimensions; y++)
            {
                for (int x = 0; x < _Playground_dimensions; x++)
                {
                    if (_Playground[x, y])
                    {
                        Increment_this_and_surrounding_fields(x, y);
                    }
                }
            }
        }

        private void Increment_this_and_surrounding_fields(int x, int y)
        {
            Increment_field_if_exists(x  ,y  ); // this
            Increment_field_if_exists(x-1,y-1); // upper left
            Increment_field_if_exists(x  ,y-1); // upper
            Increment_field_if_exists(x+1,y-1); // upper right
            Increment_field_if_exists(x-1,y  ); // left
            Increment_field_if_exists(x+1,y  ); // right
            Increment_field_if_exists(x-1,y+1); // lower left
            Increment_field_if_exists(x  ,y+1); // lower
            Increment_field_if_exists(x+1,y+1); // lower right
        }

        private void Increment_field_if_exists(int x, int y)
        {
            if (x >= 0 && 
                y >= 0 && 
                x < _Playground_dimensions && 
                y < _Playground_dimensions)
            {
                _Cheatsheet[x,y]++;
            }
        }

        #endregion
    }
}
