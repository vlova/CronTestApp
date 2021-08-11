using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TestApp.ByPzixelLova
{
    public readonly struct ScheduleInterval : IEnumerable<(int Point, bool IsAllowed)>
    {
        public int Begin { get; }
        public int End { get; }

        private readonly SortedSet<int> _allowedPoints;
        private readonly Dictionary<int, int> _nextPointCache;

        public ScheduleInterval(int begin, int end)
        {
            Begin = begin;
            End = end;
            _allowedPoints = new SortedSet<int>();
            _nextPointCache = new Dictionary<int, int>(capacity: end - begin + 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool IsPointAllowed(int point) => _allowedPoints.Contains(point);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public int NextPoint(int point)
        {
            if (_nextPointCache.TryGetValue(point, out var nextValue))
            {
                return nextValue;
            }
            else
            {
                var newNextValue = NextPointUncached(point);
                _nextPointCache.Add(point, newNextValue);
                return newNextValue;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private int NextPointUncached(int point)
        {
            if (point >= End)
            {
                return point + 1;
            }

            return _allowedPoints.GetViewBetween(point + 1, End).Cast<int?>().FirstOrDefault()
                ?? Math.Max(point + 1, End);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool ChangePointAllowance(int point, bool value)
            => value
            ? _allowedPoints.Add(point)
            : _allowedPoints.Remove(point);

        public static ScheduleInterval CreateAllowedInterval(int begin, int end)
        {
            var result = new ScheduleInterval(begin, end);
            for (int i = begin; i <= end; i++)
            {
                result._allowedPoints.Add(i);
            }

            return result;
        }

        public IEnumerator<(int Point, bool IsAllowed)> GetEnumerator()
        {
            for (int i = Begin; i <= End; i++)
            {
                yield return (i, _allowedPoints.Contains(i));
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}