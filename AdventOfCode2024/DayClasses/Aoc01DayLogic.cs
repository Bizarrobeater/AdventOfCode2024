using AdventOfCode2024.Util.FileReaders;
using AdventOfCodeApp.DayClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc01DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>
        {
            {1,
                new Dictionary<int, long>
                {
                    {1, 11 }
                }
            },
            {
                2,
                new Dictionary<int, long>
                {
                    { 1, 31 }
                }
            }
        };
        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new Aoc202401Reader();
            (List<long> right,  List<long> left) = reader.GetReadableFileContent(file, isBenchmark);

            right.Sort();
            left.Sort();
            long result = 0;
            for (int i = 0; i < right.Count; i++)
            {
                result += Math.Abs(right[i] - left[i]);
            }
            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            long result = 0;

            var reader = new Aoc202401Reader();
            (List<long> right, List<long> left) = reader.GetReadableFileContent(file, isBenchmark);

            var rightDict = CreateDictFromList(right);
            var leftDict = CreateDictFromList(left);

            foreach (KeyValuePair<long, int> kvp in rightDict)
            {
                if (leftDict.ContainsKey(kvp.Key))
                {
                    result += kvp.Key * leftDict[kvp.Key] * kvp.Value;
                }
            }

            return result;
        }

        private Dictionary<long, int> CreateDictFromList(List<long> list)
        {
            var result = new Dictionary<long, int>();
            foreach (var item in list)
            {
                if (!result.ContainsKey(item))
                {
                    result[item] = 0;
                }
                result[item]++;
            }
            return result;
        }
    }
}
