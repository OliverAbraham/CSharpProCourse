using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

Console.WriteLine("Testing my 'GetYearFromDate'");

BenchmarkRunner.Run<Benchmarks>();


[MemoryDiagnoser]
[OrdererAttribute(SummaryOrderPolicy.FastestToSlowest)]
[RankColumnAttribute]
public class Benchmarks
{
    private const string _input = "2022-12-24T13:59:59Z";
    private MyClassIWantToTest parser = new();

    [Benchmark]
    public void GetYearFromDateTime()
    {
        parser.GetYearFromDate(_input);
    }
}

public class MyClassIWantToTest
{
    // input should be in format "2022-12-24T13:59:59Z"
    public int GetYearFromDate(string dateTimeAsString)
    {
        var datetime = DateTime.Parse(dateTimeAsString);
        return datetime.Year;
    }
}