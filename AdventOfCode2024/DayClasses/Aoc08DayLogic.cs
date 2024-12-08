using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc08DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>
        {
            {
                1,
                new Dictionary<int, long>
                {
                    {1, 14 },
                    {2, 2 },
                    {3, 4 }
                }
            },
            {
                2,
                new Dictionary<int, long>
                {
                    {1,34 },
                    {2, 9 }
                }
            }
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CharMultiArrayFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);

            var antennas = new Dictionary<char, List<Coordinate>>();
            int yLength = content.GetLength(0);
            int xLength = content.GetLength(1);
            char curr;
            HashSet<Coordinate> antinodes = new HashSet<Coordinate>();
            for (int y = 0; y < yLength; y++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    curr = content[y, x];
                    if (curr == '.') continue;

                    if (!antennas.ContainsKey(curr))
                    {
                        antennas[curr] = new List<Coordinate>();
                    }
                    antennas[curr].Add(new Coordinate { X = x, Y = y });
                }
            }

            Coordinate currCoord;
            Coordinate testCoord;
            List<Coordinate> antCoords;
            Vector currVector;
            foreach (char antennaType in antennas.Keys)
            {
                antCoords = antennas[antennaType];
                for (int i = 0;  i < antCoords.Count; i++)
                {
                    currCoord = antCoords[i];
                    for (int j = i + 1; j < antCoords.Count; j++)
                    {
                        testCoord = antCoords[j];
                        currVector = currCoord.GetVector(testCoord);
                        antinodes.Add(testCoord.AddVector(currVector));
                        antinodes.Add(currCoord.AddVector(currVector.Reversed));
                    }
                }
            }

            long count = 0;

            foreach (var  ant in antinodes)
            {
                if (ant.X < 0 || ant.Y < 0 || ant.X >= xLength || ant.Y >= yLength) continue;
                count++;
            }

            return count;



        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CharMultiArrayFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);

            var antennas = new Dictionary<char, List<Coordinate>>();
            int yLength = content.GetLength(0);
            int xLength = content.GetLength(1);
            char curr;
            HashSet<Coordinate> antinodes = new HashSet<Coordinate>();
            for (int y = 0; y < yLength; y++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    curr = content[y, x];
                    if (curr == '.') continue;

                    if (!antennas.ContainsKey(curr))
                    {
                        antennas[curr] = new List<Coordinate>();
                    }
                    antennas[curr].Add(new Coordinate { X = x, Y = y });
                }
            }

            List<Coordinate> antCoords;

            foreach (char antennaType  in antennas.Keys)
            {
                antCoords = antennas[antennaType];
                antinodes = antinodes.Union(GetAntinodes(antCoords, xLength, yLength)).ToHashSet();
            }

            long count = 0;

            foreach (var ant in antinodes)
            {
                if (ant.X < 0 || ant.Y < 0 || ant.X >= xLength || ant.Y >= yLength) continue;
                count++;
            }

            return count;
        }

        private HashSet<Coordinate> GetAntinodes(List<Coordinate> antennas, int xLength, int yLength)
        {
            Coordinate curr;
            Coordinate test;
            Vector vector;
            HashSet<Coordinate> antinodes = new HashSet<Coordinate>();
            for (int i = 0; i < antennas.Count; i++)
            {
                curr = antennas[i];
                for (int j = i + 1; j < antennas.Count; j++)
                {
                    test = antennas[j];
                    vector = curr.GetVector(test);
                    antinodes = antinodes.Union(AddVectorTillMaxed(curr, vector.Reversed, xLength, yLength)).ToHashSet();
                    antinodes = antinodes.Union(AddVectorTillMaxed(test, vector, xLength, yLength)).ToHashSet();
                }
            }
            return antinodes;
        }

        private HashSet<Coordinate> AddVectorTillMaxed(Coordinate coordinate, Vector vector, int xLength, int yLength)
        {
            Coordinate newCoord = coordinate;
            HashSet<Coordinate> newSet = new HashSet<Coordinate>();
            while (true)
            {
                newSet.Add(newCoord);
                newCoord = newCoord.AddVector(vector);
                if (newCoord.X < 0 || newCoord.Y < 0 || newCoord.X >= xLength || newCoord.Y >= yLength) 
                { 
                    break;
                }
            }
            return newSet;
        }
        private record Coordinate
        {
            public required int X { get; set; }
            public required int Y { get; set; }

            public override int GetHashCode()
            {
                return HashCode.Combine(X.GetHashCode(), Y.GetHashCode());
            }

            public Vector GetVector(Coordinate other)
            {
                return new Vector() { X = other.X - this.X, Y = other.Y - this.Y };
            }

            public Coordinate AddVector(Vector vector)
            {
                return new Coordinate() { X = this.X + vector.X, Y = this.Y + vector.Y };
            }
        }

        private record Vector
        {
            public required int X { get; set; }
            public required int Y { get; set; }

            public Vector Reversed
            {
                get
                {
                    return new Vector() { X = X * -1, Y = Y * -1 };
                }
            }
        }

    }
}
