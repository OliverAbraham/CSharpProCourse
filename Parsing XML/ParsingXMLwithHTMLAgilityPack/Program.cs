using AngleSharp;
using AngleSharp.Xml;

Console.WriteLine("Parsing XML with the 'AngleSharp' library");


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


//Load the string into AngleSharp using a stream
var config = Configuration.Default.WithXml(); // from AngleSharp.Xml
var context = BrowsingContext.New(config);
var document = await context.OpenAsync(req => req.Content(testfile));

// find and fetch tag "MyProperty"
var elements = document.GetElementsByTagName("MyDocument");
var propertyNodes = elements[0].GetElementsByTagName("MyProperty");
var myProperty = propertyNodes[0];

Console.WriteLine($"The single property: {myProperty.InnerHtml}");




// find and fetch the list:
var listNodes = elements[0].GetElementsByTagName("MyList");
var listItems = listNodes[0].GetElementsByTagName("MyListItem");
var listItemValues = listItems.Select(x => x.InnerHtml);
var listItemsAsString = string.Join(',', listItemValues);

Console.WriteLine($"The list items     : {listItemsAsString}");
