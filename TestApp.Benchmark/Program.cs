using System;
using System.Globalization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using TestApp.ByNovar0;
using TestApp.ByPzixel;
using TestApp.ByPzixelLova;

namespace TestApp.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }

    [SimpleJob(RuntimeMoniker.Net50)]
    public class ParsingBench
    {
        [Params("*.9.*/2 1-5 10:00:00.000", "*.4.6,7 * *:*:*.1,2,3-5,10-20/3", "2100.12.31 23:59:59.999",
            "*.*.1 0:0:0")]
        public string Pattern = null!;

        [Benchmark(Baseline = true)]
        public object ParsingOld()
        {
            return new Novar0Schedule(Pattern);
        }

        [Benchmark]
        public object ParsingNew()
        {
            return new PzixelSchedule(Pattern);
        }
    }

    [SimpleJob(RuntimeMoniker.Net50)]
    public class FindNextBench
    {
        [Params("*.9.*/2 1-5 10:00:00.000", "*.4.6,7 * *:*:*.1,2,3-5,10-20/3", "2100.12.31 23:59:59.999",
            "*.*.1 0:0:0")]
        public string Pattern = null!;

        [Params("2001-01-01", "2080-05-05")] public string DateString = null!;

        private DateTime Date;
        private Novar0Schedule Novar0Schedule = null!;
        private PzixelSchedule PzixelSchedule = null!;
        private PzixelLovaSchedule PzixelLovaSchedule = null!;

        [GlobalSetup]
        public void Setup()
        {
            Date = DateTime.ParseExact(DateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Novar0Schedule = new Novar0Schedule(Pattern);
            PzixelSchedule = new PzixelSchedule(Pattern);
            PzixelLovaSchedule = new PzixelLovaSchedule(Pattern);
        }

        [Benchmark(Baseline = true)]
        public object FindNextNovar0() => Novar0Schedule.NextEvent(Date);

        [Benchmark]
        public object FindNextPzixel() => PzixelSchedule.NextEvent(Date);

        [Benchmark]
        public object FindNextPzixelLovaSortedSetCached() => PzixelLovaSchedule.NextEvent(Date);
    }

    [SimpleJob(RuntimeMoniker.Net50)]
    public class FindPrevBench
    {
        [Params("*.9.*/2 1-5 10:00:00.000", "*.4.6,7 * *:*:*.1,2,3-5,10-20/3", "2000.12.31 23:59:59.999",
            "*.*.1 0:0:0")]
        public string Pattern = null!;

        [Params("2001-01-01", "2080-05-05")] public string DateString = null!;

        private DateTime Date;
        private Novar0Schedule Novar0Schedule = null!;
        private PzixelSchedule PzixelSchedule = null!;
        private PzixelLovaSchedule PzixelLovaSchedule = null!;

        [GlobalSetup]
        public void Setup()
        {
            Date = DateTime.ParseExact(DateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Novar0Schedule = new Novar0Schedule(Pattern);
            PzixelSchedule = new PzixelSchedule(Pattern);
            PzixelLovaSchedule = new PzixelLovaSchedule(Pattern);
        }

        [Benchmark(Baseline = true)]
        public object FindPrevNovar0() => Novar0Schedule.PrevEvent(Date);

        [Benchmark]
        public object FindPrevPzixel() => PzixelSchedule.PrevEvent(Date);

        [Benchmark]
        public object FindPrevPzixelLovaSortedSetCached() => PzixelLovaSchedule.PrevEvent(Date);
    }
}