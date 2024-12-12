using System.Diagnostics.Eventing.Reader;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

using CommunityToolkit.HighPerformance;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc12DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new()
        {
            { 1, new() { {1,140}, { 2, 772}, { 3, 1930} } }
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CharMultiArrayFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark).AsSpan2D();

            var regions = new List<Region>();
            HashSet<Coordinate> coordinates = new HashSet<Coordinate>();
            Coordinate curr;

            for (int y = 0; y < content.Height; y++)
            {
                for (int x = 0; x < content.Width; x++)
                {
                    curr = new Coordinate() { X = x, Y = y };
                    if (coordinates.Contains(curr)) continue;
                    regions.Add(new Region(content, ))
                }
            }


            return 0;
        }

        private HashSet<Coordinate> GetUsedCoordinates(List<Region> regions)
        {
            var result = new HashSet<Coordinate>();
            foreach (Region region in regions)
            {
                result.UnionWith(region.Coordinates);
            }
            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            throw new NotImplementedException();
        }

        private record Coordinate
        {
            public required int X { get; set; }
            public required int Y { get; set; }

            public override int GetHashCode()
            {
                return HashCode.Combine(X.GetHashCode(), Y.GetHashCode());
            }
        }

        private class RegionCoordinate
        {
            private readonly (int x, int y)[] directions = { (1, 0), (-1, 0), (0, -1), (0, 1) };

            public Coordinate Coordinate { get; set; }
            public List<RegionCoordinate> Neighbours { get; set; } = new List<RegionCoordinate>();
            public Region Region { get; set; }

            public int Fences => 4 - Neighbours.Count;


            public RegionCoordinate(Region region, Coordinate coordinate, Span2D<char> map)
            {
                Region = region;
                Coordinate = coordinate;
                Coordinate temp;
                foreach (var direction in directions)
                {
                    temp = new Coordinate() {Y = Coordinate.Y + direction.y, X = Coordinate.X + direction.x };
                    if (IsWithinMap(map, temp) && map[temp.Y, temp.X] == Region.Type && !Region.Coordinates.Contains(temp))
                    {
                        Neighbours.Add(new RegionCoordinate(Region, temp, map));
                    }
                }
            }

            private static bool IsWithinMap(Span2D<char> map, Coordinate coordinate)
            {
                return coordinate.X >= 0 && coordinate.Y >= 0 && coordinate.Y < map.Height && coordinate.X < map.Width;
            }

        }

        private class Region
        {
            public char Type { get; set; }
            public HashSet<Coordinate> Coordinates { get; set; } = new HashSet<Coordinate>();
            public List<RegionCoordinate> RegionCoordinates { get; set; } = new List<RegionCoordinate>();

            private long FencePrice 
            { 
                get 
                {
                    int fenceSize = 0;
                    foreach (var regionCoordinate in RegionCoordinates)
                    {
                        fenceSize += regionCoordinate.Fences;
                    }
                    return RegionCoordinates.Count * fenceSize;
                } 
            }

            private Region(Span2D<char> map, Coordinate coordinate ) 
            {
                Type = map[coordinate.X, coordinate.Y];
                Coordinates.Add(coordinate);
                RegionCoordinates.Add(new RegionCoordinate(this, coordinate, map));
            }

        }
    }
}
