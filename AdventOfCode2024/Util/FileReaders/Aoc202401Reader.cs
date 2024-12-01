using AdventOfCodeApp.Util.FileReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Util.FileReaders
{
    internal class Aoc202401Reader : BaseFileReader<(List<long>, List<long>)>
    {
        protected override (List<long>, List<long>) ConvertFileContentToReadable(string content)
        {
            var split = content.Split(Environment.NewLine);
            string[] strings;
            List<long> list1 = new List<long>();
            List<long> list2 = new List<long>();
            long temp;
            foreach (var item in split)
            {
                strings = item.Split("   ");
                temp = long.Parse(strings[0]);
                list1.Add(temp);

                temp = long.Parse(strings[1]);
                list2.Add(temp);
            }

            return (list1, list2);
        }
    }
}
