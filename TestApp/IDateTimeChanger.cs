using System;
using System.Runtime.CompilerServices;

#pragma warning disable 8509

namespace TestApp
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
        public DateTime Change<TIsIncrementing>(DateTime t1) where TIsIncrementing : struct, IBool =>
            default(TIsIncrementing) switch
            {
                TrueType _ => new DateTime(t1.Year, 1, 1).AddYears(1),
                FalseType _ => new DateTime(t1.Year, 1, 1).AddMilliseconds(-1)
            };
    }
    
    public struct MonthChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1) where TIsIncrementing : struct, IBool =>
            default(TIsIncrementing) switch
            {
                TrueType _ => new DateTime(t1.Year, t1.Month, 1).AddMonths(1),
                FalseType _ => new DateTime(t1.Year, t1.Month, 1).AddMilliseconds(-1)
            };
    }    
    
    public struct DayChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1) where TIsIncrementing : struct, IBool =>
            default(TIsIncrementing) switch
            {
                TrueType _ => t1.Date.AddDays(1),
                FalseType _ => t1.Date.AddMilliseconds(-1)
            };
    }    
    
    public struct HourChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1) where TIsIncrementing : struct, IBool =>
            default(TIsIncrementing) switch
            {
                TrueType _ => new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, 0, 0).AddHours(1),
                FalseType _ => new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, 0, 0).AddMilliseconds(-1)
            };
    }    
    
    public struct MinuteChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1) where TIsIncrementing : struct, IBool =>
            default(TIsIncrementing) switch
            {
                TrueType _ => new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, 0).AddMinutes(1),
                FalseType _ => new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, 0).AddMilliseconds(-1)
            };
    }  
    
    public struct SecondChanger : IDateTimeChanger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public DateTime Change<TIsIncrementing>(DateTime t1) where TIsIncrementing : struct, IBool =>
            default(TIsIncrementing) switch
            {
                TrueType _ => new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, t1.Second).AddSeconds(1),
                FalseType _ => new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, t1.Second).AddMilliseconds(-1)
            };
        
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