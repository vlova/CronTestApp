using System;

namespace TestApp
{
    public class ScheduleFormatException : Exception
    {
        public ScheduleFormatException()
        {
        }

        public ScheduleFormatException(string message) : base(message)
        {
        }
    }
}