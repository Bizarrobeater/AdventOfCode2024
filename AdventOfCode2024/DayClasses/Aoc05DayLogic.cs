using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

using static System.Runtime.InteropServices.JavaScript.JSType;

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

            var rules = CreateRulesDict(rawRules);

            int[] temp;
            long result = 0;
            foreach (var update in updates)
            {
                temp = update.Split(',').Select(x => int.Parse(x)).ToArray();
                if (IsValidUpdate(temp, rules))
                {
                    result += temp[(temp.Length / 2)];
                }
            }

            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CleanFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);

            var splitContent = content.Split(Environment.NewLine + Environment.NewLine);
            var rawRules = splitContent[0].Split(Environment.NewLine);
            var updates = splitContent[1].Split(Environment.NewLine);

            var rules = CreateRulesDict(rawRules);
            int[] temp;
            long result = 0;
            int[] newOrder;
            int count;
            foreach (var update in updates)
            {
                temp = update.Split(',').Select(x => int.Parse(x)).ToArray();
                if (IsValidUpdate(temp, rules)) continue;

                newOrder = new int[temp.Length];

                for (int i = 0; i < temp.Length; i++)
                {
                    count = temp.Count(x => rules.ContainsKey(temp[i]) ? rules[temp[i]].Contains(x) : false);
                    newOrder[temp.Length - count - 1] = temp[i];
                }

                result += newOrder[(newOrder.Length / 2)];
            }

            return result;
        }

        private bool IsValidUpdate(int[] update, Dictionary<int, HashSet<int>> rules)
        {
            for (int i = update.Length - 1; i > 0; i--)
            {
                if (!rules.ContainsKey(update[i]))
                    continue;

                for (int j = 0; j < i; j++)
                {
                    if (rules[update[i]].Contains(update[j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private Dictionary<int, HashSet<int>> CreateRulesDict(string[] rawRules) 
        {
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

            return rules;
        }
    }
}
