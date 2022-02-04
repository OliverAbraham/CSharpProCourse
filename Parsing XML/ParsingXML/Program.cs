using System.Text;
using System.Xml;
using System.Xml.XPath;

Console.WriteLine("Parsing XML with the .NET Framework library's built-in class 'XPathExpression' ");


var testfile = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<MyDocument xmlns=""JUST_ANY_NAMESPACE"">
  <MyProperty>Abc</MyProperty>
  <MyAttributeProperty value=""123"" />
  <MyList>
    <MyListItem>1</MyListItem>
    <MyListItem>2</MyListItem>
    <MyListItem>3</MyListItem>
  </MyList>
</MyDocument>
";


//Load the string into XPathDocument using a stream
var stream = new MemoryStream(Encoding.UTF8.GetBytes(testfile));
var xPath = new XPathDocument(stream);
var navigator = xPath.CreateNavigator();

//Compile the query with a namespace prefix. 
XPathExpression query = navigator.Compile("ns:MyDocument/ns:MyProperty");

//Do some BS to get the default namespace to actually be called ns. 
var nameSpace = new XmlNamespaceManager(navigator.NameTable);
nameSpace.AddNamespace("ns", "JUST_ANY_NAMESPACE");
query.SetContext(nameSpace);
var singleNode = navigator.SelectSingleNode(query).Value;

Console.WriteLine($"The single property: {singleNode}");



// reading the list:
query = navigator.Compile("ns:MyDocument/ns:MyProperty/ns:MyList");
nameSpace = new XmlNamespaceManager(navigator.NameTable);
nameSpace.AddNamespace("ns", "JUST_ANY_NAMESPACE");
query.SetContext(nameSpace);
var listOfNodes = navigator.SelectDescendants("MyListItem", "JUST_ANY_NAMESPACE", matchSelf:false);
var values = "";
foreach (var item in listOfNodes)
    values += $"{item},";

Console.WriteLine($"The list items     : {values}");


Console.WriteLine("\n\n\n");

