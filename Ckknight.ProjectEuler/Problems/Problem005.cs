using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(5,
        @"2520 is the smallest number that can be divided by each of the numbers from 1 to 10 without any remainder.

        What is the smallest number that is evenly divisible by all of the numbers from 1 to 20?")]
    public class Problem005 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new PrimeGenerator()
                .TakeWhile(p => p <= 20)
                .Select(p =>
                {
                    // We want to get the largest value that is a power of `p` but <= 20.
                    // e.g. 2 => 16, 3 => 9
                    long exponent = (long)Math.Floor(Math.Log(20, p));

                    return (long)Math.Pow(p, exponent);
                })
                .Aggregate((a, b) => a * b);
        }
    }
}
