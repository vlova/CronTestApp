using System;
using System.Linq;
using Pidgin;
using static Pidgin.Parser<char>;
using Xunit;
using TestApp.ByPzixel;

namespace TestApp.UnitTest
{
    public class PzixelParserTests
    {
        [Fact]
        public void Asterisk()
        {
            PzixelParserHelper.Asterisk.ParseOrThrow("*");
            Assert.Throws<ParseException>(() => PzixelParserHelper.NumberParser.ParseOrThrow("hello"));
        }

        [Fact]
        public void Number()
        {
            Assert.Equal(0, PzixelParserHelper.NumberParser.ParseOrThrow("0"));
            Assert.Equal(1, PzixelParserHelper.NumberParser.ParseOrThrow("1"));
            Assert.Equal(11, PzixelParserHelper.NumberParser.ParseOrThrow("11"));
            Assert.Equal(int.MaxValue, PzixelParserHelper.NumberParser.ParseOrThrow($"{int.MaxValue}"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.NumberParser.ParseOrThrow("aasg0"));
        }

        [Fact]
        public void Interval()
        {
            Assert.Equal((0, null), PzixelParserHelper.IntervalParser.ParseOrThrow("0"));
            Assert.Equal((1, null), PzixelParserHelper.IntervalParser.ParseOrThrow("1"));
            Assert.Equal((1, 10), PzixelParserHelper.IntervalParser.ParseOrThrow("1-10"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.IntervalParser.ParseOrThrow("aasg0"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.IntervalParser.ParseOrThrow("1-"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.IntervalParser.ParseOrThrow("-10"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.IntervalParser.ParseOrThrow("1-a"));
        }

        [Fact]
        public void WholeInterval()
        {
            Assert.Equal(ScheduleFormatEntry.SinglePoint(0), PzixelParserHelper.WholeIntervalParser.ParseOrThrow("0"));
            Assert.Equal(ScheduleFormatEntry.SinglePoint(1), PzixelParserHelper.WholeIntervalParser.ParseOrThrow("1"));
            Assert.Equal(new ScheduleFormatEntry(1, 10, null),
                PzixelParserHelper.WholeIntervalParser.ParseOrThrow("1-10"));
            Assert.Equal(ScheduleFormatEntry.Always,
                PzixelParserHelper.WholeIntervalParser.ParseOrThrow("*"));
            Assert.Equal(new ScheduleFormatEntry(null, null, 24),
                PzixelParserHelper.WholeIntervalParser.ParseOrThrow("*/24"));
            Assert.Equal(new ScheduleFormatEntry(1, null, 24),
                PzixelParserHelper.WholeIntervalParser.ParseOrThrow("1/24"));
            Assert.Equal(new ScheduleFormatEntry(1, 15, 24),
                PzixelParserHelper.WholeIntervalParser.ParseOrThrow("1-15/24"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.WholeIntervalParser.ParseOrThrow("aasg0"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.WholeIntervalParser.ParseOrThrow("1-"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.WholeIntervalParser.ParseOrThrow("-10"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.WholeIntervalParser.ParseOrThrow("1-a"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.WholeIntervalParser.ParseOrThrow("*/*"));
            Assert.Throws<ParseException>(() =>
                PzixelParserHelper.WholeIntervalParser.Then(End).ParseOrThrow("1-15-*/12"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.WholeIntervalParser.ParseOrThrow("-*/12"));
        }

        [Fact]
        public void IntervalsSequence()
        {
            Assert.Equal(new[] {ScheduleFormatEntry.SinglePoint(0)},
                PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("0"));
            Assert.Equal(new[] {ScheduleFormatEntry.SinglePoint(1)},
                PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("1"));
            Assert.Equal(new[] {new ScheduleFormatEntry(1, 10, null)},
                PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("1-10"));
            Assert.Equal(new[] {ScheduleFormatEntry.Always},
                PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("*"));
            Assert.Equal(new[] {new ScheduleFormatEntry(null, null, 24)},
                PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("*/24"));
            Assert.Equal(new[] {new ScheduleFormatEntry(1, null, 24)},
                PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("1/24"));
            Assert.Equal(new[] {new ScheduleFormatEntry(1, 15, 24)},
                PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("1-15/24"));
            Assert.Equal(new[] {ScheduleFormatEntry.SinglePoint(0), new ScheduleFormatEntry(1, 15, 24)},
                PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("0,1-15/24"));
            Assert.Equal(new[]
                {
                    ScheduleFormatEntry.SinglePoint(1),
                    ScheduleFormatEntry.SinglePoint(2),
                    new ScheduleFormatEntry(3, 5, null),
                    new ScheduleFormatEntry(10, 20, 3)
                },
                PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("1,2,3-5,10-20/3"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("aasg0"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("1-"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("-10"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("1-a"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("*/*"));
            Assert.Throws<ParseException>(() =>
                PzixelParserHelper.IntervalsSequenceParser.Then(End).ParseOrThrow("1-15-*/12"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("-*/12"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("*,*"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.IntervalsSequenceParser.ParseOrThrow("*,*/12"));
        }

        [Fact]
        public void Date()
        {
             AssertEqualDates(new ScheduleDate(
                     new[] {ScheduleFormatEntry.SinglePoint(2011)},
                     new[] {ScheduleFormatEntry.SinglePoint(1)},
                     new[] {ScheduleFormatEntry.SinglePoint(1)}
                 ),
                 PzixelParserHelper.DateParser.ParseOrThrow("2011.01.01"));
             AssertEqualDates(new ScheduleDate(
                     new[]
                     {
                         ScheduleFormatEntry.SinglePoint(2010),
                         new ScheduleFormatEntry(2011, 2020, 3)
                     },
                     new[] {ScheduleFormatEntry.SinglePoint(1)},
                     new[] {ScheduleFormatEntry.SinglePoint(1)}
                 ),
                 PzixelParserHelper.DateParser.ParseOrThrow("2010,2011-2020/3.01.01"));
             AssertEqualDates(new ScheduleDate(
                     new[] {ScheduleFormatEntry.Always},
                     new[] {ScheduleFormatEntry.SinglePoint(9)},
                     new[] {new ScheduleFormatEntry(null, null, 2)}
                 ),
                 PzixelParserHelper.DateParser.ParseOrThrow("*.9.*/2"));
            
            Assert.Throws<ParseException>(() => PzixelParserHelper.DateParser.ParseOrThrow("1800.01.01"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.DateParser.ParseOrThrow("2050.15.03"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.DateParser.ParseOrThrow("2050.04.48"));
        }
        
        [Fact]
        public void DayOfWeek()
        {
            foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
            {
                Assert.Equal((int) dayOfWeek, PzixelParserHelper.DayOfWeekParser.ParseOrThrow(dayOfWeek.ToString("D")).Single().Begin);
            }
            Assert.Equal(new []
            {
                ScheduleFormatEntry.SinglePoint(1),
                new ScheduleFormatEntry(2, 4, null)
            }, PzixelParserHelper.DayOfWeekParser.ParseOrThrow("1,2-4"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.DayOfWeekParser.ParseOrThrow("7"));
        }

        [Fact]
        public void Time()
        {
            AssertEqualTimes(new ScheduleTime(
                    new[] {ScheduleFormatEntry.SinglePoint(10)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)}
                ),
                PzixelParserHelper.TimeParser.ParseOrThrow("10:00:00.000"));
            
            AssertEqualTimes(new ScheduleTime(
                    new[] {ScheduleFormatEntry.SinglePoint(10)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)}
                ),
                PzixelParserHelper.TimeParser.ParseOrThrow("10:00:00"));
            
            AssertEqualTimes(new ScheduleTime(
                    new[] {ScheduleFormatEntry.Always},
                    new[] {ScheduleFormatEntry.SinglePoint(0)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)}
                ),
                PzixelParserHelper.TimeParser.ParseOrThrow("*:00:00"));
            
            AssertEqualTimes(new ScheduleTime(
                    new[] {ScheduleFormatEntry.Always},
                    new[] {ScheduleFormatEntry.Always},
                    new[] {ScheduleFormatEntry.Always},
                    new[] {ScheduleFormatEntry.SinglePoint(0)}
                ),
                PzixelParserHelper.TimeParser.ParseOrThrow("*:*:*"));
            
            AssertEqualTimes(new ScheduleTime(
                    new[] {ScheduleFormatEntry.Always},
                    new[] {ScheduleFormatEntry.Always},
                    new[] {ScheduleFormatEntry.Always},
                    new[] {ScheduleFormatEntry.Always}
                ),
                PzixelParserHelper.TimeParser.ParseOrThrow("*:*:*.*"));
            
            Assert.Throws<ParseException>(() => PzixelParserHelper.TimeParser.ParseOrThrow("24:00:00.000"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.TimeParser.ParseOrThrow("00:60:00.000"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.TimeParser.ParseOrThrow("00:00:60.000"));
            Assert.Throws<ParseException>(() => PzixelParserHelper.TimeParser.ParseOrThrow("00:00:00.1000"));
        }

        [Fact]
        public void FullFormat()
        {
            AssertEqualFormat(new ScheduleFormat(new ScheduleDate(
                        new[] {ScheduleFormatEntry.Always},
                        new[] {ScheduleFormatEntry.SinglePoint(9)},
                        new[] {new ScheduleFormatEntry(null, null, 2)}
                    ),
                    new []{new ScheduleFormatEntry(1, 5, null)},
                    new ScheduleTime(
                        new[] {ScheduleFormatEntry.SinglePoint(10)},
                        new[] {ScheduleFormatEntry.SinglePoint(0)},
                        new[] {ScheduleFormatEntry.SinglePoint(0)},
                        new[] {ScheduleFormatEntry.SinglePoint(0)}
                    )),
                PzixelParserHelper.FullFormatParser.ParseOrThrow("*.9.*/2 1-5 10:00:00.000"));
            
            AssertEqualFormat(new ScheduleFormat(new ScheduleDate(
                        new[] {ScheduleFormatEntry.Always},
                        new[] {ScheduleFormatEntry.Always},
                        new[] {ScheduleFormatEntry.Always}
                    ),
                    new []{new ScheduleFormatEntry(1, 5, null)},
                    new ScheduleTime(
                        new[] {ScheduleFormatEntry.SinglePoint(10)},
                        new[] {ScheduleFormatEntry.SinglePoint(0)},
                        new[] {ScheduleFormatEntry.SinglePoint(0)},
                        new[] {ScheduleFormatEntry.SinglePoint(0)}
                    )),
                PzixelParserHelper.FullFormatParser.ParseOrThrow("1-5 10:00:00.000"));
            
            AssertEqualFormat(new ScheduleFormat(new ScheduleDate(
                        new[] {ScheduleFormatEntry.Always},
                        new[] {ScheduleFormatEntry.Always},
                        new[] {ScheduleFormatEntry.Always}
                    ),
                    new []{ScheduleFormatEntry.Always},
                    new ScheduleTime(
                        new[] {ScheduleFormatEntry.SinglePoint(10)},
                        new[] {ScheduleFormatEntry.SinglePoint(0)},
                        new[] {ScheduleFormatEntry.SinglePoint(0)},
                        new[] {ScheduleFormatEntry.SinglePoint(0)}
                    )),
                PzixelParserHelper.FullFormatParser.ParseOrThrow("10:00:00.000"));
            
            AssertEqualFormat(new ScheduleFormat(new ScheduleDate(
                        new[] {ScheduleFormatEntry.Always},
                        new[] {ScheduleFormatEntry.Always},
                        new[] {ScheduleFormatEntry.SinglePoint(1)}
                    ),
                    new []{ScheduleFormatEntry.Always},
                    new ScheduleTime(
                        new[] {ScheduleFormatEntry.SinglePoint(1)},
                        new[] {ScheduleFormatEntry.SinglePoint(30)},
                        new[] {ScheduleFormatEntry.SinglePoint(0)},
                        new[] {ScheduleFormatEntry.SinglePoint(0)}
                    )),
                PzixelParserHelper.FullFormatParser.ParseOrThrow("*.*.01 01:30:00"));
            
            AssertEqualFormat(new ScheduleFormat(new ScheduleDate(
                        new[] {ScheduleFormatEntry.Always},
                        new[] {ScheduleFormatEntry.Always},
                        new[] {ScheduleFormatEntry.Always}
                    ),
                    new []{ScheduleFormatEntry.Always},
                    new ScheduleTime(
                        new[] {ScheduleFormatEntry.Always},
                        new[] {ScheduleFormatEntry.SinglePoint(0)},
                        new[] {ScheduleFormatEntry.SinglePoint(0)},
                        new[] {ScheduleFormatEntry.SinglePoint(0)}
                    )),
                PzixelParserHelper.FullFormatParser.ParseOrThrow("*:00:00"));
        }

        private static void AssertEqualDates(ScheduleDate expected, ScheduleDate actual)
        {
            Assert.Equal(expected.Years, actual.Years);
            Assert.Equal(expected.Months, actual.Months);
            Assert.Equal(expected.Days, actual.Days);
        }

        private static void AssertEqualTimes(ScheduleTime expected, ScheduleTime actual)
        {
            Assert.Equal(expected.Hours, actual.Hours);
            Assert.Equal(expected.Minutes, actual.Minutes);
            Assert.Equal(expected.Seconds, actual.Seconds);
            Assert.Equal(expected.Milliseconds, actual.Milliseconds);
        }        
        
        private static void AssertEqualFormat(ScheduleFormat expected, ScheduleFormat actual)
        {
            AssertEqualDates(expected.Date, actual.Date);
            Assert.Equal(expected.DayOfWeek, actual.DayOfWeek);
            AssertEqualTimes(expected.Time, actual.Time);
        }
        
    }
}