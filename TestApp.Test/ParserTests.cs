using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pidgin;

namespace TestApp.Test
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Asterisk()
        {
            ParserHelper.Asterisk.ParseOrThrow("*");
        }

        [TestMethod]
        public void Number()
        {
            Assert.AreEqual(0, ParserHelper.NumberParser.ParseOrThrow("0"));
            Assert.AreEqual(1, ParserHelper.NumberParser.ParseOrThrow("1"));
            Assert.AreEqual(11, ParserHelper.NumberParser.ParseOrThrow("11"));
            Assert.AreEqual(int.MaxValue, ParserHelper.NumberParser.ParseOrThrow($"{int.MaxValue}"));
            Assert.ThrowsException<ParseException>(() => ParserHelper.NumberParser.ParseOrThrow("aasg0"));
        }
        
        [TestMethod]
        public void Interval()
        {
            Assert.AreEqual((0, null), ParserHelper.IntervalParser.ParseOrThrow("0"));
            Assert.AreEqual((1, null), ParserHelper.IntervalParser.ParseOrThrow("1"));
            Assert.AreEqual((1, 10), ParserHelper.IntervalParser.ParseOrThrow("1-10"));
            Assert.ThrowsException<ParseException>(() => ParserHelper.IntervalParser.ParseOrThrow("aasg0"));
            Assert.ThrowsException<ParseException>(() => ParserHelper.IntervalParser.ParseOrThrow("1-"));
            Assert.ThrowsException<ParseException>(() => ParserHelper.IntervalParser.ParseOrThrow("-10"));
            Assert.ThrowsException<ParseException>(() => ParserHelper.IntervalParser.ParseOrThrow("1-a"));
        }

        private static DateTime ParseTime(string timeStr)
        {
            return DateTime.ParseExact(timeStr, "yyyy-MM-dd H:mm:ss.fff", CultureInfo.InvariantCulture,
                DateTimeStyles.None);
        }
    }
}