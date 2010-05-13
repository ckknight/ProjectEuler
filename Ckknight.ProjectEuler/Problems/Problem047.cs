using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(47,
        @"The first two consecutive numbers to have two distinct prime factors are:

        14 = 2 * 7
        15 = 3 * 5

        The first three consecutive numbers to have three distinct prime factors are:

        644 = 2^2 * 7 * 23
        645 = 3 * 5 * 43
        646 = 2 * 17 * 19.

        Find the first four consecutive integers to have four distinct primes factors. What is the first of these numbers?")]
    public class Problem047 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(2, int.MaxValue)
                .Select(n => new
                {
                    Value = n,
                    NumFactors = new PrimeFactorGenerator(n).Distinct().Count()
                })
                .ToMemorableEnumerable(3, e => e)
                .Where(e => e.Length == 4)
                .Where(e => e.All(x => x.NumFactors == 4))
                .First()
                .Min(e => e.Value);
        }
    }
}
