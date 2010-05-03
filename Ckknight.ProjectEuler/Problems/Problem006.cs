using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(6,
        @"The sum of the squares of the first ten natural numbers is,

        1^2 + 2^2 + ... + 10^2 = 385
        The square of the sum of the first ten natural numbers is,

        (1 + 2 + ... + 10)^2 = 55^2 = 3025
        Hence the difference between the sum of the squares of the first ten natural numbers and the square of the sum is 3025 - 385 = 2640.

        Find the difference between the sum of the squares of the first one hundred natural numbers and the square of the sum.")]
    public class Problem006 : BaseProblem
    {
        public override object CalculateResult()
        {
            var range = new Range(1, 100, true);

            int sumOfSquares = range.Select(x => x * x).Sum();
            int sum = range.Sum();
            int squareOfSums = sum * sum;

            return squareOfSums - sumOfSquares;
        }
    }
}
