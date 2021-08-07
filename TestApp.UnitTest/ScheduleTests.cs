using System;
using System.Diagnostics.CodeAnalysis;
using Pidgin;
using Xunit;

namespace TestApp.UnitTest
{
    public class ScheduleTests
    {
        [Fact]
        public void Asterisk()
        {
            var format = new ScheduleFormat(new ScheduleDate(
                    new[] {ScheduleFormatEntry.SinglePoint(2020)},
                    new[] {ScheduleFormatEntry.SinglePoint(9)},
                    new[] {ScheduleFormatEntry.SinglePoint(1)}
                ),
                new[] {ScheduleFormatEntry.SinglePoint(1)},
                new ScheduleTime(
                    new[] {ScheduleFormatEntry.SinglePoint(10)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)},
                    new[] {ScheduleFormatEntry.SinglePoint(0)}
                ));
            var merged = MergedSchedule.FromFormat(format);
            AssertSingleValidPoint(merged.Years, 2020);
        }
        
        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        private void AssertSingleValidPoint(ScheduleInterval interval, int point)
        {
            foreach (var (p, isAllowed) in interval)
            {
                Assert.True(isAllowed == (p == point), $"Point {p} is {(isAllowed ? "allowed" : "disallowed")} when it shouldn't");
            }
        }
    }
}