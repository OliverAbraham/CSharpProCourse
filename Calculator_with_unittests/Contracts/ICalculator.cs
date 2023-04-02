namespace Contracts
{
    public interface ICalculator
    {
        void Add();
        void Subtract();
        void InsertDigitInDisplay(int digit);
        string Value { get; set; }
        string PreviousValue { get; set; }
    }
}
