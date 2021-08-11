using System;
using System.Runtime.CompilerServices;

#pragma warning disable 8509

namespace TestApp.ByPzixelLova
{
    public interface IBool
    {
    }

    public struct TrueType : IBool
    {
    }

    public struct FalseType : IBool
    {
    }

    public interface IDateTimeChanger
    {
        DateTime Change<TIsIncrementing>(DateTime t1, ScheduleInterval interval) where TIsIncrementing : struct, IBool;
    }

    public struct YearsChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1, ScheduleInterval interval) where TIsIncrementing : struct, IBool
        {
            var baseValue = new DateTime(t1.Year, 1, 1);

            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddYears(interval.NextPoint(t1.Year) - t1.Year),
                FalseType _ => baseValue.AddYears(interval.PreviousPoint(t1.Year) - t1.Year + 1).AddMilliseconds(-1)
            };
        }
    }

    public struct MonthChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1, ScheduleInterval interval) where TIsIncrementing : struct, IBool
        {
            var baseValue = new DateTime(t1.Year, t1.Month, 1);

            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddMonths(interval.NextPoint(t1.Month) - t1.Month),
                FalseType _ => baseValue.AddMonths(interval.PreviousPoint(t1.Month) - t1.Month + 1).AddMilliseconds(-1)
            };
        }
    }

    public struct DayChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1, ScheduleInterval interval) where TIsIncrementing : struct, IBool
        {
            var baseValue = t1.Date;

            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddDays(GetNextValue(t1, interval) - t1.Day),
                FalseType _ => baseValue.AddDays(interval.PreviousPoint(t1.Day) - t1.Day + 1).AddMilliseconds(-1)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static int GetNextValue(DateTime t1, ScheduleInterval interval)
        {
            var nextValue = interval.NextPoint(t1.Day);

            if (nextValue == 32)
            {
                nextValue = (DateTime.DaysInMonth(t1.Year, t1.Month) == t1.Day
                    ? (t1.Day + 1)
                    : DateTime.DaysInMonth(t1.Year, t1.Month));
            }

            return nextValue;
        }
    }

    public struct HourChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1, ScheduleInterval interval) where TIsIncrementing : struct, IBool
        {
            var baseValue = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, 0, 0);

            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddHours(interval.NextPoint(t1.Hour) - t1.Hour),
                FalseType _ => baseValue.AddHours(interval.PreviousPoint(t1.Hour) - t1.Hour + 1).AddMilliseconds(-1)
            };
        }
    }

    public struct MinuteChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1, ScheduleInterval interval) where TIsIncrementing : struct, IBool
        {
            var baseValue = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, 0);

            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddMinutes(interval.NextPoint(t1.Minute) - t1.Minute),
                FalseType _ => baseValue.AddMinutes(interval.PreviousPoint(t1.Minute) - t1.Minute + 1).AddMilliseconds(-1)
            };
        }
    }

    public struct SecondChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1, ScheduleInterval interval) where TIsIncrementing : struct, IBool
        {
            var baseValue = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, t1.Second);

            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddSeconds(interval.NextPoint(t1.Second) - t1.Second),
                FalseType _ => baseValue.AddSeconds(interval.PreviousPoint(t1.Second) - t1.Second + 1).AddMilliseconds(-1)
            };
        }
    }
    public struct MillisecondChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1, ScheduleInterval interval) where TIsIncrementing : struct, IBool
        {
            return default(TIsIncrementing) switch
            {
                TrueType _ => t1.AddMilliseconds(interval.NextPoint(t1.Millisecond) - t1.Millisecond),
                FalseType _ => t1.AddMilliseconds(interval.PreviousPoint(t1.Millisecond) - t1.Millisecond + 1).AddMilliseconds(-1)
            };
        }
    }
}