using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc07DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {
                1,
                new Dictionary<int, long>()
                {
                    {
                        1, 3749
                    },
                    {
                        2, 3245122495150
                    }
                }
            },
            {
                2,
                new Dictionary<int, long>()
                {
                    {
                        1, 11387
                    },
                    {
                        2, 105517128211543
                    }
                }
            }
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new LineSplitFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);

            long result = 0;

            foreach (var line in content)
            {
                result += ValidateLine(line);
            }


            return result;
        }

        private long ValidateLine(string line)
        {
            string[] splitLine = line.Split(": ");
            var target = long.Parse(splitLine[0]);

            Span<int> inputs = splitLine[1].Split(" ").Select(x => int.Parse(x)).ToArray();
            
            var topNode = new OperationNode(target, 0, inputs);
            return topNode.Valid ? topNode.Target : 0;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new LineSplitFileReader();
            var content = reader.GetReadableFileContent(file, isBenchmark);

            long result = 0;

            foreach (var line in content)
            {
                result += ValidateLineAdvanced(line);
            }


            return result;
        }
        private long ValidateLineAdvanced(string line)
        {
            string[] splitLine = line.Split(": ");
            var target = long.Parse(splitLine[0]);

            Span<int> inputs = splitLine[1].Split(" ").Select(x => int.Parse(x)).ToArray();

            var topNode = new AdvancedOperationNode(target, 0, inputs);
            return topNode.Valid ? topNode.Target : 0;
        }

        private class OperationNode
        {
            public long Target { get; set; }
            public long Value { get; set; }
            public OperationNode? PlusNode { get; set; }
            public OperationNode? MultNode { get; set; }

            public bool Valid
            {
                get
                {
                    return (PlusNode != null && PlusNode.Valid) 
                        || (MultNode != null && MultNode.Valid) 
                        || (PlusNode == null && MultNode == null && Value == Target);
                }
                
            }

            public OperationNode(long target, long value, Span<int> input)
            {
                Target = target;
                Value = value;
                if (Value <= Target && input.Length > 0)
                {
                    PlusNode = new OperationNode(Target, Value + input[0], input.Slice(1));
                    MultNode = new OperationNode(Target, Value * input[0], input.Slice(1));
                }
            }
        }

        private class AdvancedOperationNode
        {
            public long Target { get; set; }
            public long Value { get; set; }
            public AdvancedOperationNode? PlusNode { get; set; }
            public AdvancedOperationNode? MultNode { get; set; }
            public AdvancedOperationNode? CombineNode { get; set; }


            public bool Valid { get; set; } = false;

            public AdvancedOperationNode(long target, long value, Span<int> input)
            {
                Target = target;
                Value = value;
                if (input.Length > 0)
                {
                    if (Value <= Target)
                    {
                        PlusNode = new AdvancedOperationNode(Target, Value + input[0], input[1..]);
                        if (PlusNode.Valid)
                        {
                            Valid = true;
                            return;
                        }
                        else
                        {
                            PlusNode = null;
                        }
                        if (Value != 0)
                        {
                            MultNode = new AdvancedOperationNode(Target, Value * input[0], input[1..]);
                            if (MultNode.Valid)
                            {
                                Valid = true;
                                return;
                            }
                            else
                            {
                                MultNode = null;
                            }
                            long newValue = 0;
                            if (input[0] < 10)
                                newValue = Value * 10 + input[0];
                            else if (input[0] < 100)
                                newValue = Value * 100 + input[0];
                            else if (input[0] < 1000)
                                newValue = Value * 1000 + input[0];
                            else if (input[0] < 10_000)
                                newValue = Value * 10_000 + input[0];
                            CombineNode = new AdvancedOperationNode(Target, newValue, input[1..]);
                            if (CombineNode.Valid)
                            {
                                Valid = true;
                                return;
                            }
                            else
                            {
                                CombineNode = null;
                            }
                        }
                    }
                }
                else
                {
                    Valid = Value == Target;
                }
            }
        }
    }
}
