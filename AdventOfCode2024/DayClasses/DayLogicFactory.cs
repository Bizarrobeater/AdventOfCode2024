using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace AdventOfCodeApp.DayClasses
{
    internal class DayLogicFactory
    {
        public static IDayLogic? CreateDayLogic(AdventOfCode adventOfCode)
        {
            string dayLogicName = $"Aoc{adventOfCode.Day.ToString("00")}DayLogic";
            return Assembly.GetExecutingAssembly().CreateInstance($"AdventOfCode2024.DayClasses.{dayLogicName}") as IDayLogic;
        }
    }
}