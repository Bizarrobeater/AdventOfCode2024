using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc09DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new()
        {
            { 1, new() { { 1, 1928 } } },
            { 1, new() { { 1, 2858 } } }
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CleanFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);
            var intContent = content.Select(x => int.Parse(x.ToString())).ToArray();
            int totalLength = 0;
            foreach (var x in intContent) 
            {
                totalLength += x;
            }
            Span<int?> disk = new int?[totalLength];

            int fileIndex = 0;
            int diskIndex = 0;
            for (int i = 0; i < intContent.Length; i++)
            {
                if (i % 2 != 0)
                {
                    diskIndex += intContent[i];
                    continue;
                }
                for (int j = 0; j < intContent[i]; j++)
                {
                    disk[diskIndex] = fileIndex;
                    diskIndex++;
                }
                fileIndex++;
            }

            int lastCheckIndex = 0;

            for (int i = disk.Length - 1; i > lastCheckIndex; i--)
            {
                if (disk[i] == null) continue;

                for (int j = lastCheckIndex; j < i; j++)
                {
                    if (disk[j] != null) continue;

                    disk[j] = disk[i];
                    disk[i] = null;
                    lastCheckIndex = j;
                    break;
                }

            }

            long result = 0;

            for (int i = 0; i < disk.Length; i++)
            {
                if (disk[i] == null) continue;
                result += disk[i].Value * i;
            }
            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CleanFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);
            var intContent = content.Select(x => int.Parse(x.ToString())).ToArray();
            int totalLength = 0;
            foreach (var x in intContent)
            {
                totalLength += x;
            }
            Span<int?> disk = new int?[totalLength];

            int fileIndex = 0;
            int diskIndex = 0;
            for (int i = 0; i < intContent.Length; i++)
            {
                if (i % 2 != 0)
                {
                    diskIndex += intContent[i];
                    continue;
                }
                for (int j = 0; j < intContent[i]; j++)
                {
                    disk[diskIndex] = fileIndex;
                    diskIndex++;
                }
                fileIndex++;
            }

            throw new NotImplementedException();
        }
    }
}
