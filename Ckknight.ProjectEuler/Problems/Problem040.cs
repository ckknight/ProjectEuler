using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(40,
        @"An irrational decimal fraction is created by concatenating the
        positive integers:

                0.12345678910[1]112131415161718192021...

        It can be seen that the 12th digit of the fractional part is 1.

        If d_n represents the nth digit of the fractional part, find the value
        of the following expression.

        d_1 * d_10 * d_100 * d_1000 * d_10000 * d_100000 * d_1000000")]
    public class Problem040 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, int.MaxValue)
                .SelectMany(n => MathUtilities.ToDigits(n).Reverse())
                .ElementsAt(0, 9, 99, 999, 9999, 99999, 999999)
                .Product();
        }
    }
}
