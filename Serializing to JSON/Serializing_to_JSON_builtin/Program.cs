using System.Text.Json;


Console.WriteLine("Serializing and deserializing to and from JSON (with built-in function)");


var myData         = new MyData();
myData.Name        = "Oliver";
myData.Description = "A person on this planet";
myData.Amount      = 123.456m;

Console.WriteLine($"My data that I want to save:");
Console.WriteLine($"myData.Name        = '{myData.Name       }'");
Console.WriteLine($"myData.Description = '{myData.Description}'");
Console.WriteLine($"myData.Amount      = '{myData.Amount     }'");

Console.WriteLine($"\nWriting this data to a file");

var jsonToFile = JsonSerializer.Serialize(myData);
File.WriteAllText("JsonSerializerDemo.dat", jsonToFile);


Console.WriteLine($"\nReading data back from file");

var jsonFromFile = File.ReadAllText("JsonSerializerDemo.dat");

Console.WriteLine($"\nConverting back to my class");

var myDataBack = JsonSerializer.Deserialize<MyData>(jsonFromFile);

Console.WriteLine($"\nThe result:");
Console.WriteLine($"myData.Name        = '{myDataBack.Name       }'");
Console.WriteLine($"myData.Description = '{myDataBack.Description}'");
Console.WriteLine($"myData.Amount      = '{myDataBack.Amount     }'");

public class MyData
{
    public string  Name        { get; set; }
    public string  Description { get; set; }
    public decimal Amount      { get; set; }
}

