using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TestGeneratorLibrary;

namespace ProjectCheckFunctionalityTests
{
    [TestClass]
    public class UnitTest
    {
        private readonly string _writePath = @"C:\Users\Veronika\Documents\work\5 ñåì\ÑÏÏ\LR4\TestsGenerator\result";
        private readonly string _testClass1Path = @"C:\Users\Veronika\Documents\work\5 ñåì\ÑÏÏ\LR4\TestsGenerator\ExampleClassesLibrary\Class1.cs";
        private readonly string _testClass2Path = @"C:\Users\Veronika\Documents\work\5 ñåì\ÑÏÏ\LR4\TestsGenerator\ExampleClassesLibrary\Class2.cs";

        [TestMethod]
        public void Analyze_GeneratingTests_UsingsCorrect()
        {
            var config = new TestGeneratorConfig(3, 3, 3,
                new List<string>()
                {
                    _testClass2Path
                }, _writePath);
            var generator = new TestGenerator(config);
            var expected = new List<string>()
            {
                "ExampleClassesLibrary",
                "System",
                "System.Collections.Generic",
                "System.Linq",
                "System.Text",
                "System.Threading.Tasks"
            };

            generator.Generate().Wait();
            var actual = CSharpSyntaxTree
                .ParseText(File.ReadAllText(Path.Combine(_writePath, "Class2Tests.cs")))
                .GetCompilationUnitRoot().Usings.Select(syntax => syntax.Name.ToString());

            CollectionAssert.AreEqual(expected, actual.ToList<string>());
        }

        [TestMethod]
        public void Analyze_GeneratingTests_NamespaceCorrect()
        {
            var config = new TestGeneratorConfig(3, 3, 3,
                new List<string>()
                {
                    _testClass1Path
                }, _writePath);
            var generator = new TestGenerator(config);
            var expected = "ExampleClassesLibrary.Tests";

            generator.Generate().Wait();
            var actual = CSharpSyntaxTree
                .ParseText(File.ReadAllText(Path.Combine(_writePath, "Class1Tests.cs")))
                .GetCompilationUnitRoot().DescendantNodes().OfType<NamespaceDeclarationSyntax>().First().Name.ToString();

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void Analyze_GeneratingTests_ClassCorrect()
        {
            var config = new TestGeneratorConfig(3, 3, 3,
                 new List<string>()
                 {
                    _testClass1Path
                 }, _writePath);
            var generator = new TestGenerator(config);
            var expected = "Class1Tests";

            generator.Generate().Wait();
            var actual = CSharpSyntaxTree
                .ParseText(File.ReadAllText(Path.Combine(_writePath, "Class1Tests.cs")))
                .GetCompilationUnitRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();

            //Assert.Single(actual);

            Assert.AreEqual(expected, actual.First().Identifier.Text);
        }
        
        [TestMethod]
        public void Analyze_GeneratingTests_MethodsNamesCorrect()
        {
            var config = new TestGeneratorConfig(3, 3, 3,
                  new List<string>()
                  {
                    _testClass1Path
                  }, _writePath);
            var generator = new TestGenerator(config);
            var expected = new List<string>()
            {
                "FirstMethodTest",
                "SecondMethodTest",
                "ThirdMethodTest",
                "ThirdMethodTest1",
            };

            generator.Generate().Wait();
            var actual = CSharpSyntaxTree
                .ParseText(File.ReadAllText(Path.Combine(_writePath, "Class1Tests.cs")))
                .GetCompilationUnitRoot().DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Select(syntax => syntax.Identifier.Text);
            
            CollectionAssert.AreEqual(expected, actual.ToList<string>());
        }
    }
}