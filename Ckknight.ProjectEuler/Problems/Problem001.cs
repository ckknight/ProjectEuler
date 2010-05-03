using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(1,
        @"If we list all the natural numbers below 10 that are multiples of 3
        or 5, we get 3, 5, 6 and 9. The sum of these multiples is 23.
        
        Find the sum of all the multiples of 3 or 5 below 1000.")]
    public class Problem001 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, 1000)
                .Where(x => (x % 3) == 0 || (x % 5) == 0)
                .Sum();
        }
    }
}
