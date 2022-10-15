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
                    @"C:\Users\Veronika\Documents\work\5 сем\СПП\LR2\Faker\FakerLibrary\Constructor.cs",
                    @"C:\Users\Veronika\Documents\work\5 сем\СПП\LR2\Faker\FakerLibrary\CycleError.cs",
                    @"C:\Users\Veronika\Documents\work\5 сем\СПП\LR2\Faker\FakerLibrary\Faker.cs",
                    @"C:\Users\Veronika\Documents\work\5 сем\СПП\LR2\Faker\FakerLibrary\Generator.cs",
                }, @"C:\Users\Veronika\Documents\work\5 сем\СПП\LR4\TestsGenerator\result");

            var generator = new TestGenerator(config);

            generator.Generate().Wait();
        }
    }
}