using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc11DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new()
        {
            {1, new() { {1, 55312 } } },
        };

        

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CleanFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark).Split(' ');
            int blinks = 25;

            var dict = Blink(content, blinks);

            long result = 0;
            foreach (var item in dict)
            {
                foreach (var rocks in item.Value)
                {
                    result += rocks.Value;
                }
            }

            return result;

        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CleanFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark).Split(' ');
            int blinks = 75;

            var dict = Blink(content, blinks);

            long result = 0;
            foreach (var item in dict)
            {
                foreach (var rocks in item.Value)
                {
                    result += rocks.Value;
                }
            }

            return result;
        }

        private Dictionary<Rule, Dictionary<string, long>> Blink(string[] content, int blinks)
        {
            var dict = GetNewDict();
            foreach (var input in content)
            {
                PlaceStringValueInDict(dict, input, 1);
            }
            Dictionary<Rule, Dictionary<string, long>> tempDict;
            for (int i = 0; i < blinks; i++)
            {
                tempDict = GetNewDict();
                PlaceStringValueInDict(tempDict, "1", dict[Rule.Zero].GetValueOrDefault("0", 0));

                foreach(KeyValuePair<string, long> value in dict[Rule.Even])
                {
                    PlaceStringValueInDict(tempDict, value.Key.Substring(0, value.Key.Length / 2), value.Value);
                    PlaceStringValueInDict(tempDict, value.Key.Substring(value.Key.Length / 2), value.Value);
                }

                foreach (KeyValuePair<string, long> value in dict[Rule.None])
                {
                    PlaceStringValueInDict(tempDict, (long.Parse(value.Key) * 2024).ToString(), value.Value);
                }
                dict = tempDict;
            }

            return dict;
        }

        private void PlaceStringValueInDict(Dictionary<Rule, Dictionary<string, long>> dict, string input, long amount)
        {
            if (amount == 0) return;
            input = input.TrimStart('0');
            if (input == "")
                SafeAddValueToDict(dict[Rule.Zero], "0", amount);
            else if (input.Length % 2 == 0)
                SafeAddValueToDict(dict[Rule.Even], input, amount);
            else
                SafeAddValueToDict(dict[Rule.None], input, amount);
        }

        private void SafeAddValueToDict (Dictionary<string, long> dict, string value, long amount)
        {
            if (!dict.ContainsKey(value))
            {
                dict[value] = 0; 
            }
            dict[value] += amount;
        }

        private Dictionary<Rule, Dictionary<string, long>> GetNewDict()
        {
            return new()
            {
                { Rule.None, new() },
                { Rule.Even, new() },
                { Rule.Zero, new() }
            };
        }

        private enum Rule 
        {
            None,
            Even,
            Zero
        }
    }
}
