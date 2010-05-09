using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(34,
        @"145 is a curious number, as 1! + 4! + 5! = 1 + 24 + 120 = 145.

        Find the sum of all numbers which are equal to the sum of the factorial
        of their digits.

        Note: as 1! = 1 and 2! = 2 are not sums they are not included.")]
    public class Problem034 : BaseProblem
    {
        public override object CalculateResult()
        {
            int maxDigits = new Range(1, int.MaxValue)
                .First(i => MathUtilities.Pow(10, i) >= i * Factorial(9));

            return new Range(10, maxDigits * Factorial(9))
                .Where(n => MathUtilities.ToDigitList(n).Sum(d => Factorial(d)) == n)
                .Sum();
        }

        private readonly Dictionary<int, int> _cache = new Dictionary<int, int>
        {
            { 0, 1 },
        };
        public int Factorial(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value", value, "Must be at least 0");
            }

            int result;
            if (!_cache.TryGetValue(value, out result))
            {
                _cache[value] = result = value * Factorial(value - 1);
            }
            return result;
        }
    }
}
