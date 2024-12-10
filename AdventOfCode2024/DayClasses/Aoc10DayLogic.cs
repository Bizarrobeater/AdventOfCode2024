using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

using CommunityToolkit.HighPerformance;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc10DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new()
        {
            { 1, new() { { 1, 36 } , {2, 2 }, { 3, 4 }, { 4, 3 } } },
            { 2, new() { { 1, 3 }, {2, 13 }, {3, 227}, {4, 81 } } }
        };

        private readonly (int x, int y)[] directions = { (1, 0), (-1, 0), (0, -1), (0, 1) };
        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new IntMultiArrayFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark).AsSpan2D();

            var trailheads = GetTrailheads(content);

            long result = 0;
            foreach (var trailhead in trailheads)
            {
                result += CountRatingFromPoint(trailhead, content).ToHashSet().Count;
            }

            return result;
        }

        private bool IsPointInBounds((int x, int y) point, Span2D<int?> map)
        {
            return point.x >= 0 && point.y >= 0 && point.x < map.Width && point.y < map.Height;
        }

        private List<(int x, int y)> GetTrailheads(Span2D<int?> map)
        {
            var trailheads = new List<(int x, int y)>();
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (map[y, x] == 0)
                    {
                        trailheads.Add((x, y));
                    }
                }
            }
            return trailheads;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new IntMultiArrayFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark).AsSpan2D();

            var trailheads = GetTrailheads(content);

            long result = 0;
            foreach (var trailhead in trailheads)
            {
                result += CountRatingFromPoint(trailhead, content).Count;
            }

            return result;
        }

        private List<(int x, int y)> CountRatingFromPoint((int x, int y) point, Span2D<int?> map)
        {
            int? currHeight = map[point.y, point.x];
            if (currHeight == 9)
                return new() { point };

            var result = new List<(int x, int y)>();
            (int x, int y) newPoint;
            foreach (var direction in directions)
            {
                newPoint = (point.x + direction.x, point.y + direction.y);
                if (!IsPointInBounds(newPoint, map) || map[newPoint.y, newPoint.x] != currHeight + 1) continue;
                result.AddRange(CountRatingFromPoint((point.x + direction.x, point.y + direction.y), map));
            }
            return result;
        }
    }
}
