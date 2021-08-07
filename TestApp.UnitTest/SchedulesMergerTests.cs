using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace TestApp.UnitTest
{
    public class SchedulesMergerTests
    {
        [Fact]
        public void SinglePoint()
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
            AssertSingleValidPoint(merged.Months, 9);
            AssertSingleValidPoint(merged.Days, 1);
            AssertSingleValidPoint(merged.DayOfWeek, 1);
            AssertSingleValidPoint(merged.Hours, 10);
            AssertSingleValidPoint(merged.Minutes, 0);
            AssertSingleValidPoint(merged.Seconds, 0);
            AssertSingleValidPoint(merged.Milliseconds, 0);
        }
        
        [Fact]
        public void Asterisk()
        {
            var format = new ScheduleFormat(new ScheduleDate(
                    new[] {ScheduleFormatEntry.Always},
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
            AssertOnlyValidPoints(merged.Years,
                Enumerable.Range(Constant.MinYear, Constant.MaxYear - Constant.MinYear + 1).ToHashSet());
            AssertSingleValidPoint(merged.Months, 9);
            AssertSingleValidPoint(merged.Days, 1);
            AssertSingleValidPoint(merged.DayOfWeek, 1);
            AssertSingleValidPoint(merged.Hours, 10);
            AssertSingleValidPoint(merged.Minutes, 0);
            AssertSingleValidPoint(merged.Seconds, 0);
            AssertSingleValidPoint(merged.Milliseconds, 0);
        }
        
        [Fact]
        public void Range()
        {
            var format = new ScheduleFormat(new ScheduleDate(
                    new[] {new ScheduleFormatEntry(2000, 2050, null)},
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
            AssertOnlyValidPoints(merged.Years,
                Enumerable.Range(2000, 51).ToHashSet());
            AssertSingleValidPoint(merged.Months, 9);
            AssertSingleValidPoint(merged.Days, 1);
            AssertSingleValidPoint(merged.DayOfWeek, 1);
            AssertSingleValidPoint(merged.Hours, 10);
            AssertSingleValidPoint(merged.Minutes, 0);
            AssertSingleValidPoint(merged.Seconds, 0);
            AssertSingleValidPoint(merged.Milliseconds, 0);
        }     
        
        [Fact]
        public void RangeWithStep()
        {
            var format = new ScheduleFormat(new ScheduleDate(
                    new[] {new ScheduleFormatEntry(2000, 2050, 3)},
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
            AssertOnlyValidPoints(merged.Years,
                Enumerable.Range(2000, 51).Where(x => (x-2000)%3==0).ToHashSet());
            AssertSingleValidPoint(merged.Months, 9);
            AssertSingleValidPoint(merged.Days, 1);
            AssertSingleValidPoint(merged.DayOfWeek, 1);
            AssertSingleValidPoint(merged.Hours, 10);
            AssertSingleValidPoint(merged.Minutes, 0);
            AssertSingleValidPoint(merged.Seconds, 0);
            AssertSingleValidPoint(merged.Milliseconds, 0);
        }
        
        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        private void AssertSingleValidPoint(ScheduleInterval interval, int point)
        {
            AssertOnlyValidPoints(interval, new HashSet<int> {point});
        }
        
        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        private void AssertOnlyValidPoints(ScheduleInterval interval, HashSet<int> validPoints)
        {
            foreach (var (p, isAllowed) in interval)
            {
                Assert.True(isAllowed == validPoints.Contains(p), $"Point {p} is {(isAllowed ? "allowed" : "disallowed")} when it shouldn't");
            }
        }
    }
}