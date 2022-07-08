using System;
using Geschäftslogik;

namespace Taschenrechner
{
    /// <summary>
    /// User Interface für den Taschenrechner
    /// </summary>
    public class CalculatorUI
    {
        private string _PreviousOperator;
        private ICalculator _Calc;

        public CalculatorUI(ICalculator calculator_implementation)
        {
            _Calc = calculator_implementation;
        }

        /// <summary>
        /// Bekommt den Code der gedrückten Taste und liefert den nächsten Anzeigewert zurück
        /// </summary>
        public string Process_key_pressure_and_return_new_display_text(string taste)
        {
            if (int.TryParse(taste, out int Zahl))
                return ProcessDigit(Zahl);
            else 
                return ProcessOperatorKey(taste);
        }

        private string ProcessDigit(int digit)
        {
            _Calc.InsertDigitInDisplay(digit);
            return _Calc.Value;
        }

        private string ProcessOperatorKey(string key)
        {
            if (key == "+" || key == "-")
            {
                _Calc.PreviousValue = _Calc.Value;
                _Calc.Value = "0";
                _PreviousOperator = key;
                return _Calc.Value.ToString();
            }

            if (key == "=" || key == "\r")
            {
                if (_PreviousOperator == "+")
                    _Calc.Add();
                if (_PreviousOperator == "-")
                    _Calc.Subtract();

                _Calc.PreviousValue = _Calc.Value;
                return _Calc.Value.ToString();
            }
            throw new NotImplementedException();
        }
    }
}
