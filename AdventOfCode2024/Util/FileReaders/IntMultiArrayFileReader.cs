using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.Util.FileReaders
{
    internal class IntMultiArrayFileReader : BaseFileReader<int?[,]>
    {
        protected override int?[,] ConvertFileContentToReadable(string content)
        {
            var stringArray = GetStringContent(content);

            int?[,] result = new int?[stringArray.GetLength(0), stringArray.GetLength(1)];

            for (int i = 0; i < stringArray.GetLength(0); i++)
            {
                for (int j = 0; j < stringArray.GetLength(1); j++)
                {
                    result[i,j] = int.TryParse(stringArray[i,j], out int newInt) ? newInt : null;

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
