using Newtonsoft.Json.Linq;

namespace Serializing_to_JSON;

internal class ParsingJsonWithLinq
{
    public void Demo()
    {
        Console.WriteLine("Parsing JSON with Newtonsoft.Json and LINQ (edge case)");


        string json = @"{
              'channel': {
                'title': 'This is the document title',
                'link': 'https://www.oliver-abraham.de',
                'item': [
                  {
                    'title': 'Just an article 1',
                    'description': 'Just a description',
                    'categories': [ 'Json.NET', 'CodePlex' ]
                  },
                  {
                    'title': 'Just an article 2',
                    'description': 'Just a description',
                    'categories': [ 'Json.NET', 'LINQ' ]
                  }
                ]
              }
            }";


        var document = JObject.Parse(json);

        var rssTitle = (string)document["channel"]["title"];

        Console.WriteLine($"Title         : {rssTitle}");

        var itemTitle = (string)document["channel"]["item"][0]["title"];

        Console.WriteLine($"itemTitle     : {itemTitle}");

        var categories = (JArray)document["channel"]["item"][0]["categories"];
        var  categoriesText = categories.Select(c => (string)c).ToList();

        Console.WriteLine($"categoriesText: {string.Join(',', categoriesText)}");
    }
}
