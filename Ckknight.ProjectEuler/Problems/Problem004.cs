using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(4,
        @"A palindromic number reads the same both ways. The largest palindrome made from the product of two 2-digit numbers is 9009 = 91  99.

        Find the largest palindrome made from the product of two 3-digit numbers.")]
    public class Problem004 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(100, 1000)
                .SelectMany(a => new Range(a, 1000)
                    .Select(b => a * b))
                .Where(x => ConvertToDigits(x).IsPalindrome())
                .Max();
        }

        public static IEnumerable<int> ConvertToDigits(int value)
        {
            while (value > 0)
            {
                yield return value % 10;
                value /= 10;
            }
        }
    }
}
