using System.Reflection;

namespace Attributes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Zugriff auf Attribute mit Reflektion\n\n");


            Console.Write($"Here are the attributes of 'SomeClass': ");  
            var classAttributes = Attribute.GetCustomAttributes(typeof(SomeClass));  // Reflection.  
            foreach (var attribute in classAttributes)
                Console.WriteLine(attribute);


            var instance = new SomeClass();
            Console.WriteLine($"\n\nNow running over the Properties of '{instance.GetType().Name}'");
            var t = instance.GetType();
            var properties = t.GetProperties();

            foreach (var property in properties)  
            {  
                Console.WriteLine($"Here are the attributes of property '{property.Name}':");  
                //var value = property.GetValue(instance, new object[]{});
                var propertyAttributes = property.GetCustomAttributes(typeof(MyTranslation));
                foreach(var attribute in propertyAttributes)
                {
                    if (attribute is MyTranslation)
                    {
                        var myTranslation = attribute as MyTranslation;
                        Console.WriteLine($"   MyTranslation Language={myTranslation.Language} Text={myTranslation.Text}");
                    }
                }
            }  
        }
    }


    [AttributeUsage(AttributeTargets.Class)]  
    public class MyName : Attribute
    {
    }


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]  
    public class MyTranslation : Attribute
    {
        public string Language { get; set; }
        public string Text { get; set; }
    }


    [MyName]
    internal class SomeClass
    {
        [MyTranslation(Language = "DE", Text = "Betrag")]
        [MyTranslation(Language = "FR", Text = "Montant")]
        [MyTranslation(Language = "EN", Text = "Amount")]
        public int Amount { get; set; }
    }
}