using System.Linq.Expressions;

namespace CSharpNeuigkeiten_8_9_10
{
    internal class LambdaExpressions
    {
        delegate int Quadrierer(int x);

        public static void Demo()
        {
            Console.WriteLine("Lambda expressions 1:");
            Quadrierer QuadriereDieZahl = (x => x * x);
            Console.WriteLine(QuadriereDieZahl(3));

            Console.WriteLine("Lambda expressions 2:");
            var einPaarZahlen = new int[] { 2, 3, 4, 5, 6, 7 };
            var zahlenGrößerAlsFünf = einPaarZahlen.Where(x => x > 5).ToList();
            Console.WriteLine(string.Join(' ', zahlenGrößerAlsFünf));

            Console.WriteLine("Lambda expressions 3:");
            var personen = new Person[]
            {
                new Person("Hans", 10),
                new Person("Paul", 15),
                new Person("Mary", 30),
            };
            var kinder = personen.Where(x => x.Alter < 18).Select(x => x.Name).ToList();
            Console.WriteLine(string.Join(' ', kinder));
        }
    }
}
