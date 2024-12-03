using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc03DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {
                1,
                new Dictionary<int, long>()
                {
                    { 1,
                    161 }
                }
            },
            {
                2, 
                    new Dictionary<int, long>()
                    {
                        {1, 48 }
                    }
            }
        };

        private static readonly Regex _doReg = new Regex(@"(do\(\)|don't\(\))(((?!(do\(\)|don't\(\))).|\n)*)",
            options: RegexOptions.Compiled);
        private static readonly Regex _mulReg = new Regex(@"mul\(\d{1,3},\d{1,3}\)", options: RegexOptions.Compiled);
        private static readonly Regex _subReg = new Regex(@"\((\d{1,3}),(\d{1,3})\)", options: RegexOptions.Compiled);

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CleanFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);
            return FindAndAddMatches(content);
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CleanFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);
            content = "do()" + content;

            long result = 0;
            foreach (Match match in _doReg.Matches(content))
            {
                if (match.Groups[1].Value == "do()")
                {
                    result += FindAndAddMatches(match.Captures[0].Value);
                }
            }
            return result;
        }

        private long FindAndAddMatches(string content)
        {
            Match subMatch;
            long result = 0;
            foreach (var match in _mulReg.Matches(content))
            {
                subMatch = _subReg.Match(match.ToString()!);
                result += int.Parse(subMatch.Groups[1].Value) * int.Parse(subMatch.Groups[2].Value);
            }
            return result;
        }

        /*Benchmarks
         Before compiled regex
        Benchmark in ticks:
        First Run Time: 6836
        Last Run Time: 81634
        Average: 19860,751
        Median: 20641
        Max Time: 81634
        Min Time: 6836
         
        After compiled regex
        Benchmark in ticks:
        First Run Time: 1978
        Last Run Time: 39796
        Average: 4450,742
        Median: 3475
        Max Time: 39796
        Min Time: 1978
         
         */
    }
}
