using AdventOfCodeApp.Util.FileReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.DayClasses
{
    internal interface IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults { get; }

        public long RunQuestion1(FileInfo file, bool isBenchmark = false);
        public long RunQuestion2(FileInfo file, bool isBenchmark = false);

    }
}