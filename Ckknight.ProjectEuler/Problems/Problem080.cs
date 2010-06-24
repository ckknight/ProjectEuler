using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;
using System.Numerics;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(80,
        @"It is well known that if the square root of a natural number is not
        an integer, then it is irrational. The decimal expansion of such square
        roots is infinite without any repeating pattern at all.

        The square root of two is 1.41421356237309504880..., and the digital
        sum of the first one hundred decimal digits is 475.

        For the first one hundred natural numbers, find the total of the
        digital sums of the first one hundred decimal digits for all the
        irrational square roots.")]
    public class Problem080 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, 100, true)
                .Where(n => !MathUtilities.IsPerfectSquare(n))
                .Select(n => Sqrt(n)
                    .Take(100)
                    .Sum())
                .Sum();
        }

        public static IEnumerable<int> Sqrt(BigDecimal value)
        {
            BigInteger log10 = BigDecimal.Log10Ceiling(value);

            BigDecimal v = BigDecimal.Pow(100, (log10 - 1) / 2);
            value /= v;

            BigInteger p = 0;
            BigInteger c = 0;
            while (true)
            {
                c += (BigInteger)value;
                int X = new Range(9, 0, -1, true)
                    .First(x => (20 * p + x) * x <= c);
                c -= (20 * p + X) * X;
                p *= 10;
                p += X;
                yield return X;
                c *= 100;
                value *= 100;
                value %= 100;
            }
        }
    }
}
