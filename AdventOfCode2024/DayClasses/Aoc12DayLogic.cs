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
            { 1, new() { {1,140}, { 2, 772}, { 3, 1930} } },
            { 2, new() { {1, 80}, { 2, 436}, {3, 236 }, {4, 368 }, { 5, 1206 } } }
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CharMultiArrayFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark).AsSpan2D();

            var regions = new List<Region>();
            HashSet<Coordinate> coordinates = new HashSet<Coordinate>();
            Coordinate curr;
            Region newRegion;

            for (int y = 0; y < content.Height; y++)
            {
                for (int x = 0; x < content.Width; x++)
                {
                    curr = new Coordinate() { X = x, Y = y };
                    if (coordinates.Contains(curr)) continue;
                    newRegion = new Region(content, curr);
                    regions.Add(newRegion);
                    coordinates.UnionWith(newRegion.Coordinates);
                }
            }

            long result = 0;
            foreach (var region in regions)
            {
                result += region.FencePrice;
            }

            return result;
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
            public HashSet<Coordinate> Neighbours { get; set; } = new();
            public Region Region { get; set; }

            public int Fences => 4 - Neighbours.Count;


            public RegionCoordinate(Region region, Coordinate coordinate, Span2D<char> map)
            {
                Region = region;
                Coordinate = coordinate;
                Region.Coordinates.Add(coordinate);
                Coordinate temp;
                foreach (var direction in directions)
                {
                    temp = new Coordinate() {Y = Coordinate.Y + direction.y, X = Coordinate.X + direction.x };
                    if (IsWithinMap(map, temp) && map[temp.Y, temp.X] == Region.Type)
                    {
                        if (!Region.Coordinates.Contains(temp))
                        {
                            Region.RegionCoordinates.Add(new RegionCoordinate(region, temp, map));
                        }
                        Neighbours.Add(temp);
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

            public long FencePrice 
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

            public Region(Span2D<char> map, Coordinate coordinate ) 
            {
                Type = map[coordinate.Y, coordinate.X];
                Coordinates.Add(coordinate);
                RegionCoordinates.Add(new RegionCoordinate(this, coordinate, map));
            }

        }
    }
}
