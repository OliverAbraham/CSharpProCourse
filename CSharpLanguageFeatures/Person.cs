namespace CSharpNeuigkeiten_8_9_10
{
    internal class Person
    {
        public string Name { get; set; }
        public int Alter { get; set; }

        public Person(string name, int alter)
        {
            Name = name;
            Alter = alter;
        }
    }
}