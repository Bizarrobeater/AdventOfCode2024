using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

using AdventOfCode2024.Util.FileReaders;

using AdventOfCodeApp.DayClasses;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc02DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>
        {
            {
                1,
                    new Dictionary<int, long> {
                        {
                        1, 2 }
                    }
            },
            {
                2,
                new Dictionary<int, long>
                {
                    {1,4 }
                }
            }
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new SplitIntMultiArrayFileReader();
            var reports = reader.GetReadableFileContent(file, isBenchmark);
            bool increasing = false;
            bool safe = false;
            int curr;
            int diff;
            int result = 0;
            int count = 0;
            foreach (var report in reports)
            {
                for (int i = 0; i < report.Length; i++)
                {
                    curr = report[i];
                    if (i == 0)
                    {
                        increasing = curr < report[i + 1];
                        continue;
                    }
                    diff = curr - report[i - 1];
                    if (diff == 0 || Math.Abs(diff) > 3)
                    {
                        safe = false;
                        break;
                    }
                    if ((increasing && diff > 0) || (!increasing && diff < 0))
                    {
                        safe = true;
                    }
                    else
                    {
                        safe = false;
                        break;
                    }
                }
                count++;
                if (safe)
                {
                    result++;
                }
                safe = false;
            }
            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new SplitIntMultiArrayFileReader();
            var reports = reader.GetReadableFileContent(file, isBenchmark);
            bool increasing = false;
            bool safe = false;
            int curr;
            int prev = 0;
            int diff;
            int result = 0;
            int errors = 0;
            foreach (var report in reports)
            {
                for (int i = 0; i < report.Length; i++)
                {
                    curr = report[i];
                    if (i == 0)
                    {
                        increasing = curr < report[i + 1];
                        prev = curr;
                        continue;
                    }
                    diff = curr - prev;
                    if ((diff == 0 || Math.Abs(diff) > 3) || !((increasing && diff > 0) || (!increasing && diff < 0)))
                    {
                        if (errors == 0)
                        {
                            if (i == 1)
                            {
                                increasing = curr < report[i + 1];
                                if (1 >= Math.Abs(curr - report[i+1]) && Math.Abs(curr - report[i + 1]) <= 3)
                                {
                                    prev = curr;
                                }
                            }

                            errors++;
                            continue;
                        }

                        safe = false;
                        break;
                    }
                    else
                    {
                        safe = true;
                        prev = curr;
                    }
                }
                if (safe)
                {
                    result++;
                }
                safe = false;
                errors = 0;
            }
            return result;
        }
    }
}
