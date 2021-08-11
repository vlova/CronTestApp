namespace TestApp.ByPzixelLova
{
    public record ScheduleFormatEntry(int? Begin, int? End, int? Step)
    {
        public static ScheduleFormatEntry Always { get; } = new(null, null, null);
        public static ScheduleFormatEntry SinglePoint(int point) => new(point, null, null);
    }
}