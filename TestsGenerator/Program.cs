using TestGeneratorLibrary;

namespace ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new TestGeneratorConfig(3, 6, 3,
                new List<string>()
                {
                    @"C:\Users\Veronika\Documents\work\5 сем\СПП\LR4\TestsGenerator\ExampleClassesLibrary\Class2.cs",
                    @"C:\Users\Veronika\Documents\work\5 сем\СПП\LR4\TestsGenerator\ExampleClassesLibrary\Class1.cs",
                }, @"C:\Users\Veronika\Documents\work\5 сем\СПП\LR4\TestsGenerator\result");

            var generator = new TestGenerator(config);

            generator.Generate().Wait();
        }
    }
}