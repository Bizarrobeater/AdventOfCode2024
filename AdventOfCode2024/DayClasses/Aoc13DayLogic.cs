using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCodeApp.DayClasses;
using AdventOfCodeApp.Util.FileReaders;

namespace AdventOfCode2024.DayClasses
{
    internal class Aoc13DayLogic : IDayLogic
    {
        const int _default = 100_000_000;
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new()
        {
            { 1, new() { { 1, 480 } } }
        };

        // current wrong results
        /*
         * 46832: 46.832 high
            26670: 26.670 low
            29671: 29.671 low
            33153: 33.153 wrong
            33902: 33.902 wrong
         */


        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CleanFileReader();
            var pureContent = reader.GetReadableFileContent(file, isBenchmark);
            var content = pureContent.Split(Environment.NewLine + Environment.NewLine);
            var arcades = new List<Arcade>();
            
            
            foreach (var item in content)
            {
                arcades.Add(new Arcade(item));
            }

            char buttonType;
            char valueType;
            char testButton;
            char testType;
            int buttonValue;
            int testButtonValue;
            
            int ratio;
            int maxRatio;
            int mainPresses;
            int secPresses;
            int currValue;
            int goal;
            long result = 0;
            long currLowest;
            int hitValue;
            foreach (var arcade in arcades)
            {
                currLowest = _default;
                (buttonType, valueType, ratio) = GetButtonTestType(arcade);
                (testButton, testType) = GetOther((buttonType, valueType));
                
                buttonValue = arcade.GetButton(buttonType).GetValue(valueType);
                testButtonValue = arcade.GetButton(testButton).GetValue(valueType);
                
                goal = arcade.GetGoal(valueType);
                
                //maxRatio = (goal - ((goal - ratio * buttonValue) * testButtonValue)) / buttonValue;
                //var ratioDiff = ratio - maxRatio;
                for (mainPresses = 0; mainPresses < 100; mainPresses++)
                {
                    currValue = goal - buttonValue * mainPresses;
                    if (currValue < 0) break;
                    secPresses = currValue / testButtonValue;
                    if (currValue % testButtonValue == 0 && (arcade.GetGoal(testType) - arcade.GetButton(buttonType).GetValue(testType) * mainPresses) % arcade.GetButton(testButton).GetValue(testType) == 0)
                    {
                        secPresses = currValue / testButtonValue;
                        if (secPresses > 100) continue;
                        if (buttonType == 'A')
                        {
                            hitValue = 3 * mainPresses + 1 * secPresses;
                            
                        }
                        else
                        {
                            hitValue = 3 * secPresses + mainPresses;
                            
                        }
                        currLowest = hitValue < currLowest ? hitValue : currLowest;
                    }
                }
                result += currLowest != _default ? currLowest : 0;
                //}
                //currValue = arcade.GetGoal(valueType) - (arcade.GetButton(buttonType).GetValue(valueType) * (maxRatio / 2));
            }

            return result;
        }

        private (char button, char type, int maxRatio) GetButtonTestType(Arcade arcade)
        {
            bool xValDiff = arcade.A.X - arcade.B.X * 3 >= 0;
            bool yValDiff = arcade.A.Y - arcade.B.Y * 3 >= 0;
            int aXRatio = arcade.XGoal / arcade.A.X;
            int aYRatio = arcade.YGoal / arcade.A.Y;
            int bXRatio = arcade.XGoal / arcade.B.X;
            int bYRatio = arcade.YGoal / arcade.B.Y;

            if (!yValDiff && bYRatio <= bXRatio) 
                return ('B', 'Y', bYRatio);
            else if (!xValDiff) 
                return ('B', 'X', bXRatio);
            else if (aXRatio <= aYRatio) 
                return ('A', 'X', aXRatio);
            else 
                return ('A', 'Y', aYRatio);

            //if (aXRatio <= aYRatio && aXRatio <= bXRatio && aXRatio <= bYRatio) return ('A', 'X', aXRatio);
            //else if (aYRatio <= bXRatio && aYRatio <= bYRatio) return ('A', 'Y', aYRatio);
            //else if (bXRatio <= bYRatio) return ('B', 'X', bXRatio);
            //else return ('B', 'Y', bYRatio);
        }

        private (char button, char type) GetOther((char button, char type) old)
        {
            char newButton;
            char newType;
            if (old.button == 'A') newButton = 'B';
            else newButton = 'A';
            if (old.type == 'X') newType = 'Y';
            else newType = 'X';
            return (newButton, newType);
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            throw new NotImplementedException();
        }

        private record Button
        {
            public int X { get; set; }
            public int Y { get; set; }

            public int GetValue(char type)
            {
                switch (type)
                {
                    case 'X': return X;
                    case 'Y': return Y;
                    default: throw new NotImplementedException();
                }            
            }
            public Button(string input)
            {
                var split = input.Split(": ")[1].Split(' ');
                X = int.Parse(split[0].Split('+')[1].Trim(','));
                Y = int.Parse(split[1].Split('+')[1].Trim(','));
            }
        }

        private record Arcade
        {
            private static Regex _prizeRegex = new Regex(@"Prize: (X=(\d+)), (Y=(\d+))", RegexOptions.Compiled);
            public Button A { get; set; }
            public Button B { get; set; }
            public int XGoal { get; set; }
            public int YGoal { get; set; }

            public Arcade(string input)
            {
                var split = input.Split(Environment.NewLine);
                A = new Button(split[0]);
                B = new Button(split[1]);
                XGoal = int.Parse(_prizeRegex.Match(input).Groups[2].Value);
                YGoal = int.Parse(_prizeRegex.Match(input).Groups[4].Value);
            }

            public int GetGoal(char type)
            {
                switch (type)
                {
                    case 'X': return XGoal;
                    case 'Y': return YGoal;
                    default: throw new NotImplementedException();
                }
            }

            public Button GetButton(char type)
            {
                switch (type)
                {
                    case 'A': return A;
                    case 'B': return B;
                    default: throw new NotImplementedException();
                }
            }


        }
    }
}
