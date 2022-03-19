using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberToLcd
{
    public class NumberToLcdConverter
    {
        public string[] OutputLines { get { return _OutputLines; } }

        private string[] _OutputLines = new string[5];

        public void Convert(int digit)
        {
            switch (digit)
            {
                case 0: 
                    _OutputLines[0] = " - ";
                    _OutputLines[1] = "| |";
                    _OutputLines[2] = "   ";
                    _OutputLines[3] = "| |";
                    _OutputLines[4] = " - ";
                    break;
                case 1: 
                    _OutputLines[0] = "   ";
                    _OutputLines[1] = "  |";
                    _OutputLines[2] = "   ";
                    _OutputLines[3] = "  |";
                    _OutputLines[4] = "   ";
                    break;
                case 2: 
                    _OutputLines[0] = " - ";
                    _OutputLines[1] = "  |";
                    _OutputLines[2] = " - ";
                    _OutputLines[3] = "|  ";
                    _OutputLines[4] = " - ";
                    break;
                case 3: 
                    _OutputLines[0] = " - ";
                    _OutputLines[1] = "  |";
                    _OutputLines[2] = " - ";
                    _OutputLines[3] = "  |";
                    _OutputLines[4] = " - ";
                    break;
                case 4: 
                    _OutputLines[0] = "   ";
                    _OutputLines[1] = "| |";
                    _OutputLines[2] = " - ";
                    _OutputLines[3] = "  |";
                    _OutputLines[4] = "   ";
                    break;
                case 5: 
                    _OutputLines[0] = " - ";
                    _OutputLines[1] = "|  ";
                    _OutputLines[2] = " - ";
                    _OutputLines[3] = "  |";
                    _OutputLines[4] = " - ";
                    break;
                case 6: 
                    _OutputLines[0] = " - ";
                    _OutputLines[1] = "|  ";
                    _OutputLines[2] = " - ";
                    _OutputLines[3] = "| |";
                    _OutputLines[4] = " - ";
                    break;
                case 7: 
                    _OutputLines[0] = " - ";
                    _OutputLines[1] = "  |";
                    _OutputLines[2] = "   ";
                    _OutputLines[3] = "  |";
                    _OutputLines[4] = "   ";
                    break;
                case 8: 
                    _OutputLines[0] = " - ";
                    _OutputLines[1] = "| |";
                    _OutputLines[2] = " - ";
                    _OutputLines[3] = "| |";
                    _OutputLines[4] = " - ";
                    break;
                case 9: 
                    _OutputLines[0] = " - ";
                    _OutputLines[1] = "| |";
                    _OutputLines[2] = " - ";
                    _OutputLines[3] = "  |";
                    _OutputLines[4] = " - ";
                    break;
            }
        }
    }
}
