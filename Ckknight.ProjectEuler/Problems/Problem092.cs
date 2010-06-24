using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(92,
        @"A number chain is created by continuously adding the square of the
        digits in a number to form a new number until it has been seen before.

        For example,

        44 -> 32 -> 13 -> 10 -> 1 -> 1
        85 -> 89 -> 145 -> 42 -> 20 -> 4 -> 16 -> 37 -> 58 -> 89

        Therefore any chain that arrives at 1 or 89 will become stuck in an
        endless loop. What is most amazing is that EVERY starting number will
        eventually arrive at 1 or 89.

        How many starting numbers below ten million will arrive at 89?")]
    public class Problem092 : BaseProblem
    {
        public Problem092()
        {
            _cache = new int[maximum];
            _cache[1] = 1;
            _cache[89] = 89;
        }

        private const int maximum = 10000000;
        public override object CalculateResult()
        {
            return new Range(1, maximum)
                .AsParallel()
                .Select(n => GetChainRepeater(n))
                .Count(v => v == 89);
        }

        private readonly int[] _cache;
        public int GetChainRepeater(int value)
        {
            int result = _cache[value];
            if (result == 0)
            {
                _cache[value] = result = GetChainRepeater(NextLink(value));
            }
            return result;
        }

        private readonly int[] _links = new int[maximum];
        public int NextLink(int value)
        {
            int result = _links[value];
            if (result == 0)
            {
                _links[value] = result = MathUtilities.ToDigits(value)
                    .Select(n => n * n)
                    .Sum();
            }
            return result;
        }
    }
}
