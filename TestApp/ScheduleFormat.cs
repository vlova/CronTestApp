namespace TestApp
{
    public record ScheduleFormat(
        ScheduleFormatEntry Years, 
        ScheduleFormatEntry Months, 
        ScheduleFormatEntry Days,
        ScheduleFormatEntry Hours, 
        ScheduleFormatEntry Minutes, 
        ScheduleFormatEntry Seconds,
        ScheduleFormatEntry Milliseconds);
}