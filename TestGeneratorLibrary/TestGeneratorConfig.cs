using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneratorLibrary
{
    public class TestGeneratorConfig
    {
        internal int MaxFilesReadingParallel;
        internal int MaxFilesWritingParallel;
        internal int MaxTestClassesGeneratingParallel;
        internal List<string> FilesPaths;
        internal string SavePath;
        public TestGeneratorConfig(int maxFilesReadingParallel, int maxFilesWritingParallel,
          int maxTestClassesGeneratingParallel, List<string> filesPaths, string savePath)
        {
            MaxFilesReadingParallel = maxFilesReadingParallel;
            MaxFilesWritingParallel = maxFilesWritingParallel;
            MaxTestClassesGeneratingParallel = maxTestClassesGeneratingParallel;
            FilesPaths = filesPaths;
            SavePath = savePath;
        }
    }
}
