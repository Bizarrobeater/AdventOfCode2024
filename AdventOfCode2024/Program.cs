using AdventOfCodeApp.DayClasses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace AdventOfCodeApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddHttpClient();

            IHost host = builder.Build();

            var client = host.Services.GetRequiredService<HttpClient>();

            var app = new AdventOfCode();

            //app.RunTest(1);
            //app.RunActual(1);

            //app.RunTest(2);
            app.RunActual(2);
            //Benchmark(app, 1, "micro");
            //Benchmark(app, 2, "milli");
            //Benchmark(app, 2, "micro");
            //Console.ReadKey();
        }

        public static void Benchmark(AdventOfCode app, int question, string type = "milli")
        {
            int runs = 1_0;
            List<long> timeTaken = new List<long>();
            Dictionary<long, int> resultAmounts = new Dictionary<long, int>();
            long time;
            Func<int, long> benchmarkFunction;
            if (type == "milli")
                benchmarkFunction = app.RunActualBenchmarkMilliseconds;
            else if (type == "micro")
                benchmarkFunction = app.RunActualBenchmarkMicroseconds;
            else
                benchmarkFunction = app.RunActualBenchmarkTicks;

            for (int i = 0; i < runs; i++)
            {
                time = benchmarkFunction(question);
                timeTaken.Add(time);
                if (!resultAmounts.ContainsKey(time))
                    resultAmounts[time] = 0;
                resultAmounts[time]++;
            }
            timeTaken.Sort();
            string explainText = type != "milli" && type != "micro" ? "in ticks" : $"in {type}seconds";
            Console.WriteLine($"Benchmark {explainText}:");
            Console.WriteLine($"First Run Time: {timeTaken[0]}");
            Console.WriteLine($"Last Run Time: {timeTaken[timeTaken.Count - 1]}");
            Console.WriteLine($"Average: {timeTaken.Average()}");
            Console.WriteLine($"Median: {timeTaken[timeTaken.Count / 2]}");
            Console.WriteLine($"Max Time: {timeTaken.Max()}");
            Console.WriteLine($"Min Time: {timeTaken.Min()}");

            if (type != "milli" && type != "micro")
                return;

            Console.WriteLine("Result counts:");
            List<long> uniqueTimes = resultAmounts.Keys.ToList();
            uniqueTimes.Sort();
            foreach (long uniqueTime in uniqueTimes)
            {
                Console.WriteLine($"Time taken - {uniqueTime}, Count - {resultAmounts[uniqueTime]}");
            }
        }
    }
}