using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;
using System.Collections.Concurrent;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(95,
        @"The proper divisors of a number are all the divisors excluding the
        number itself. For example, the proper divisors of 28 are 1, 2, 4, 7,
        and 14. As the sum of these divisors is equal to 28, we call it a
        perfect number.

        Interestingly the sum of the proper divisors of 220 is 284 and the sum
        of the proper divisors of 284 is 220, forming a chain of two numbers.
        For this reason, 220 and 284 are called an amicable pair.

        Perhaps less well known are longer chains. For example, starting with
        12496, we form a chain of five numbers:

        12496 -> 14288 -> 15472 -> 14536 -> 14264 (-> 12496 -> ...)

        Since this chain returns to its starting point, it is called an
        amicable chain.

        Find the smallest member of the longest amicable chain with no element
        exceeding one million.")]
    public class Problem095 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, 1000000, true)
                .AsParallel()
                .Select(n => GetChain(n)
                    .TakeWhile(v => v <= 1000000)
                    .TakeWhileDistinct(true)
                    .ToArray())
                .Where(c => c[0] == c[c.Length - 1])
                .WithMax(c => c.Length)
                .Min();
        }

        public IEnumerable<long> GetChain(long value)
        {
            do
            {
                yield return value;
                value = GetLink(value);
            } while (true);
        }

        private readonly ConcurrentDictionary<long, long> _links = new ConcurrentDictionary<long, long>(new Dictionary<long, long>
            {
                { 0, 0 }
            });
        public long GetLink(long value)
        {
            return _links.GetOrAdd(value, v => FactorGenerator.SumOfFactors(v));
        }
    }
}
