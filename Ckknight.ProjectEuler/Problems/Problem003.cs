using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(3,
        @"The prime factors of 13195 are 5, 7, 13 and 29.

        What is the largest prime factor of the number 600851475143 ?")]
    public class Problem003 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new PrimeFactorGenerator(600851475143)
                .Max();
        }
    }
}
