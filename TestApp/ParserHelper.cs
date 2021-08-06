using System;
using System.Collections.Generic;
using System.Linq;
using Pidgin;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;
using Char = System.Char;

namespace TestApp
{
    public static class ParserHelper
    {
        public static Parser<char, Unit> Asterisk { get; } = Char('*').Map(_ => Unit.Value);
        public static Parser<char, int> NumberParser { get; } = Digit.AtLeastOnce().Map(s => int.Parse(new string(s.ToArray())));

        public static Parser<char, (int begin, int? end)> IntervalParser { get; } =
            from begin in NumberParser
            from end in Try(Char('-').Then(NumberParser).Optional())
                .Map(x => x.HasValue ? x.GetValueOrDefault() : default(int?))
            select (begin, end);

        public static Parser<char, ScheduleFormatEntry> WholeIntervalParser { get; } =
            from interval in
                Try(Asterisk.Map(_ => (begin: default(int?), end: default(int?))))
                    .Or(IntervalParser.Map(x => ((int?) x.begin, x.end)))
            from step in Try(Char('/').Then(NumberParser).Optional())
                .Map(x => x.HasValue ? x.GetValueOrDefault() : default(int?))
            select new ScheduleFormatEntry(interval.begin, interval.end, step);

        public static Parser<char, ScheduleFormatEntry[]> IntervalsSequenceParser { get; } =
            Validate(WholeIntervalParser.SeparatedAndOptionallyTerminatedAtLeastOnce(Char(',')).Map(x => x.ToArray()), ValidateWildcards);

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
        public static Parser<char, ScheduleDate> DateParser { get; } =
            from years in Validate(IntervalsSequenceParser, ValidateBounds(2000, 2100))
            from _ in Char('.')
            from months in Validate(IntervalsSequenceParser, ValidateBounds(1, 12))
            from __ in Char('.')
            from days in Validate(IntervalsSequenceParser, ValidateBounds(1, 32))
            select new ScheduleDate(years, months, days);
        
        public static Parser<char, DayOfWeek> DayOfWeekParser { get; } =
            from num in NumberParser
            let x = Enum.IsDefined(typeof(DayOfWeek), num) 
                ? (DayOfWeek) num 
                : throw new ScheduleFormatException($"Cannot parse {num} as day of week")
            select x;

        private static ScheduleFormatEntry[] Validate(ScheduleFormatEntry[] x, Action<ScheduleFormatEntry[]> check)
        {
            check(x);
            return x;
        }
        
        private static Parser<char, ScheduleFormatEntry[]> Validate(Parser<char, ScheduleFormatEntry[]> parser, Action<ScheduleFormatEntry[]> check) =>
            parser.Map(x =>
            {
                check(x);
                return x;
            });

        private static void ValidateWildcards(ScheduleFormatEntry[] entries)
        {
            if (entries.Length > 1 && entries.Any(x => x == new ScheduleFormatEntry(null, null, null)))
            {
                throw new ScheduleFormatException("Cannot have more than one wildcard entry in schedule");
            }
        }

        private static Action<ScheduleFormatEntry[]> ValidateBounds(int min, int max) =>
            entries =>
            {
                if (entries.Any(x => x.Begin < min || x.Begin > max || x.End < x.Begin || x.End > max))
                {
                    throw new ScheduleFormatException("Datetime component is out of bounds");
                }
            };
    }
}