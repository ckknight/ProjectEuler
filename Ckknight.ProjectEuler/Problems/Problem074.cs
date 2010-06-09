using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;
using System.Collections.Concurrent;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(74,
        @"The number 145 is well known for the property that the sum of the
        factorial of its digits is equal to 145:

        1! + 4! + 5! = 1 + 24 + 120 = 145

        Perhaps less well known is 169, in that it produces the longest chain
        of numbers that link back to 169; it turns out that there are only
        three such loops that exist:

        169 -> 363601 -> 1454 -> 169
        871 -> 45361 -> 871
        872 -> 45362 -> 872

        It is not difficult to prove that EVERY starting number will eventually
        get stuck in a loop. For example,

        69 -> 363600 -> 1454 -> 169 -> 363601 (-> 1454)
        78 -> 45360  871 -> 45361 (-> 871)
        540 -> 145 (-> 145)

        Starting with 69 produces a chain of five non-repeating terms, but the
        longest non-repeating chain with a starting number below one million is
        sixty terms.

        How many chains, with a starting number below one million, contain
        exactly sixty non-repeating terms?")]
    public class Problem074 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, 1000000)
                .AsParallel()
                .Count(i => GetChain(i)
                    .TakeWhileDistinct()
                    .Count() == 60);
        }

        private readonly int[] Factorials = new Range(1, 10)
            .SelectWithAggregate(1, (x, i) => x * i)
            .PrependItem(1)
            .ToArray();

        public IEnumerable<int> GetChain(int value)
        {
            return CollectionUtilities.Repeat(default(object))
                .SelectWithAggregate(value, (x, o) => GetNextLink(x))
                .PrependItem(value);
        }

        private ConcurrentDictionary<int, int> _links = new ConcurrentDictionary<int, int>();
        public int GetNextLink(int value)
        {
            return _links.GetOrAdd(value, v => MathUtilities.ToDigits(v)
                .Sum(d => Factorials[d]));
        }
    }
}
