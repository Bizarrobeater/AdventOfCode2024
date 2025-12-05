using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc2025_05DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new()
        {
            { 1, new() { { 1, 3 } } },
            { 2, new() { { 1, 14 } } },
        };

        // 389 too low
        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CleanFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);
            var (ranges, ids) = ParseContent(content);
            var rangeSorted = new List<(long start, long end)>();
            foreach (var range in ranges)
            {
                var parts = range.Split('-');
                long start = long.Parse(parts[0]);
                long end = long.Parse(parts[1]);
                rangeSorted.Add((start, end));
            }
            rangeSorted = rangeSorted.OrderBy(r => r.start).ToList();
            bool found;
            long result = 0;
            foreach (var id in ids)
            {
                found = false;
                foreach (var range in rangeSorted)
                {
                    if (id >= range.start && id <= range.end)
                    {
                        found = true;
                        break;
                    }
                    if (id < range.start)
                    {
                        break;
                    }
                }
                if (found)
                {
                    result++;
                }
            }
            return result;
        }

        (string[] ranges, long[] ids) ParseContent(string content)
        {
            var parts = content.Split(Environment.NewLine + Environment.NewLine);
            var ranges = parts[0].Split(Environment.NewLine).Select(r => r.Trim()).ToArray();
            var ids = parts[1].Split(Environment.NewLine).Select(id => long.Parse(id.Trim())).ToArray();
            return (ranges, ids);
        }

        // 346.705.909.538.854 too high
        // 346.705.909.538.758 too high
        // 345.995.423.801.866
        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CleanFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);
            var (ranges, ids) = ParseContent(content);
            var rangeSorted = new List<(long start, long end)>();
            foreach (var range in ranges)
            {
                var parts = range.Split('-');
                long start = long.Parse(parts[0]);
                long end = long.Parse(parts[1]);
                rangeSorted.Add((start, end));
            }
            rangeSorted = rangeSorted.OrderBy(r => r.start).ThenBy(r => r.end).ToList();
            List<(long start, long end)> newRanges;
            bool changed = true;
            while (changed)
            {
                changed = false;
                newRanges = new List<(long start, long end)>();
                for (int i = 0; i < rangeSorted.Count; i++)
                {
                    var current = rangeSorted[i];
                    if (i < rangeSorted.Count - 1)
                    {
                        var next = rangeSorted[i + 1];

                        // current range wraps next range
                        if (current.start <= next.start && current.end >= next.end)
                        {
                            i++;
                            changed = true;
                            newRanges.Add(current);
                        }
                        else if (current.start == next.start && current.end <= next.end)
                        {
                            changed = true;
                        }
                        else if (current.end >= next.start)
                        {
                            newRanges.Add((current.start, next.start - 1));
                            changed = true;
                        }
                        else
                        {
                            newRanges.Add(current);
                        }
                    }
                    else
                    {
                        newRanges.Add(current);
                    }
                }
                rangeSorted = newRanges;
            }

            long result = 0;
            foreach (var range in rangeSorted)
            {
                result += range.end - range.start + 1;
            }
            return result;


        }
    }
}
