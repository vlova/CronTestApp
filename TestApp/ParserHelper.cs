using System.Linq;
using Pidgin;
using static Pidgin.Parser;
using static Pidgin.Parser<char>; 

namespace TestApp
{
    public static class ParserHelper
    {
        public static Parser<char, Unit> Asterisk = Char('*').Map(_ => Unit.Value);
        public static Parser<char, int> NumberParser = Digit.AtLeastOnce().Map(s => int.Parse(new string(s.ToArray())));
        public static Parser<char, (int begin, int? end)> IntervalParser =
            from begin in NumberParser
            from end in Try(Char('-').Then(NumberParser).Optional()).Map(x => x.HasValue ? x.GetValueOrDefault() : default(int?))
            select (begin, end);
        
        // public static Parser<char, (int? begin, int? end)> WholeIntervalParser =
        //     Try(Asterisk).Or(NumberParser)
    }
}