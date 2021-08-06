using System.Linq;
using Pidgin;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

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
    }
}