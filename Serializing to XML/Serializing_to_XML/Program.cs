using System.Xml.Serialization;

Console.WriteLine("Serializing and deserializing data to and from XML");



var myData         = new MyData();
myData.Name        = "Oliver";
myData.Description = "A person on this planet";
myData.Amount      = 123.456m;

Console.WriteLine($"My data that I want to save:");
Console.WriteLine($"myData.Name        = '{myData.Name       }'");
Console.WriteLine($"myData.Description = '{myData.Description}'");
Console.WriteLine($"myData.Amount      = '{myData.Amount     }'");


Console.WriteLine($"\nWriting this data to a file");

var serializer = new XmlSerializer(typeof(MyData));
var stream = new FileStream("MyFilename.xml", FileMode.Create);
serializer.Serialize(stream, myData);
stream.Close();


Console.WriteLine($"\nReading data back from file");

stream = new FileStream("MyFilename.xml", FileMode.Open);
MyData myDataBack = (MyData)serializer.Deserialize(stream);
stream.Close();


Console.WriteLine($"\nThe result:");
Console.WriteLine($"myData.Name        = '{myDataBack.Name       }'");
Console.WriteLine($"myData.Description = '{myDataBack.Description}'");
Console.WriteLine($"myData.Amount      = '{myDataBack.Amount     }'");
Console.WriteLine($"Press a key to end");
Console.ReadKey();


public class MyData
{
    public string  Name        { get; set; }
    public string  Description { get; set; }
    public decimal Amount      { get; set; }
}

