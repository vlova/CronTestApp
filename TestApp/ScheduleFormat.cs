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
}