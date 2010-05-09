using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(30,
        @"Surprisingly there are only three numbers that can be written as the sum of fourth powers of their digits:

            1634 = 1^4 + 6^4 + 3^4 + 4^4
            8208 = 8^4 + 2^4 + 0^4 + 8^4
            9474 = 9^4 + 4^4 + 7^4 + 4^4
        
        As 1 = 1^4 is not a sum it is not included.

        The sum of these numbers is 1634 + 8208 + 9474 = 19316.

        Find the sum of all the numbers that can be written as the sum of fifth powers of their digits.")]
    public class Problem030 : BaseProblem
    {
        private readonly int Exponent = 5;

        public override object CalculateResult()
        {
            int maxDigits = new Range(1, int.MaxValue)
                .First(n => n * MathUtilities.Pow(9, Exponent) < MathUtilities.Pow(10, n) - 1);

            return new Range(2, maxDigits * MathUtilities.Pow(9, Exponent))
                .Where(n => MathUtilities.ToDigits(n)
                    .Select(d => MathUtilities.Pow(d, Exponent))
                    .Sum() == n)
                .Sum();
        }
    }
}
