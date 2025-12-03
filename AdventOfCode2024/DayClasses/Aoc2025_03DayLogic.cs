using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

using CommunityToolkit.HighPerformance;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc2025_03DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new()
        {
            { 1, new() { { 1, 357 } } },
            { 2, new() { { 1, 3_121_910_778_619 } } },
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new IntMultiArrayFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark).AsSpan2D();

            long result = 0;

            for (int i = 0; i < content.Height; i++)
            {
                result += FindHighestInRow(content.GetRow(i).ToArray());
            }
            return result;

        }

        public int FindHighestInRow(int?[] row)
        {
            int? currHighest = 0;
            int? highestFirstNumber = 0;
            int? highestSecondNumber = 0;

            for (int i = 0; i < row.Length; i++)
            {
                if (row[i] <= highestFirstNumber) continue;

                highestFirstNumber = row[i];
                for (int j = i + 1; j < row.Length; j++)
                {
                    if (row[j] <= highestSecondNumber) continue;

                    highestSecondNumber = row[j];
                    currHighest = highestFirstNumber * 10 + highestSecondNumber;
                }
                highestSecondNumber = 0;
            }
            return currHighest ?? -1;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new IntMultiArrayFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark).AsSpan2D();

            long result = 0;

            for (int i = 0; i < content.Height; i++)
            {
                result += FindHigest12NumberInRow(content.GetRow(i).ToArray());
            }
            return result;
        }

        public long FindHigest12NumberInRow(int?[] row)
        {
            long result = 0;

            int removedDigits = 0;
            int maxRemoveDigits = row.Length - 12;

            int currValue = 0;

            int digitCount = 11;

            bool wasRemoved = false;

            for (int i = 0; i < row.Length; i++)
            {
                wasRemoved = false;
                if (digitCount < 0) break;
                currValue = row[i] ?? -1;
                if (currValue == -1) continue;
                if (removedDigits >= maxRemoveDigits)
                {
                    result += currValue * (long)Math.Pow(10, digitCount);
                    digitCount--;
                    continue;
                }
                if (i == row.Length - 1)
                {
                    result += currValue;
                    break;
                }

                for (int j = i + 1; j <= i + maxRemoveDigits - removedDigits && j < row.Length; j++)
                {
                    if (currValue < row[j])
                    {
                        removedDigits++;
                        wasRemoved = true;
                        break;
                    }
                }

                if (wasRemoved)
                {
                    continue;
                }
                
                result += currValue * (long)Math.Pow(10, digitCount);
                digitCount--;
            }

            return result;
        }
    }
}
