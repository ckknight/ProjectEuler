using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(37,
        @"The number 3797 has an interesting property. Being prime itself, it
        is possible to continuously remove digits from left to right, and
        remain prime at each stage: 3797, 797, 97, and 7. Similarly we can work
        from right to left: 3797, 379, 37, and 3.

        Find the sum of the only eleven primes that are both truncatable from
        left to right and right to left.

        NOTE: 2, 3, 5, and 7 are not considered to be truncatable primes.")]
    public class Problem037 : BaseProblem
    {
        public override object CalculateResult()
        {
            return PrimeGenerator.Instance
                .SkipWhile(x => x < 10)
                .Where(x => GetTruncations(x)
                    .All(p => PrimeGenerator.Instance.IsPrime(p)))
                .Take(11)
                .Sum();
        }

        public static IEnumerable<long> GetTruncations(long value)
        {
            return GetLeftTruncations(value)
                .Concat(GetRightTruncations(value));
        }

        public static IEnumerable<long> GetLeftTruncations(long value)
        {
            int digits = (int)Math.Ceiling(Math.Log10(value));

            for (int i = digits - 1; i > 0; i--)
            {
                yield return value % MathUtilities.Pow(10L, i);
            }
        }

        public static IEnumerable<long> GetRightTruncations(long value)
        {
            value /= 10;
            while (value > 0)
            {
                yield return value;
                value /= 10;
            }
        }
    }
}
