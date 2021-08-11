using System;
using System.Runtime.CompilerServices;

#pragma warning disable 8509

namespace TestApp.ByPzixel
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
        DateTime Change<TIsIncrementing>(DateTime t1) where TIsIncrementing : struct, IBool;
    }

    public struct YearsChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1) where TIsIncrementing : struct, IBool
        {
            var baseValue = new DateTime(t1.Year, 1, 1);
            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddYears(1),
                FalseType _ => baseValue.AddMilliseconds(-1)
            };
        }
    }

    public struct MonthChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1) where TIsIncrementing : struct, IBool
        {
            var baseValue = new DateTime(t1.Year, t1.Month, 1);
            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddMonths(1),
                FalseType _ => baseValue.AddMilliseconds(-1)
            };
        }
    }

    public struct DayChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1) where TIsIncrementing : struct, IBool
        {
            var baseValue = t1.Date;
            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddDays(1),
                FalseType _ => baseValue.AddMilliseconds(-1)
            };
        }
    }

    public struct HourChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1) where TIsIncrementing : struct, IBool
        {
            var baseValue = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, 0, 0);
            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddHours(1),
                FalseType _ => baseValue.AddMilliseconds(-1)
            };
        }
    }

    public struct MinuteChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1) where TIsIncrementing : struct, IBool
        {
            var baseValue = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, 0);
            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddMinutes(1),
                FalseType _ => baseValue.AddMilliseconds(-1)
            };
        }
    }

    public struct SecondChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1) where TIsIncrementing : struct, IBool
        {
            var baseValue = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, t1.Second);
            return default(TIsIncrementing) switch
            {
                TrueType _ => baseValue.AddSeconds(1),
                FalseType _ => baseValue.AddMilliseconds(-1)
            };
        }
    }
    public struct MillisecondChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1) where TIsIncrementing : struct, IBool =>
            default(TIsIncrementing) switch
            {
                TrueType _ => t1.AddMilliseconds(1),
                FalseType _ => t1.AddMilliseconds(-1)
            };
    }
}