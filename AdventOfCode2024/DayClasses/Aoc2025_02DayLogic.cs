using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc2025_02DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new()
        {
            { 1, new() { { 1, 1_227_775_554 } } },
            { 2, new() { { 1, 4_174_379_265 } } },
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CleanFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);
            var ranges = content.Split(",");
            var startString = string.Empty;
            var endString = string.Empty;
            var startSplitValue = string.Empty;
            var endSplitValue = string.Empty;
            long start;
            long end;
            string currentValueString;
            string[] bounds;

            long result = 0;
            foreach (var range in ranges)
            {
                bounds = range.Split("-");
                startString = bounds[0];
                endString = bounds[1];
                if (IsOdd(startString.Length) && IsOdd(endString.Length)) continue;

                start = Convert.ToInt64(startString);
                end = Convert.ToInt64(endString);

                for (long i = start; i <= end; i++)
                {
                    currentValueString = i.ToString();
                    if (IsOdd(currentValueString.Length)) continue;
                    (startSplitValue, endSplitValue) = SplitStringInHalf(currentValueString);
                    if (startSplitValue == endSplitValue)
                    {
                        result += i;
                    }
                }
                //startSplitValue = GetLastHalf(startString);
                //endSplitValue = GetLastHalf(endString);
            }
            return result;
        }

        public (string first, string last) SplitStringInHalf(string value)
        {
            int halfLength = value.Length / 2;
            string first = value[0..halfLength];
            string last = value[halfLength..];
            return (first, last);
        }

        public bool IsOdd(int number)
        {
            return (number % 2) != 0;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CleanFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);
            var ranges = content.Split(",");
            var startString = string.Empty;
            var endString = string.Empty;
            var startSplitValue = string.Empty;
            var endSplitValue = string.Empty;
            long start;
            long end;
            string currentValueString;
            string[] bounds;

            long result = 0;
            foreach (var range in ranges)
            {
                bounds = range.Split("-");
                startString = bounds[0];
                endString = bounds[1];

                start = Convert.ToInt64(startString);
                end = Convert.ToInt64(endString);

                for (long i = start; i <= end; i++)
                {
                    result += CompareOddLength(i);
                    //currentValueString = i.ToString();

                    //if (IsOdd(currentValueString.Length))
                    //{
                    //    result += CompareOddLength(i);
                    //}
                    //else
                    //{
                    //    (startSplitValue, endSplitValue) = SplitStringInHalf(currentValueString);
                    //    if (startSplitValue == endSplitValue)
                    //    {
                    //        result += i;
                    //    }
                    //}
                }
            }
            return result;
        }

        public long CompareOddLength(long value)
        {
            string valueString = value.ToString();
            string[] chunks;
            bool matchFound = false;
            int chunkLength;
            for (int i = 1; i <= valueString.Length / 2; i++)
            {
                chunks = Enumerable.Range(0, valueString.Length / i).Select(j => valueString.Substring(j * i, i)).ToArray();
                if (chunks.Length == 1) break;

                chunkLength = 0;
                foreach (var chunk in chunks)
                {
                    chunkLength += chunk.Length;
                }

                if (chunkLength < valueString.Length) continue;


                matchFound = CompareChunks(chunks);
                if (matchFound)
                {
                    return value;
                }
            }
            return 0;
        }

        public bool CompareChunks(string[] chunks)
        {
            string firstChunk = chunks[0];
            for (int j = 1; j < chunks.Length; j++)
            {
                if (chunks[j] != firstChunk)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
