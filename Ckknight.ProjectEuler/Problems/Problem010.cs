using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(10,
        @"The sum of the primes below 10 is 2 + 3 + 5 + 7 = 17.

        Find the sum of all the primes below two million.")]
    public class Problem010 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new PrimeGenerator()
                .TakeWhile(i => i < 2000000)
                .Sum();
        }
    }
}
