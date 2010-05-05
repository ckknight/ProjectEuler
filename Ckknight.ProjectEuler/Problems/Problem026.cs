using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(26,
        @"A unit fraction contains 1 in the numerator. The decimal representation of the unit fractions with denominators 2 to 10 are given:

        1/2  = 0.5
        1/3  = 0.(3)
        1/4  = 0.25
        1/5  = 0.2
        1/6  = 0.1(6)
        1/7  = 0.(142857)
        1/8  = 0.125
        1/9  = 0.(1)
        1/10 = 0.1
        
        Where 0.1(6) means 0.166666..., and has a 1-digit recurring cycle. It can be seen that 1/7 has a 6-digit recurring cycle.
        
        Find the value of d < 1000 for which 1/d contains the longest recurring cycle in its decimal fraction part.")]
    public class Problem026 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, 1000)
                .Select(i =>
                {
                    List<int> foundRemainders = new List<int>();

                    int value = 1;
                    int position = 0;
                    while (!foundRemainders.Contains(value) && value != 0)
                    {
                        foundRemainders.Add(value);

                        value *= 10;
                        value %= i;

                        position++;
                    }

                    return new { Value = i, PeriodLength = value == 0 ? 0 : position - foundRemainders.IndexOf(value) };
                })
                .Aggregate((a, b) => a.PeriodLength > b.PeriodLength ? a : b)
                .Value;
        }
    }
}
