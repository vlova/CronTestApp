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
            var nextValue = interval.NextPoint(t1.Year);
            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddYears(nextValue - t1.Year),
                FalseType _ => baseValue.AddMilliseconds(-1)
            };
        }
    }

    public struct MonthChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1, ScheduleInterval interval) where TIsIncrementing : struct, IBool
        {
            var baseValue = new DateTime(t1.Year, t1.Month, 1);
            var nextValue = interval.NextPoint(t1.Month);

            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddMonths(nextValue - t1.Month),
                FalseType _ => baseValue.AddMilliseconds(-1)
            };
        }
    }

    public struct DayChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1, ScheduleInterval interval) where TIsIncrementing : struct, IBool
        {
            var baseValue = t1.Date;

            var nextValue = interval.NextPoint(t1.Day);
            nextValue = nextValue == 32
                ? (DateTime.DaysInMonth(t1.Year, t1.Month) == t1.Day
                    ? (t1.Day + 1)
                    : DateTime.DaysInMonth(t1.Year, t1.Month))
                : nextValue;

            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddDays(nextValue - t1.Day),
                FalseType _ => baseValue.AddMilliseconds(-1)
            };
        }
    }

    public struct HourChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1, ScheduleInterval interval) where TIsIncrementing : struct, IBool
        {
            var baseValue = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, 0, 0);
            var nextValue = interval.NextPoint(t1.Hour);
            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddHours(nextValue - t1.Hour),
                FalseType _ => baseValue.AddMilliseconds(-1)
            };
        }
    }

    public struct MinuteChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1, ScheduleInterval interval) where TIsIncrementing : struct, IBool
        {
            var baseValue = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, 0);
            var nextValue = interval.NextPoint(t1.Minute);
            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddMinutes(nextValue - t1.Minute),
                FalseType _ => baseValue.AddMilliseconds(-1)
            };
        }
    }

    public struct SecondChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1, ScheduleInterval interval) where TIsIncrementing : struct, IBool
        {
            var baseValue = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, t1.Second);
            var nextValue = interval.NextPoint(t1.Second);
            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddSeconds(nextValue - t1.Second),
                FalseType _ => baseValue.AddMilliseconds(-1)
            };
        }
    }
    public struct MillisecondChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1, ScheduleInterval interval) where TIsIncrementing : struct, IBool
        {
            var nextValue = interval.NextPoint(t1.Millisecond);
            return default(TIsIncrementing) switch
            {
                TrueType _ => t1.AddMilliseconds(nextValue - t1.Millisecond),
                FalseType _ => t1.AddMilliseconds(-1)
            };
        }
    }
}