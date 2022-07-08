using System;

namespace Geschäftslogik
{
    public interface ICalculator
    {
        void Add();
        void Subtract();
        void InsertDigitInDisplay(int digit);
        string Value { get; set; }
        string PreviousValue { get; set; }
    }

    /// <summary>
    /// Rechenlogik für den Taschenrechner
    /// </summary>
    public class Calculator : ICalculator
    {
        public string Value { get { return _Value.ToString(); } set { _Value = Convert.ToInt32(value); } }
        public string PreviousValue { get { return _PreviousValue.ToString(); } set { _PreviousValue = Convert.ToInt32(value); } }

        private int _Value;
        private int _PreviousValue;


        public void Add()
        {
            _Value = Add(_PreviousValue, _Value);
        }

        public void Subtract()
        {
            _Value = Subtract(_PreviousValue, _Value);
        }

        public int Add(int zahla, int zahlb)
        {
            return zahla + zahlb;
        }

        public int Subtract(int zahla, int zahlb)
        {
            return zahla - zahlb;
        }

        public void InsertDigitInDisplay(int digit)
        {
            _Value = _Value * 10 + digit;
        }
    }
}
