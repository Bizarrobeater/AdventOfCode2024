using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc04DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {
                1,
                new Dictionary<int, long>() {
                    {1, 18 }
                }
            },
            {
                2,
                new Dictionary<int, long>()
                {
                    {1, 9 }
                }
            }
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CharMultiArrayFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);
            var result = 0;

            (int x, int y) curr;

            for (int x = 0; x < content.GetLength(0); x++)
            {
                for (int y = 0; y < content.GetLength(1); y++)
                {
                    if (content[x, y] != 'X') continue;

                    curr = new(x, y);
                    result += CountCardinal(content, curr) + CountDiagonal(content, curr);
                }
            }
            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            throw new NotImplementedException();
        }

        private int CountCardinal(char[,] input, (int x, int y) curr)
        {
            var count = 0;

            if (CheckRight(input, curr))
                count++;
            if (CheckLeft(input, curr))
                count++;
            if (CheckUp(input, curr))
                count++;
            if (CheckDown(input, curr))
                count++;

            return count;
        }

        private int CountDiagonal(char[,] input, (int x, int y) curr)
        {
            var count = 0;

            if (CheckDiagRightUp(input, curr))
                count++;
            if (CheckDiagRightDown(input, curr))
                count++;
            if (CheckDiagLeftUp(input, curr))
                count++;
            if (CheckDiagLeftDown(input, curr))
                count++;

            return count;
        }

        private bool CheckRight(char[,] input, (int x, int y) curr)
        {
            if (curr.x + 3 < input.GetLength(0) && input[curr.x + 1, curr.y] == 'M' && input[curr.x + 2, curr.y] == 'A' && input[curr.x + 3, curr.y] == 'S') return true;
            return false;
        }

        private bool CheckLeft(char[,] input, (int x, int y) curr)
        {
            if (curr.x - 3 >= 0 && input[curr.x - 1, curr.y] == 'M' && input[curr.x - 2, curr.y] == 'A' && input[curr.x - 3, curr.y] == 'S') return true;
            return false;
        }

        private bool CheckUp(char[,] input, (int x, int y) curr)
        {
            if (curr.y - 3 >= 0 && input[curr.x, curr.y-1] == 'M' && input[curr.x, curr.y - 2] == 'A' && input[curr.x, curr.y - 3] == 'S') return true;
            return false;
        }

        private bool CheckDown(char[,] input, (int x, int y) curr)
        {
            if (curr.y + 3 < input.GetLength(1) && input[curr.x, curr.y + 1] == 'M' && input[curr.x, curr.y + 2] == 'A' && input[curr.x, curr.y + 3] == 'S') return true;
            return false;
        }

        private bool CheckDiagRightUp(char[,] input, (int x, int y) curr)
        {
            if (curr.x + 3 < input.GetLength(0) && curr.y + 3 < input.GetLength(1) 
                && input[curr.x + 1, curr.y + 1] == 'M' && input[curr.x + 2, curr.y + 2] == 'A' && input[curr.x + 3, curr.y + 3] == 'S') 
                return true;
            return false;
        }

        private bool CheckDiagRightDown(char[,] input, (int x, int y) curr)
        {
            if (curr.x + 3 < input.GetLength(0) && curr.y - 3 >= 0
                && input[curr.x + 1, curr.y - 1] == 'M' && input[curr.x + 2, curr.y - 2] == 'A' && input[curr.x + 3, curr.y - 3] == 'S')
                return true;
            return false;
        }

        private bool CheckDiagLeftUp(char[,] input, (int x, int y) curr)
        {
            if (curr.x - 3 >= 0 && curr.y + 3 < input.GetLength(1)
                && input[curr.x - 1, curr.y + 1] == 'M' && input[curr.x - 2, curr.y + 2] == 'A' && input[curr.x - 3, curr.y + 3] == 'S')
                return true;
            return false;
        }

        private bool CheckDiagLeftDown(char[,] input, (int x, int y) curr)
        {
            if (curr.x - 3 >= 0 && curr.y - 3 >= 0
                && input[curr.x - 1, curr.y - 1] == 'M' && input[curr.x - 2, curr.y - 2] == 'A' && input[curr.x - 3, curr.y - 3] == 'S')
                return true;
            return false;
        }
    }
}
