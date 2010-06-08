using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(7,
        @"By listing the first six prime numbers: 2, 3, 5, 7, 11, and 13, we can see that the 6th prime is 13.

        What is the 10001st prime number?")]
    public class Problem007 : BaseProblem
    {
        public override object CalculateResult()
        {
            return PrimeGenerator.Instance.GetPrimeAtIndex(10001 - 1);
        }
    }
}
