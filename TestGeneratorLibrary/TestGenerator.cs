using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestGeneratorLibrary
{
    public class TestGenerator
    {
        private readonly TestGeneratorConfig config;

        public TestGenerator(TestGeneratorConfig config)
        {
            this.config = config;

            if (!Directory.Exists(config.SavePath))
            {
                Directory.CreateDirectory(config.SavePath);
            }
        }

        public Task Generate()
        {
            var readFiles = new TransformBlock<string, string>(async path => await ReadFileAsync(path),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = config.MaxFilesReadingParallel
                }
                );

            var generateTestsByFile = new TransformManyBlock<string, string>
            (
              async data => await GenerateTestClasses(data),
              new ExecutionDataflowBlockOptions
              {
                  MaxDegreeOfParallelism = config.MaxTestClassesGeneratingParallel
              }
              );

            var writeFile = new ActionBlock<string>
           (
               async data => await WriteFileAsync(data),
               new ExecutionDataflowBlockOptions
               {
                   MaxDegreeOfParallelism = config.MaxFilesWritingParallel
               }
           );

            readFiles.LinkTo(generateTestsByFile, new DataflowLinkOptions { PropagateCompletion = true });
            generateTestsByFile.LinkTo(writeFile, new DataflowLinkOptions { PropagateCompletion = true });

            foreach (var path in config.FilesPaths)
            {
                readFiles.Post(path);
            }
            readFiles.Complete();

            return writeFile.Completion;
        }

        private async Task<string> ReadFileAsync(string path){
            return await File.ReadAllTextAsync(path);
        }

        private async Task WriteFileAsync(string data)
        {
            // var tree = CSharpSyntaxTree.ParseText(data);
            //  var fileName = (await tree.GetRootAsync())
            //      .DescendantNodes().OfType<ClassDeclarationSyntax>()
            //     .First().Identifier.Text;
            // var filePath = Path.Combine(_config.SavePath, $"{fileName}.cs");
            var fileName = data;
            var filePath = Path.Combine(config.SavePath, $"{fileName}.cs");

            await File.WriteAllTextAsync(filePath, data);
        }

        private async Task<string[]> GenerateTestClasses(string fileText)
        {
            var root = CSharpSyntaxTree.ParseText(fileText).GetCompilationUnitRoot();
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            var result = new List<string>();

            foreach (var class_ in classes)
            {
                result.Add(class_.Identifier.Text/*await GenerateTestClass(class_, root)*/);
            }

            return result.ToArray();
        }

    }
}
