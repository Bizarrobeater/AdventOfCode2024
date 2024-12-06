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
            },
            {
                2,
                new Dictionary<int, long>()
                {
                    { 1, 6 }
                }
            }
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CharMultiArrayFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);

            HashSet<Coordinate> visited = new HashSet<Coordinate>();

            Ray curr = LocateStart(content);
            visited.Add(curr.Position);

            while (true)
            {
                curr = MoveNext(content, curr.Position, curr.Direction);
                if (curr.Position.X == -1) { break; }
                visited.Add(curr.Position);

            }

            return visited.Count;
        }

        private Ray MoveNext(char[,] content, Coordinate pos, Coordinate direction, Coordinate? newBlock = null)
        {
            int nextX = pos.X + direction.X;
            int nextY = pos.Y + direction.Y;
            if (nextX < 0 || nextX >= content.GetLength(0)
                || nextY < 0 || nextY >= content.GetLength(1))
            {
                return new Ray() { Direction = direction, Position = new Coordinate() { X = -1, Y = -1 } };
            }
            else if (content[nextY, nextX] == '#' || (newBlock != null && nextX == newBlock.X && nextY == newBlock.Y))
            {
                return MoveNext(content, pos, new Coordinate() { X = direction.Y * -1, Y = direction.X}, newBlock);
            }
            return new Ray() { Direction = direction, Position = new Coordinate() { X = nextX, Y = nextY } };
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CharMultiArrayFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);

            List<Ray> rays = new List<Ray>();

            Ray start = LocateStart(content);
            Ray curr = LocateStart(content);
            rays.Add(curr);
            int count = 0;
            while (true)
            {
                curr = MoveNext(content, curr.Position, curr.Direction);
                if (curr.Position.X == -1) { break; }
                rays.Add(curr);
            }

            Coordinate newBlock;
            HashSet<Ray> loopRays;
            HashSet<Coordinate> tested = new HashSet<Coordinate>();
            foreach (var ray in rays) 
            {
                tested.Add(ray.Position);
                newBlock = new Coordinate() { X = ray.Position.X + ray.Direction.X, Y = ray.Position.Y + ray.Direction.Y };
                newBlock = MoveNext(content, ray.Position, ray.Direction).Position;
                if (
                    //newBlock == start.Position ||
                    tested.Contains(newBlock) ||
                    newBlock.X < 0 || newBlock.Y < 0 ||
                    newBlock.Y >= content.GetLength(1) ||
                    newBlock.X >= content.GetLength(0) ||
                    content[newBlock.Y, newBlock.X] == '#'
                    )
                    continue;
                //curr = new Ray() { Position = ray.Position, Direction = new Coordinate() { X = ray.Direction.Y * -1, Y = ray.Direction.X } };
                curr = MoveNext(content, ray.Position, ray.Direction, newBlock);
                loopRays = [curr];
                while (true)
                {
                    curr = MoveNext(content, curr.Position, curr.Direction, newBlock);
                    if (loopRays.Contains(curr))
                    {
                        count++;
                        break;
                    }
                    else if (curr.Position.X == -1)
                    {
                        break;
                    }
                    loopRays.Add(curr);
                }

            }

            return count;
        }

        private Ray LocateStart(char[,] input)
        {
            for (int y = 0;  y < input.GetLength(0); y++)
            {
                for (int x = 0;  x < input.GetLength(1); x++)
                {
                    if (input[y, x] == '^')
                    {
                        return new Ray()
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
                //return X * 10 + Y;
            }
        }

        private record Ray
        {
            public required Coordinate Position;
            public required Coordinate Direction;

            public override int GetHashCode()
            {
                return HashCode.Combine(Position.GetHashCode(), Direction.GetHashCode());
            }
        }
    }
}