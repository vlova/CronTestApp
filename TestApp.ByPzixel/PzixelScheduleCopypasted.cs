using System;
using Pidgin;

namespace TestApp.ByPzixel
{
    public class PzixelScheduleCopypasted
    {
        private readonly MergedSchedule _innerSchedule;

        /// <summary>
        /// Создает пустой экземпляр, который будет соответствовать
        /// расписанию типа "*.*.* * *:*:*.*" (раз в 1 мс).
        /// </summary>
        public PzixelScheduleCopypasted()
            : this("*.*.* * *:*:*.*")
        {
        }

        /// <summary>
        /// Создает экземпляр из строки с представлением расписания.
        /// </summary>
        /// <param name="scheduleString">Строка расписания.
        /// Формат строки:
        ///     yyyy.MM.dd w HH:mm:ss.fff
        ///     yyyy.MM.dd HH:mm:ss.fff
        ///     HH:mm:ss.fff
        ///     yyyy.MM.dd w HH:mm:ss
        ///     yyyy.MM.dd HH:mm:ss
        ///     HH:mm:ss
        /// Где yyyy - год (2000-2100)
        ///     MM - месяц (1-12)
        ///     dd - число месяца (1-31 или 32). 32 означает последнее число месяца
        ///     w - день недели (0-6). 0 - воскресенье, 6 - суббота
        ///     HH - часы (0-23)
        ///     mm - минуты (0-59)
        ///     ss - секунды (0-59)
        ///     fff - миллисекунды (0-999). Если не указаны, то 0
        /// Каждую часть даты/времени можно задавать в виде списков и диапазонов.
        /// Например:
        ///     1,2,3-5,10-20/3
        ///     означает список 1,2,3,4,5,10,13,16,19
        /// Дробью задается шаг в списке.
        /// Звездочка означает любое возможное значение.
        /// Например (для часов):
        ///     */4
        ///     означает 0,4,8,12,16,20
        /// Вместо списка чисел месяца можно указать 32. Это означает последнее
        /// число любого месяца.
        /// Пример:
        ///     *.9.*/2 1-5 10:00:00.000
        ///     означает 10:00 во все дни с пн. по пт. по нечетным числам в сентябре
        ///     *:00:00
        ///     означает начало любого часа
        ///     *.*.01 01:30:00
        ///     означает 01:30 по первым числам каждого месяца
        /// </param>
        public PzixelScheduleCopypasted(string scheduleString)
        {
            var format = PzixelParserHelper.FullFormatParser.ParseOrThrow(scheduleString);
            _innerSchedule = MergedSchedule.FromFormat(format);
        }

        /// <summary>
        /// Возвращает следующий ближайший к заданному времени момент в расписании или
        /// само заданное время, если оно есть в расписании.
        /// </summary>
        /// <param name="t1">Заданное время</param>
        /// <returns>Ближайший момент времени в расписании</returns>
        public DateTime NearestEvent(DateTime t1)
        {
            while (true)
            {
                LoopStart: ;
                // search for year
                while (!_innerSchedule.Years.IsPointAllowed(t1.Year))
                {
                    t1 = new DateTime(t1.Year, 1, 1).AddYears(1);
                }

                // search for month
                var year = t1.Year;
                while (!_innerSchedule.Months.IsPointAllowed(t1.Month))
                {
                    t1 = new DateTime(t1.Year, t1.Month, 1).AddMonths(1);
                    if (t1.Year != year)
                    {
                        goto LoopStart;
                    }
                }

                // search for day
                var month = t1.Month;
                while (!(_innerSchedule.DayOfWeek.IsPointAllowed((int) t1.DayOfWeek)
                         && (_innerSchedule.Days.IsPointAllowed(t1.Day) ||
                             _innerSchedule.Days.IsPointAllowed(32)
                             && t1.Day == DateTime.DaysInMonth(t1.Year, t1.Month))))
                {

                    t1 = new DateTime(t1.Year, t1.Month, t1.Day).AddDays(1);
                    if (t1.Month != month)
                    {
                        goto LoopStart;
                    }
                }
                
                // search for hour
                var day = t1.Day;
                while (!_innerSchedule.Hours.IsPointAllowed(t1.Hour))
                {
                    t1 = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, 0, 0).AddHours(1);
                    if (t1.Day != day)
                    {
                        goto LoopStart;
                    }
                }
                
                // search for minute
                var hour = t1.Hour;
                while (!_innerSchedule.Minutes.IsPointAllowed(t1.Minute))
                {
                    t1 = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, 0).AddMinutes(1);
                    if (t1.Hour != hour)
                    {
                        goto LoopStart;
                    }
                }
                
                // search for second
                var minute = t1.Minute;
                while (!_innerSchedule.Seconds.IsPointAllowed(t1.Second))
                {
                    t1 = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, t1.Second).AddSeconds(1);
                    if (t1.Minute != minute)
                    {
                        goto LoopStart;
                    }
                }
                
                // search for second
                var second = t1.Second;
                while (!_innerSchedule.Milliseconds.IsPointAllowed(t1.Millisecond))
                {
                    t1 = t1.AddMilliseconds(1);
                    if (t1.Second != second)
                    {
                        goto LoopStart;
                    }
                }

                return t1;
            }
        }

        /// <summary>
        /// Возвращает предыдущий ближайший к заданному времени момент в расписании или
        /// само заданное время, если оно есть в расписании.
        /// </summary>
        /// <param name="t1">Заданное время</param>
        /// <returns>Ближайший момент времени в расписании</returns>
        public DateTime NearestPrevEvent(DateTime t1)
        {
            while (true)
            {
                LoopStart: ;
                // search for year
                while (!_innerSchedule.Years.IsPointAllowed(t1.Year))
                {
                    t1 = new DateTime(t1.Year, 1, 1).AddMilliseconds(-1);
                }

                // search for month
                var year = t1.Year;
                while (!_innerSchedule.Months.IsPointAllowed(t1.Month))
                {
                    t1 = new DateTime(t1.Year, t1.Month, 1).AddMilliseconds(-1);
                    if (t1.Year != year)
                    {
                        goto LoopStart;
                    }
                }

                // search for day
                var month = t1.Month;
                while (!(_innerSchedule.DayOfWeek.IsPointAllowed((int) t1.DayOfWeek)
                         && (_innerSchedule.Days.IsPointAllowed(t1.Day) ||
                             _innerSchedule.Days.IsPointAllowed(32)
                             && t1.Day == DateTime.DaysInMonth(t1.Year, t1.Month))))
                {

                    t1 = t1.Date.AddMilliseconds(-1);
                    if (t1.Month != month)
                    {
                        goto LoopStart;
                    }
                }
                
                // search for hour
                var day = t1.Day;
                while (!_innerSchedule.Hours.IsPointAllowed(t1.Hour))
                {
                    t1 = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, 0, 0).AddMilliseconds(-1);
                    if (t1.Day != day)
                    {
                        goto LoopStart;
                    }
                }
                
                // search for minute
                var hour = t1.Hour;
                while (!_innerSchedule.Minutes.IsPointAllowed(t1.Minute))
                {
                    t1 = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, 0).AddMilliseconds(-1);
                    if (t1.Hour != hour)
                    {
                        goto LoopStart;
                    }
                }
                
                // search for second
                var minute = t1.Minute;
                while (!_innerSchedule.Seconds.IsPointAllowed(t1.Second))
                {
                    t1 = new DateTime(t1.Year, t1.Month, t1.Day, t1.Hour, t1.Minute, t1.Second).AddMilliseconds(-1);
                    if (t1.Minute != minute)
                    {
                        goto LoopStart;
                    }
                }
                
                // search for second
                var second = t1.Second;
                while (!_innerSchedule.Milliseconds.IsPointAllowed(t1.Millisecond))
                {
                    t1 = t1.AddMilliseconds(-1);
                    if (t1.Second != second)
                    {
                        goto LoopStart;
                    }
                }

                return t1;
            }
        }

        /// <summary>
        /// Возвращает следующий момент времени в расписании.
        /// </summary>
        /// <param name="t1">Время, от которого нужно отступить</param>
        /// <returns>Следующий момент времени в расписании</returns>
        public DateTime NextEvent(DateTime t1) => NearestEvent(t1.AddMilliseconds(1));

        /// <summary>
        /// Возвращает предыдущий момент времени в расписании.
        /// </summary>
        /// <param name="t1">Время, от которого нужно отступить</param>
        /// <returns>Предыдущий момент времени в расписании</returns>
        public DateTime PrevEvent(DateTime t1) => NearestPrevEvent(t1.AddMilliseconds(-1));
    }
}