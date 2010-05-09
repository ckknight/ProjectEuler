using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(35,
        @"The number, 197, is called a circular prime because all rotations of
        the digits: 197, 971, and 719, are themselves prime.

        There are thirteen such primes below 100: 2, 3, 5, 7, 11, 13, 17, 31,
        37, 71, 73, 79, and 97.

        How many circular primes are there below one million?")]
    public class Problem035 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new PrimeGenerator()
                .TakeWhile(n => n < 1000000)
                .Select(n => MathUtilities.ToDigits(n))
                .Where(n => new Range(1, n.Length)
                    .All(i => PrimeGenerator.IsPrime(
                        MathUtilities.FromDigits(
                            n.Skip(i).Concat(n.Take(i))))))
                .Count();
        }
    }
}
