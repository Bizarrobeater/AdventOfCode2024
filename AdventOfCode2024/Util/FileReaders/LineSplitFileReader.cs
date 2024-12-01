using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.Util.FileReaders
{
    internal class LineSplitFileReader : BaseFileReader<string[]>
    {
        protected override string[] ConvertFileContentToReadable(string content)
        {
            string newLine = Environment.NewLine;
            return content.Split(newLine, StringSplitOptions.TrimEntries);
        }
    }
}
