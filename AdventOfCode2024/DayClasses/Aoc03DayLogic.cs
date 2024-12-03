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

            var doPattern = @"(do\(\)|don't\(\))(((?!(do\(\)|don't\(\))).)*)";
            var doRegex = new Regex(doPattern);
            long result = 0;
            foreach (Match match in doRegex.Matches(content))
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
            string pattern = @"mul\(\d{1,3},\d{1,3}\)";
            var reg = new Regex(pattern);
            var matches = reg.Matches(content);

            var subPattern = @"\((\d{1,3}),(\d{1,3})\)";
            var subRegex = new Regex(subPattern);
            Match subMatch;
            long result = 0;
            foreach (var match in matches)
            {
                subMatch = subRegex.Match(match.ToString()!);
                result += int.Parse(subMatch.Groups[1].Value) * int.Parse(subMatch.Groups[2].Value);
            }
            return result;
        }
    }
}
