using System;

namespace TestApp
{
    public record ScheduleFormat(
        ScheduleDate Date,
        ScheduleFormatEntry[] DayOfWeek,
        ScheduleTime Time);
    
    public record ScheduleDate(
        ScheduleFormatEntry[] Years, 
        ScheduleFormatEntry[] Months, 
        ScheduleFormatEntry[] Days);
    
    public record ScheduleTime(
        ScheduleFormatEntry[] Hours, 
        ScheduleFormatEntry[] Minutes, 
        ScheduleFormatEntry[] Seconds,
        ScheduleFormatEntry[] Milliseconds);

    public record MergedSchedule(
        ScheduleInterval Years,
        ScheduleInterval Months,
        ScheduleInterval Days,
        ScheduleInterval DayOfWeek,
        ScheduleInterval Hours,
        ScheduleInterval Minutes,
        ScheduleInterval Seconds)
    {
        public static MergedSchedule FromFormat(ScheduleFormat format)
        {
            return new MergedSchedule(GetMerged(format.Date.Years, Constant.MinYear, Constant.MaxYear), 
                default!, default!, default!, default!, default!, default!);
        }

        private static ScheduleInterval GetMerged(ScheduleFormatEntry[] intervals, int begin, int end)
        {
            if (intervals.Length == 1 && intervals[0] == ScheduleFormatEntry.Always)
            {
                return ScheduleInterval.CreateAllowedInterval(begin, end);
            }
            
            var result = new ScheduleInterval(begin, end);
            foreach (var scheduleFormatEntry in intervals)
            {
                var (b, e, s) = (scheduleFormatEntry.Begin, scheduleFormatEntry.End, scheduleFormatEntry.Step) switch
                {
                    (null, null, {} s1) => (begin, end, s1),
                    ({} b2, null, null) => (b2, b2, 1),
                    ({} b3, {} e3, null) => (b3, e3, 1),
                    ({} b4, {} e4, {} s4) => (b4, e4, s4),
                    _ => throw new InvalidOperationException($"Bad period {scheduleFormatEntry} cannot be parsed")
                };
                for (int i = b; i <= e; i += s)
                {
                    result.ChangePointAllowance(i, true);
                }
            }

            return result;
        }
    }
}