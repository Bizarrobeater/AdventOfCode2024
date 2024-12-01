using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.Util.FileReaders
{
    internal abstract class BaseFileReader<T>
    {
        public static T? BenchmarkValue { get; private set; }

        private string ReadFile(FileInfo file)
        {
            string result = "";
            using (StreamReader sr = file.OpenText())
            {
                result = sr.ReadToEnd();
            }
            return result;
        }

        public T GetReadableFileContent(FileInfo file, bool isBenchmark)
        {
            if (isBenchmark && BenchmarkValue != null)
                return BenchmarkValue;

            string contentResult = ReadFile(file);
            T result = ConvertFileContentToReadable(contentResult);

            if (isBenchmark) 
                BenchmarkValue = result;

            return result;
        }

        public T GetReadableFileContent(string content, bool isBenchmark)
        {
            if (isBenchmark && BenchmarkValue != null)
                return BenchmarkValue;

            T result = ConvertFileContentToReadable(content);

            if (isBenchmark)
                BenchmarkValue = result;

            return result;
        }

        protected abstract T ConvertFileContentToReadable(string content);
    }
}
