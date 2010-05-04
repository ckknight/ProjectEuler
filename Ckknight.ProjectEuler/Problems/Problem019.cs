using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(19,
        @"You are given the following information, but you may prefer to do some research for yourself.

        * 1 Jan 1900 was a Monday.
        * Thirty days has September,
          April, June and November.
          All the rest have thirty-one,
          Saving February alone,
          Which has twenty-eight, rain or shine.
          And on leap years, twenty-nine.
        * A leap year occurs on any year evenly divisible by 4, but not on a century unless it is divisible by 400.

        How many Sundays fell on the first of the month during the twentieth century (1 Jan 1901 to 31 Dec 2000)?")]
    class Problem019 : BaseProblem
    {
        public override object CalculateResult()
        {
            int startDayOfWeek = 1; // 1 being Monday, 0 being Sunday

            return new Range(1900, 2000, true)
                .SelectMany(y => new Range(1, 12, true)
                    .SelectMany(m => new Range(1, GetNumDaysInMonth(m, y), true)
                        .Select(d => new { y, m, d })))
                .Select((x, i) => new { x.y, x.m, x.d, dayOfWeek = (startDayOfWeek + i) % 7 })
                .Where(x => x.y >= 1901)
                .Where(x => x.d == 1)
                .Where(x => x.dayOfWeek == 0)
                .Count();
        }

        private static readonly Dictionary<int, int> NumDaysPerMonth = new Dictionary<int, int>
        {
            { 1, 31 },
            { 2, 28 }, // note: will be a leap year on certain years
            { 3, 31 },
            { 4, 30 },
            { 5, 31 },
            { 6, 30 },
            { 7, 31 },
            { 8, 31 },
            { 9, 30 },
            { 10, 31 },
            { 11, 30 },
            { 12, 31 }
        };
        public int GetNumDaysInMonth(int month, int year)
        {
            int numDays = NumDaysPerMonth[month];
            if (month == 2)
            {
                if (IsLeapYear(year))
                {
                    numDays++;
                }
            }
            return numDays;
        }

        public bool IsLeapYear(int year)
        {
            if ((year % 4) != 0)
            {
                return false;
            }

            if ((year % 400) == 0)
            {
                return true;
            }

            if ((year % 100) == 0)
            {
                return false;
            }

            return true;
        }
    }
}
