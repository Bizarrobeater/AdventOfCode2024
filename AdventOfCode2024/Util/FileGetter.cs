using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.Util
{
    internal class FileGetter
    {
        public int Year { get; set; } = 2024;
        public int Day { get; set; }

        public string BaseFileName => $"Adv{Year}{Day.ToString("00")}";

        public List<FileInfo>? BenchMarkFile { get; private set; }

        public FileGetter(int day)
        {
            Day = day;
        }

        public FileGetter(int year, int day) : this(day)
        {
            Year = year;
        }

        private DirectoryInfo GetFileFolder()
        {
            string target = "InputFiles";
            string currDirString = Directory.GetCurrentDirectory();
            DirectoryInfo currDir = new DirectoryInfo(currDirString);
            bool isBaseFolder = false;
            DirectoryInfo[] currDirContains;
            DirectoryInfo foundDir = new DirectoryInfo(target);

            while (!isBaseFolder && currDir.Exists)
            {
                currDirContains = currDir.GetDirectories();
                foreach (var dir in currDirContains)
                {
                    if (dir.Name == target)
                    {
                        foundDir = dir;
                        isBaseFolder = true;
                        break;
                    }

                }
                currDir = currDir.Parent ?? new DirectoryInfo(target);
            }
            return foundDir;
        }

        public List<FileInfo> GetFiles(bool isTest = false, bool isBenchMark = false)
        {
            if (isBenchMark && BenchMarkFile != null)
                return BenchMarkFile;

            var fileDir = GetFileFolder();
            if (!fileDir.Exists)
                throw new Exception("Some error regarding file folders");

            Func<FileInfo, bool> fileFunction = isTest ? IsTestFile : IsActualFile;

            List<FileInfo> result = new List<FileInfo>();

            foreach (FileInfo file in fileDir.GetFiles())
            {
                if (fileFunction(file))
                {
                    result.Add(file);
                }
            }

            if (isBenchMark)
                BenchMarkFile = result;
            return result;
        }

        public bool IsTestFile(FileInfo file)
        {
            return file.Name.StartsWith($"{BaseFileName}-");
        }

        public bool IsActualFile(FileInfo file)
        {
            return file.Name == $"{BaseFileName}.txt";
        }
    }
}
