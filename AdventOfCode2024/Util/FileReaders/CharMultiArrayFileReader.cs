using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.Util.FileReaders
{
    internal class CharMultiArrayFileReader : BaseFileReader<char[,]>
    {
        protected override char[,] ConvertFileContentToReadable(string content)
        {
            var stringArray = GetStringContent(content);

            char[,] result = new char[stringArray.GetLength(0), stringArray.GetLength(1)];
            for (int i = 0; i < stringArray.GetLength(0); i++)
            {
                for (int j = 0; j < stringArray.GetLength(1); j++)
                {
                    result[i,j] = char.Parse(stringArray[i,j]);

                }
            }
            return result;

        }

        private string[,] GetStringContent(string content)
        {
            var stringReader = new StringArrayFileReader();
            return stringReader.GetReadableFileContent(content, false);
        }
    }
}
