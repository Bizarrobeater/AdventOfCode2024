using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

using CommunityToolkit.HighPerformance;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc12DayLogic : IDayLogic
    {
        public static readonly (int x, int y)[] directions = { (1, 0), (-1, 0), (0, -1), (0, 1) };
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

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
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
                result += region.FencePriceWithSides;
            }

            return result;
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

        private record EdgeCoordinate
        {
            public required Coordinate Coordinate { get; set; }
            public required int X { get; set; }
            public required int Y { get; set; }
            public override int GetHashCode()
            {
                return HashCode.Combine(Coordinate.GetHashCode(), X.GetHashCode(), Y.GetHashCode());
            }
        }

        private class RegionCoordinate
        {
            

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

            public HashSet<RegionCoordinate> EdgeCoordinates
            {
                get
                {
                    var result = new HashSet<RegionCoordinate>();
                    foreach (var reg in RegionCoordinates)
                    {
                        if (reg.Neighbours.Count == 4) continue;
                        result.Add(reg);
                    }
                    return result;
                }
            }

            public long FencePrice => GetFencePrice();

            public long FencePriceWithSides => GetFencePriceWithSides();

            public Region(Span2D<char> map, Coordinate coordinate ) 
            {
                Type = map[coordinate.Y, coordinate.X];
                Coordinates.Add(coordinate);
                RegionCoordinates.Add(new RegionCoordinate(this, coordinate, map));
            }

            private long GetFencePrice()
            {
                int fenceSize = 0;
                foreach (var regionCoordinate in RegionCoordinates)
                {
                    fenceSize += regionCoordinate.Fences;
                }
                return RegionCoordinates.Count * fenceSize;
            }

            private long GetFencePriceWithSides()
            {
                int sides = 0;
                var edges = EdgeCoordinates;
                var edgeCoords = edges.Select(x => x.Coordinate).ToHashSet();
                Coordinate temp;
                EdgeCoordinate edgeCoordinate;
                HashSet<EdgeCoordinate> checkedEdges = new HashSet<EdgeCoordinate>();
                (int x, int y) moveDir;
                foreach (var edgeCorr in EdgeCoordinates)
                {
                    foreach (var lookDir in directions) 
                    { 
                        moveDir = GetMoveDirFromLookDir(lookDir);
                        temp = new Coordinate() { X = edgeCorr.Coordinate.X + moveDir.x, Y = edgeCorr.Coordinate.Y + moveDir.y };
                        edgeCoordinate = new EdgeCoordinate() { Coordinate = edgeCorr.Coordinate, X = lookDir.x, Y = lookDir.y };
                        if (edgeCorr.Neighbours.Contains(temp) || checkedEdges.Contains(edgeCoordinate)) continue;
                        checkedEdges.Add(edgeCoordinate);
                        sides++;
                        while (edgeCoords.Contains(temp))
                        {
                            checkedEdges.Add(new EdgeCoordinate() { Coordinate = temp, X = lookDir.x, Y = lookDir.y });
                            temp = new Coordinate() { X = temp.X + moveDir.x, Y = temp.Y + moveDir.y };
                        }
                    }
                }
                return sides;
            }

            private static (int x, int y) GetMoveDirFromLookDir((int x, int y) dir)
            {
                switch (dir)
                {
                    case (-1, 0): return (0, 1);
                    case (0, -1): return (-1, 0);
                    case (1, 0): return (0, -1);
                    case (0, 1): return (1, 0);
                    default: throw new NotImplementedException();
                }
            }
        }
    }
}
