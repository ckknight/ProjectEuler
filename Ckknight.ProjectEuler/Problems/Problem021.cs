using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(21,
        @"Let d(n) be defined as the sum of proper divisors of n (numbers
        less than n which divide evenly into n).
        If d(a) = b and d(b) = a, where a != b, then a and b are an amicable pair
        and each of a and b are called amicable numbers.

        For example, the proper divisors of 220 are 1, 2, 4, 5, 10, 11, 20, 22,
        44, 55 and 110; therefore d(220) = 284. The proper divisors of 284 are
        1, 2, 4, 71 and 142; so d(284) = 220.

        Evaluate the sum of all the amicable numbers under 10000.")]
    public class Problem021 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(2, 10000)
                .Where(n => SumOfProperDivisors(n) < 10000)
                .Where(n => n != SumOfProperDivisors(n))
                .Where(n => n == SumOfProperDivisors(SumOfProperDivisors(n)))
                .Sum();
        }

        private readonly Dictionary<long, long> _cache = new Dictionary<long, long>();
        public long SumOfProperDivisors(long n)
        {
            long result;
            if (!_cache.TryGetValue(n, out result))
            {
                _cache[n] = result = new FactorGenerator(n)
                    .SkipLast(1)
                    .Sum();
            }
            return result;
        }
    }
}
