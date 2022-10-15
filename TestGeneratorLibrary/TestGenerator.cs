﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace TestGeneratorLibrary
{
   // using static SyntaxFactory;
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

            //var generateTestsByFile = new TransformManyBlock<string, string>
            var generateTestsByFile = new TransformManyBlock<string, TestFile>
            (
              async data => await GenerateTestClasses(data),
              new ExecutionDataflowBlockOptions
              {
                  MaxDegreeOfParallelism = config.MaxTestClassesGeneratingParallel
              }
              );

            var writeFile = new ActionBlock<TestFile>
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

        private async Task WriteFileAsync(TestFile data)
        {
            //var tree = CSharpSyntaxTree.ParseText(data);
           /* var fileName = (await tree.GetRootAsync())
                  .DescendantNodes().OfType<ClassDeclarationSyntax>()
                 .First().Identifier.Text;*/
            // var filePath = Path.Combine(_config.SavePath, $"{fileName}.cs");
           // var fileName = data;
            var filePath = Path.Combine(config.SavePath, $"{data.TestName}.cs");

            await File.WriteAllTextAsync(filePath, data.data);
        }

        private async Task<TestFile[]> GenerateTestClasses(string fileText)
        {
            var root = CSharpSyntaxTree.ParseText(fileText).GetCompilationUnitRoot();
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            // var result = new List<string>();
            var result = new List<TestFile>();

            foreach (var class_ in classes)
            {
                result.Add(/*class_.Identifier.Text*/await GenerateTestClass(class_, root));
            }

            return result.ToArray();
        }

        private async Task<TestFile> GenerateTestClass(ClassDeclarationSyntax classDeclaration,
           CompilationUnitSyntax root)
        {
            return await Task.Run(() =>
            {
                var compilationUnit = SyntaxFactory.CompilationUnit();

        //        compilationUnit = compilationUnit.AddUsings(GenerateTestUsings(root).ToArray());

                var baseNamespace = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
                var @namespace = SyntaxFactory.NamespaceDeclaration(
                    baseNamespace is null ? SyntaxFactory.IdentifierName("Tests") : SyntaxFactory.IdentifierName($"{baseNamespace.Name}.Tests")
                );

                var @class = SyntaxFactory.ClassDeclaration(classDeclaration.Identifier.Text + "Tests")
                    .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)));

                //var depInj = GenerateDependencyInjection(classDeclaration);

                //@class = @class.AddMembers(depInj.Item1.ToArray());
               // @class = @class.AddMembers(depInj.Item2);
               // @class = @class.AddMembers(GenerateTestMethods(classDeclaration).ToArray());

                compilationUnit = compilationUnit.AddMembers(@namespace.AddMembers(@class));
                var testFile = new TestFile(/*classDeclaration.Identifier.Text*/@class.Identifier.Text, compilationUnit.NormalizeWhitespace("    ","\r\n").ToString());//compilationUnit.NormalizeWhitespace().ToString());
                return testFile;// compilationUnit.ToString();//.NormalizeWhitespace().ToString();
            });
        }

    }
}
