using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(36,
        @"The decimal number, 585 = 10010010012 (binary), is palindromic in
        both bases.

        Find the sum of all numbers, less than one million, which are
        palindromic in base 10 and base 2.

        (Please note that the palindromic number, in either base, may not
        include leading zeros.)")]
    public class Problem036 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, 1000000)
                .Where(n => MathUtilities.ToDigits(n)
                    .IsPalindrome())
                .Where(n => MathUtilities.ToBits(n)
                    .Cast<bool>()
                    .IsPalindrome())
                .Sum();
        }
    }
}
