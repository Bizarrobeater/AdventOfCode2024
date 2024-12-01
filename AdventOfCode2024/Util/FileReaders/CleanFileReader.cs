using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.Util.FileReaders
{
    internal class CleanFileReader : BaseFileReader<string>
    {
        protected override string ConvertFileContentToReadable(string content)
        {
            return content;
        }
    }
}
