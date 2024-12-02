using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCodeApp.Util.FileReaders;

namespace AdventOfCode2024.Util.FileReaders
{
    internal class SplitIntMultiArrayFileReader : BaseFileReader<int[][]>
    {
        protected override int[][] ConvertFileContentToReadable(string content)
        {
            var lines = GetPerLineResult(content);

            int[][] result = new int[lines.Length][];
            int[] linesInt;
            string item;
            for (int i = 0; i < lines.Length; i++)
            {
                item = lines[i];
                linesInt = item.Split().Select(s => int.Parse(s)).ToArray();
                result[i] = linesInt;
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