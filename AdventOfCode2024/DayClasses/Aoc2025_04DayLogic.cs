using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc2025_04DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new()
        {
            { 1, new() { { 1, 13 } } },
            { 2, new() { { 1, 43 } } },
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CharMultiArrayFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);

            long result = 0;
            for (int y = 0; y < content.GetLength(0); y++)
            {
                for (int x = 0; x < content.GetLength(1); x++)
                {
                    if (content[y, x] == '@' && IsAccessable(content, y, x, 3))
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        bool IsAccessable(char[,] area, int y, int x, int maxSurrounding)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    int newY = y + i;
                    int newX = x + j;
                    if (newY >= 0 && newY < area.GetLength(0) && newX >= 0 && newX < area.GetLength(1))
                    {
                        if (area[newY, newX] == '@')
                        {
                            count++;
                            if (count > maxSurrounding)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }



        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CharMultiArrayFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);

            long result = 0;
            long oldResult = -1;
            char[,] updatedContent;
            while (result != oldResult)
            {
                oldResult = result;
                updatedContent = new char[content.GetLength(0), content.GetLength(1)];
                for (int y = 0; y < content.GetLength(0); y++)
                {
                    for (int x = 0; x < content.GetLength(1); x++)
                    {
                        if (content[y, x] == '@' && IsAccessable(content, y, x, 3))
                        {

                            result++;
                            updatedContent[y, x] = '.';
                        }
                        else
                        {
                            updatedContent[y, x] = content[y, x];
                        }
                    }
                }
                content = updatedContent;
            }


            return result;
        }
    }
}
