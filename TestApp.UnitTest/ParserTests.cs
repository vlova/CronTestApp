using System;
using System.Linq;
using Pidgin;
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
            Assert.Equal(ScheduleFormatEntry.SinglePoint(0), ParserHelper.WholeIntervalParser.ParseOrThrow("0"));
            Assert.Equal(ScheduleFormatEntry.SinglePoint(1), ParserHelper.WholeIntervalParser.ParseOrThrow("1"));
            Assert.Equal(new ScheduleFormatEntry(1, 10, null),
                ParserHelper.WholeIntervalParser.ParseOrThrow("1-10"));
            Assert.Equal(ScheduleFormatEntry.Always,
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
            Assert.Equal(new[] {ScheduleFormatEntry.SinglePoint(0)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("0"));
            Assert.Equal(new[] {ScheduleFormatEntry.SinglePoint(1)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("1"));
            Assert.Equal(new[] {new ScheduleFormatEntry(1, 10, null)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("1-10"));
            Assert.Equal(new[] {ScheduleFormatEntry.Always},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("*"));
            Assert.Equal(new[] {new ScheduleFormatEntry(null, null, 24)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("*/24"));
            Assert.Equal(new[] {new ScheduleFormatEntry(1, null, 24)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("1/24"));
            Assert.Equal(new[] {new ScheduleFormatEntry(1, 15, 24)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("1-15/24"));
            Assert.Equal(new[] {ScheduleFormatEntry.SinglePoint(0), new ScheduleFormatEntry(1, 15, 24)},
                ParserHelper.IntervalsSequenceParser.ParseOrThrow("0,1-15/24"));
            Assert.Equal(new[]
                {
                    ScheduleFormatEntry.SinglePoint(1),
                    ScheduleFormatEntry.SinglePoint(2),
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
            Assert.Throws<ParseException>(() => ParserHelper.IntervalsSequenceParser.ParseOrThrow("*,*"));
            Assert.Throws<ParseException>(() => ParserHelper.IntervalsSequenceParser.ParseOrThrow("*,*/12"));
        }

        [Fact]
        public void Date()
        {
             AssertEqualDates(new ScheduleDate(
                     new[] {ScheduleFormatEntry.SinglePoint(2011)},
                     new[] {ScheduleFormatEntry.SinglePoint(1)},
                     new[] {ScheduleFormatEntry.SinglePoint(1)}
                 ),
                 ParserHelper.DateParser.ParseOrThrow("2011.01.01"));
             AssertEqualDates(new ScheduleDate(
                     new[]
                     {
                         ScheduleFormatEntry.SinglePoint(2010),
                         new ScheduleFormatEntry(2011, 2020, 3)
                     },
                     new[] {ScheduleFormatEntry.SinglePoint(1)},
                     new[] {ScheduleFormatEntry.SinglePoint(1)}
                 ),
                 ParserHelper.DateParser.ParseOrThrow("2010,2011-2020/3.01.01"));
             AssertEqualDates(new ScheduleDate(
                     new[] {ScheduleFormatEntry.Always},
                     new[] {ScheduleFormatEntry.SinglePoint(9)},
                     new[] {new ScheduleFormatEntry(null, null, 2)}
                 ),
                 ParserHelper.DateParser.ParseOrThrow("*.9.*/2"));
            
            Assert.Throws<ParseException>(() => ParserHelper.DateParser.ParseOrThrow("1800.01.01"));
            Assert.Throws<ParseException>(() => ParserHelper.DateParser.ParseOrThrow("2050.15.03"));
            Assert.Throws<ParseException>(() => ParserHelper.DateParser.ParseOrThrow("2050.04.48"));
        }
        
        [Fact]
        public void DayOfWeek()
        {
            foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
            {
                Assert.Equal((int) dayOfWeek, ParserHelper.DayOfWeekParser.ParseOrThrow(dayOfWeek.ToString("D")).Single().Begin);
            }
            Assert.Throws<ParseException>(() => ParserHelper.DayOfWeekParser.ParseOrThrow("7"));
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
                ParserHelper.TimeParser.ParseOrThrow("10:00:00.000"));
            
            AssertEqualTimes(new ScheduleTime(
                    new[] {ScheduleFormatEntry.SinglePoint(10)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)}
                ),
                ParserHelper.TimeParser.ParseOrThrow("10:00:00"));
            
            AssertEqualTimes(new ScheduleTime(
                    new[] {ScheduleFormatEntry.Always},
                    new[] {ScheduleFormatEntry.SinglePoint(0)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)}
                ),
                ParserHelper.TimeParser.ParseOrThrow("*:00:00"));
            
            AssertEqualTimes(new ScheduleTime(
                    new[] {ScheduleFormatEntry.Always},
                    new[] {ScheduleFormatEntry.Always},
                    new[] {ScheduleFormatEntry.Always},
                    new[] {ScheduleFormatEntry.SinglePoint(0)}
                ),
                ParserHelper.TimeParser.ParseOrThrow("*:*:*"));
            
            AssertEqualTimes(new ScheduleTime(
                    new[] {ScheduleFormatEntry.Always},
                    new[] {ScheduleFormatEntry.Always},
                    new[] {ScheduleFormatEntry.Always},
                    new[] {ScheduleFormatEntry.Always}
                ),
                ParserHelper.TimeParser.ParseOrThrow("*:*:*.*"));
            
            Assert.Throws<ParseException>(() => ParserHelper.TimeParser.ParseOrThrow("24:00:00.000"));
            Assert.Throws<ParseException>(() => ParserHelper.TimeParser.ParseOrThrow("00:60:00.000"));
            Assert.Throws<ParseException>(() => ParserHelper.TimeParser.ParseOrThrow("00:00:60.000"));
            Assert.Throws<ParseException>(() => ParserHelper.TimeParser.ParseOrThrow("00:00:00.1000"));
        }

        /// <summary>
        /// Создает экземпляр из строки с представлением расписания.
        /// </summary>
        /// <param name="scheduleString">Строка расписания.
        /// Формат строки:
        ///     yyyy.MM.dd w HH:mm:ss.fff
        ///     yyyy.MM.dd HH:mm:ss.fff
        ///     HH:mm:ss.fff
        ///     yyyy.MM.dd w HH:mm:ss
        ///     yyyy.MM.dd HH:mm:ss
        ///     HH:mm:ss
        /// Где yyyy - год (2000-2100)
        ///     MM - месяц (1-12)
        ///     dd - число месяца (1-31 или 32). 32 означает последнее число месяца
        ///     w - день недели (0-6). 0 - воскресенье, 6 - суббота
        ///     HH - часы (0-23)
        ///     mm - минуты (0-59)
        ///     ss - секунды (0-59)
        ///     fff - миллисекунды (0-999). Если не указаны, то 0
        /// Каждую часть даты/времени можно задавать в виде списков и диапазонов.
        /// Например:
        ///     1,2,3-5,10-20/3
        ///     означает список 1,2,3,4,5,10,13,16,19
        /// Дробью задается шаг в списке.
        /// Звездочка означает любое возможное значение.
        /// Например (для часов):
        ///     */4
        ///     означает 0,4,8,12,16,20
        /// Вместо списка чисел месяца можно указать 32. Это означает последнее
        /// число любого месяца.
        /// Пример:
        ///     *.9.*/2 1-5 10:00:00.000
        ///     означает 10:00 во все дни с пн. по пт. по нечетным числам в сентябре
        ///     *:00:00
        ///     означает начало любого часа
        ///     *.*.01 01:30:00
        ///     означает 01:30 по первым числам каждого месяца
        /// </param>
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
                ParserHelper.FullFormatParser.ParseOrThrow("*.9.*/2 1-5 10:00:00.000"));
            
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
                ParserHelper.FullFormatParser.ParseOrThrow("1-5 10:00:00.000"));
            
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
                ParserHelper.FullFormatParser.ParseOrThrow("10:00:00.000"));
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