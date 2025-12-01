using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc2025_01DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new() 
        { 
            { 1, new() { { 1, 3 } } },
            { 2, new() { { 1, 6 } } },
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new LineSplitFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);

            int dialPos = 50;
            char direction;
            int steps;
            long result = 0;
            var remainderSteps = 0;
            foreach (var line in content)
            {
                direction = line[0];
                steps = int.Parse(line[1..]);
                remainderSteps = steps % 100;
                if (direction == 'L')
                {
                    if (remainderSteps > dialPos)
                    {
                        dialPos += 100; // * (int)(Math.Ceiling((double)steps / 100));
                    }
                    dialPos -= remainderSteps;
                }
                else if (direction == 'R')
                {
                    if (dialPos + remainderSteps >= 100)
                    {
                        dialPos -= 100; // * (int)(Math.Ceiling((double)steps / 100));
                    }
                    dialPos += remainderSteps;
                }
                if (dialPos == 100)
                {
                    dialPos = 0;
                }

                if (dialPos == 0)
                {
                    result++;
                }                
            }

            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new LineSplitFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);

            int dialPos = 50;
            char direction;
            int steps;
            long result = 0;
            var remainderSteps = 0;
            var passes = 0;
            foreach (var line in content)
            {
                direction = line[0];
                steps = int.Parse(line[1..]);
                remainderSteps = steps % 100;
                passes = (steps - remainderSteps) / 100;

                if (direction == 'L')
                {
                    if (remainderSteps > dialPos)
                    {
                        dialPos += 100; // * (int)(Math.Ceiling((double)steps / 100));
                    }
                    dialPos -= remainderSteps;
                }
                else if (direction == 'R')
                {
                    if (dialPos + remainderSteps >= 100)
                    {
                        dialPos -= 100; // * (int)(Math.Ceiling((double)steps / 100));
                    }
                    dialPos += remainderSteps;
                }
                if (dialPos == 100)
                {
                    dialPos = 0;
                }

                if (dialPos == 0)
                {
                    result++;
                }
                result += passes;
            }

            return result;
        }
    }
}
