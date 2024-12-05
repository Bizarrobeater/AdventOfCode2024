using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc05DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {
                1,
                new Dictionary<int, long>()
                {
                    {
                        1,
                        143
                    }
                }
            },
            {
                2,
                new Dictionary<int, long>()
                {
                    {
                        1, 
                        123
                    }
                }
            }
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CleanFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);

            var splitContent = content.Split(Environment.NewLine + Environment.NewLine);
            var rawRules = splitContent[0].Split(Environment.NewLine);
            var updates = splitContent[1].Split(Environment.NewLine);

            var rules = new Dictionary<int, HashSet<int>>();
            string[] temp;
            int num;
            int ahead;
            foreach (var rule in rawRules)
            {
                temp = rule.Split('|');
                num = int.Parse(temp[0]); 
                ahead = int.Parse(temp[1]);
                if (!rules.ContainsKey(num))
                {
                    rules.Add(num, new HashSet<int>());
                }
                rules[num].Add(ahead);
            }
            int[] tempUp;
            bool valid;
            long result = 0;
            int curr;
            int currCheck;
            foreach (var update in updates)
            {
                valid = true;
                tempUp = update.Split(',').Select(x => int.Parse(x)).ToArray();
                for (int i = tempUp.Length - 1; i > 0; i--)
                {
                    curr = tempUp[i];
                    if (!rules.ContainsKey(tempUp[i])) 
                        continue;

                    for (int j = 0; j < i; j++)
                    {
                        currCheck = tempUp[j];
                        if (rules[tempUp[i]].Contains(tempUp[j])){
                            valid = false;
                            break;
                        }
                    }
                }
                if (valid)
                {
                    result += tempUp[(tempUp.Length / 2)];
                }
            }




            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            throw new NotImplementedException();
        }
    }
}
