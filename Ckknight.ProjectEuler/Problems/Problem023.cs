using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(23,
        @"A perfect number is a number for which the sum of its proper divisors
        is exactly equal to the number. For example, the sum of the proper
        divisors of 28 would be 1 + 2 + 4 + 7 + 14 = 28, which means that 28 is
        a perfect number.

        A number n is called deficient if the sum of its proper divisors is less
        than n and it is called abundant if this sum exceeds n.

        As 12 is the smallest abundant number, 1 + 2 + 3 + 4 + 6 = 16, the
        smallest number that can be written as the sum of two abundant numbers
        is 24. By mathematical analysis, it can be shown that all integers
        greater than 28123 can be written as the sum of two abundant numbers.
        However, this upper limit cannot be reduced any further by analysis even
        though it is known that the greatest number that cannot be expressed as
        the sum of two abundant numbers is less than this limit.

        Find the sum of all the positive integers which cannot be written as the
        sum of two abundant numbers.")]
    public class Problem023 : BaseProblem
    {
        private readonly int UpperLimit = 28123;
        public override object CalculateResult()
        {
            var abundantNumbers = new Range(1, UpperLimit)
                .Where(n => new FactorGenerator(n, false)
                     .Sum() > n)
                .ToArray();

            return abundantNumbers
                .SelectMany((a, i) => abundantNumbers
                    .Take(i + 1)
                    .Select(b => a + b)
                    .TakeWhile(n => n < UpperLimit))
                .ToInt32Set(UpperLimit)
                .GetInverse()
                .Sum();
        }
    }
}
