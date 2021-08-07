using System;
using System.Collections;
using System.Collections.Generic;

namespace TestApp
{
    public readonly struct ScheduleInterval : IEnumerable<(int Point, bool IsAllowed)>
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

        public bool IsPointAllowed(int point) => _allowedPoints[point - Begin];
        public bool ChangePointAllowance(int point, bool value) => _allowedPoints[point - Begin] = value;

        public static ScheduleInterval CreateAllowedInterval(int begin, int end)
        {
            var result = new ScheduleInterval(begin, end);
            for (int i = 0; i < result._allowedPoints.Length; i++)
            {
                result._allowedPoints[i] = true;
            }

            return result;
        }

        public IEnumerator<(int Point, bool IsAllowed)> GetEnumerator()
        {
            for (int i = Begin; i <= End; i++)
            {
                yield return (i, _allowedPoints[i - Begin]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}