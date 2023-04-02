using Contracts;

namespace UserInterfaceLogic
{
    /// <summary>
    /// User interface for the calculator
    /// </summary>
    public class CalculatorUI
    {
        private string _previousOperator;
        private ICalculator _calculator;

        public CalculatorUI(ICalculator calculator_implementation)
        {
            _calculator = calculator_implementation;
        }

        /// <summary>
        /// Gets the code of the key pressed and returns the next display value
        /// </summary>
        public string Process_key_pressure_and_return_new_display_text(string key)
        {
            bool keyIsADigit = int.TryParse(key, out int Zahl);
            if (keyIsADigit)
                return ProcessDigit(Zahl);
            else 
                return ProcessOperatorKey(key);
        }

        private string ProcessDigit(int digit)
        {
            _calculator.InsertDigitInDisplay(digit);
            return _calculator.Value;
        }

        /// <summary>
        /// For operators, save the operator and the previous value.
        /// For Result key, apply the operator and return the result
        /// </summary>
        private string ProcessOperatorKey(string key)
        {
            if (key == "+" || key == "-")
            {
                _calculator.PreviousValue = _calculator.Value;
                _calculator.Value = "0";
                _previousOperator = key;
                return _calculator.Value.ToString();
            }

            if (key == "=" || key == "\r")
            {
                if (_previousOperator == "+")
                    _calculator.Add();
                if (_previousOperator == "-")
                    _calculator.Subtract();

                _calculator.PreviousValue = _calculator.Value;
                return _calculator.Value.ToString();
            }
            throw new NotImplementedException();
        }
    }
}
