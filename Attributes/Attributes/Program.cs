namespace Attributes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Zugriff auf Attribute mit Reflektion");

            var attributes = Attribute.GetCustomAttributes(typeof(SomeClass));  // Reflection.  
            Console.WriteLine($"Attributes of SomeClass:");  
            foreach (Attribute attr in attributes)
                Console.WriteLine(attr);

            //var instance = new SomeClass();
            //attributes = Attribute.GetCustomAttributes(instance.Amount);  // Reflection.  
            //foreach (Attribute attr in attributes)  
            //{  
            //    if (attr is MyName)  
            //    {  
            //        Author a = (Author)attr;  
            //        System.Console.WriteLine("   {0}, version {1:f}", a.GetName(), a.version);  
            //    }  
            //}  
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