using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.Util.FileReaders
{
    internal class StringArrayFileReader : BaseFileReader<string[,]>
    {
        protected override string[,] ConvertFileContentToReadable(string content)
        {
            string[] perLineResult = GetPerLineResult(content);
            int lineLength = perLineResult[0].Length;
            string[,] result = new string[perLineResult.Length, lineLength];

            for (int i = 0; i < perLineResult.Length; i++)
            {
                for (int j = 0; j < lineLength; j++)
                {
                    result[i,j] = perLineResult[i][j].ToString();
                }
            }

            return result;
        }

        private string[] GetPerLineResult(string content)
        {
            var reader = new LineSplitFileReader();
            return reader.GetReadableFileContent(content, false);
        }
    }
}
