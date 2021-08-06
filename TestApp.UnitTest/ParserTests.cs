using System;
using System.Globalization;
using System.Linq;
using Pidgin;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;
using Xunit;

namespace TestApp.UnitTest
{
    public class ParserTests
    {
        [Fact]
        public void Asterisk()
        {
            ParserHelper.Asterisk.ParseOrThrow("*");
        }

        [Fact]
        public void Number()
        {
            Assert.Equal(0, ParserHelper.NumberParser.ParseOrThrow("0"));
            Assert.Equal(1, ParserHelper.NumberParser.ParseOrThrow("1"));
            Assert.Equal(11, ParserHelper.NumberParser.ParseOrThrow("11"));
            Assert.Equal(int.MaxValue, ParserHelper.NumberParser.ParseOrThrow($"{int.MaxValue}"));
            Assert.Throws<ParseException>(() => ParserHelper.NumberParser.ParseOrThrow("aasg0"));
        }

        [Fact]
        public void Interval()
        {
            Assert.Equal((0, null), ParserHelper.IntervalParser.ParseOrThrow("0"));
            Assert.Equal((1, null), ParserHelper.IntervalParser.ParseOrThrow("1"));
            Assert.Equal((1, 10), ParserHelper.IntervalParser.ParseOrThrow("1-10"));
            Assert.Throws<ParseException>(() => ParserHelper.IntervalParser.ParseOrThrow("aasg0"));
            Assert.Throws<ParseException>(() => ParserHelper.IntervalParser.ParseOrThrow("1-"));
            Assert.Throws<ParseException>(() => ParserHelper.IntervalParser.ParseOrThrow("-10"));
            Assert.Throws<ParseException>(() => ParserHelper.IntervalParser.ParseOrThrow("1-a"));
        }

        [Fact]
        public void WholeInterval()
        {
            Assert.Equal(new ScheduleFormatEntry(0, null, null), ParserHelper.WholeIntervalParser.ParseOrThrow("0"));
            Assert.Equal(new ScheduleFormatEntry(1, null, null), ParserHelper.WholeIntervalParser.ParseOrThrow("1"));
            Assert.Equal(new ScheduleFormatEntry(1, 10, null),
                ParserHelper.WholeIntervalParser.ParseOrThrow("1-10"));
            Assert.Equal(new ScheduleFormatEntry(null, null, null),
                ParserHelper.WholeIntervalParser.ParseOrThrow("*"));
            Assert.Equal(new ScheduleFormatEntry(null, null, 24),
                ParserHelper.WholeIntervalParser.ParseOrThrow("*/24"));
            Assert.Equal(new ScheduleFormatEntry(1, null, 24),
                ParserHelper.WholeIntervalParser.ParseOrThrow("1/24"));
            Assert.Equal(new ScheduleFormatEntry(1, 15, 24),
                ParserHelper.WholeIntervalParser.ParseOrThrow("1-15/24"));
            Assert.Throws<ParseException>(() => ParserHelper.WholeIntervalParser.ParseOrThrow("aasg0"));
            Assert.Throws<ParseException>(() => ParserHelper.WholeIntervalParser.ParseOrThrow("1-"));
            Assert.Throws<ParseException>(() => ParserHelper.WholeIntervalParser.ParseOrThrow("-10"));
            Assert.Throws<ParseException>(() => ParserHelper.WholeIntervalParser.ParseOrThrow("1-a"));
            Assert.Throws<ParseException>(() => ParserHelper.WholeIntervalParser.ParseOrThrow("*/*"));
            Assert.Throws<ParseException>(() =>
                ParserHelper.WholeIntervalParser.Then(End).ParseOrThrow("1-15-*/12"));
            Assert.Throws<ParseException>(() => ParserHelper.WholeIntervalParser.ParseOrThrow("-*/12"));
        }

        [Fact]
        public void IntervalsSequence()
        {
            Assert.Equal(new[] {new ScheduleFormatEntry(0, null, null)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("0"));
            Assert.Equal(new[] {new ScheduleFormatEntry(1, null, null)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("1"));
            Assert.Equal(new[] {new ScheduleFormatEntry(1, 10, null)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("1-10"));
            Assert.Equal(new[] {new ScheduleFormatEntry(null, null, null)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("*"));
            Assert.Equal(new[] {new ScheduleFormatEntry(null, null, 24)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("*/24"));
            Assert.Equal(new[] {new ScheduleFormatEntry(1, null, 24)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("1/24"));
            Assert.Equal(new[] {new ScheduleFormatEntry(1, 15, 24)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("1-15/24"));
            Assert.Equal(new[] {new ScheduleFormatEntry(0, null, null), new ScheduleFormatEntry(1, 15, 24)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("0,1-15/24"));
            Assert.Equal(new[]
                {
                    new ScheduleFormatEntry(1, null, null),
                    new ScheduleFormatEntry(2, null, null),
                    new ScheduleFormatEntry(3, 5, null),
                    new ScheduleFormatEntry(10, 20, 3)
                },
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("1,2,3-5,10-20/3"));
            Assert.Throws<ParseException>(() => ParserHelper.IntervalsSequenceParser.ParseOrThrow("aasg0"));
            Assert.Throws<ParseException>(() => ParserHelper.IntervalsSequenceParser.ParseOrThrow("1-"));
            Assert.Throws<ParseException>(() => ParserHelper.IntervalsSequenceParser.ParseOrThrow("-10"));
            Assert.Throws<ParseException>(() => ParserHelper.IntervalsSequenceParser.ParseOrThrow("1-a"));
            Assert.Throws<ParseException>(() => ParserHelper.IntervalsSequenceParser.ParseOrThrow("*/*"));
            Assert.Throws<ParseException>(() =>
                ParserHelper.IntervalsSequenceParser.Then(End).ParseOrThrow("1-15-*/12"));
            Assert.Throws<ParseException>(() => ParserHelper.IntervalsSequenceParser.ParseOrThrow("-*/12"));
            Assert.Throws<ScheduleFormatException>(() => ParserHelper.IntervalsSequenceParser.ParseOrThrow("*,*"));
            Assert.Throws<ScheduleFormatException>(() => ParserHelper.IntervalsSequenceParser.ParseOrThrow("*,*/12"));
        }

        [Fact]
        public void Date()
        {
             AssertEqualDates(new ScheduleDate(
                     new[] {new ScheduleFormatEntry(2011, null, null)},
                     new[] {new ScheduleFormatEntry(1, null, null)},
                     new[] {new ScheduleFormatEntry(1, null, null)}
                 ),
                 ParserHelper.DateParser.ParseOrThrow("2011.01.01"));
             AssertEqualDates(new ScheduleDate(
                     new[]
                     {
                         new ScheduleFormatEntry(2010, null, null),
                         new ScheduleFormatEntry(2011, 2020, 3)
                     },
                     new[] {new ScheduleFormatEntry(1, null, null)},
                     new[] {new ScheduleFormatEntry(1, null, null)}
                 ),
                 ParserHelper.DateParser.ParseOrThrow("2010,2011-2020/3.01.01"));
             AssertEqualDates(new ScheduleDate(
                     new[] {new ScheduleFormatEntry(null, null, null)},
                     new[] {new ScheduleFormatEntry(9, null, null)},
                     new[] {new ScheduleFormatEntry(null, null, 2)}
                 ),
                 ParserHelper.DateParser.ParseOrThrow("*.9.*/2"));
            
            Assert.Throws<ScheduleFormatException>(() => ParserHelper.DateParser.ParseOrThrow("1800.01.01"));
            Assert.Throws<ScheduleFormatException>(() => ParserHelper.DateParser.ParseOrThrow("2050.15.03"));
            Assert.Throws<ScheduleFormatException>(() => ParserHelper.DateParser.ParseOrThrow("2050.04.48"));
        }
        
        [Fact]
        public void DayOfWeek()
        {
            foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
            {
                Assert.Equal(dayOfWeek, ParserHelper.DayOfWeekParser.ParseOrThrow(dayOfWeek.ToString("D")));
            }
            Assert.Throws<ScheduleFormatException>(() => ParserHelper.DayOfWeekParser.ParseOrThrow("7"));
        }

        private static void AssertEqualDates(ScheduleDate expected, ScheduleDate actual)
        {
            Assert.Equal(expected.Years, actual.Years);
            Assert.Equal(expected.Months, actual.Months);
            Assert.Equal(expected.Days, actual.Days);
        }
    }
}