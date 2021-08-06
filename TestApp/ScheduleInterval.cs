namespace TestApp
{
    public readonly struct ScheduleInterval
    {
        public int Begin { get; }
        public int End { get; }

        private readonly bool[] _allowedPoints;

        public ScheduleInterval(int begin, int end)
        {
            Begin = begin;
            End = end;
            _allowedPoints = new bool[end - begin + 1];
        }

        public bool IsPointAllowed(int point) => _allowedPoints[Begin + point];
        public bool ChangePointAllowance(int point, bool value) => _allowedPoints[Begin + point] = value;

        public static ScheduleInterval CreateAllowedInterval(int begin, int end)
        {
            var result = new ScheduleInterval(begin, end);
            for (int i = 0; i < result._allowedPoints.Length; i++)
            {
                result._allowedPoints[i] = true;
            }

            return result;
        }
    }
}