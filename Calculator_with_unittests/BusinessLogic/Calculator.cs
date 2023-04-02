using Contracts;

namespace BusinessLogic
{
    /// <summary>
    /// Math logic for the calculator
    /// </summary>
    public class Calculator : ICalculator
    {
        public string Value { get { return _value.ToString(); } set { _value = Convert.ToInt32(value); } }
        public string PreviousValue { get { return _previousValue.ToString(); } set { _previousValue = Convert.ToInt32(value); } }

        private int _value;
        private int _previousValue;


        public void Add()
        {
            _value = Add(_previousValue, _value);
        }

        public void Subtract()
        {
            _value = Subtract(_previousValue, _value);
        }

        public int Add(int numbera, int numberb)
        {
            return numbera + numberb;
        }

        public int Subtract(int numbera, int numberb)
        {
            return numbera - numberb;
        }

        public void InsertDigitInDisplay(int digit)
        {
            _value = _value * 10 + digit;
        }
    }
}
