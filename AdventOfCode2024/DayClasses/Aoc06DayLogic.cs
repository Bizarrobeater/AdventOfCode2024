using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc06DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {
                1,
                new Dictionary<int, long>() 
                { 
                    { 1, 41 } 
                } 
            } 
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CharMultiArrayFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);

            HashSet<Coordinate> visited = new HashSet<Coordinate>();

            CoordinateDirection curr = LocateStart(content);
            visited.Add(curr.Position);

            while (true)
            {
                curr = MoveNext(content, curr.Position, curr.Direction);
                if (curr.Position.X == -1) { break; }
                visited.Add(curr.Position);

            }

            return visited.Count;
        }

        private CoordinateDirection MoveNext(char[,] content, Coordinate pos, Coordinate direction)
        {
            int nextX = pos.X + direction.X;
            int nextY = pos.Y + direction.Y;
            if (nextX < 0 || nextX >= content.GetLength(0)
                || nextY < 0 || nextY >= content.GetLength(1))
            {
                return new CoordinateDirection() { Direction = direction, Position = new Coordinate() { X = -1, Y = -1 } };
            }
            else if (content[nextY, nextX] == '#')
            {
                return MoveNext(content, pos, new Coordinate() { X = direction.Y * -1, Y = direction.X});
            }
            return new CoordinateDirection() { Direction = direction, Position = new Coordinate() { X = nextX, Y = nextY } };
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            throw new NotImplementedException();
        }

        private CoordinateDirection LocateStart(char[,] input)
        {
            for (int y = 0;  y < input.GetLength(0); y++)
            {
                for (int x = 0;  x < input.GetLength(1); x++)
                {
                    if (input[y, x] == '^')
                    {
                        return new CoordinateDirection()
                        {
                            Position = new Coordinate() { X = x, Y = y },
                            Direction = new Coordinate() { X = 0, Y = -1 },
                        };
                    }
                }
            }
            throw new ArgumentOutOfRangeException();
        }

        private record Coordinate
        {
            public int X;
            public int Y;

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
        }

        private record CoordinateDirection
        {
            public required Coordinate Position;
            public required Coordinate Direction;

            public override int GetHashCode()
            {
                return HashCode.Combine(Position, Direction);
            }
        }
    }
}