using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(41,
        @"We shall say that an n-digit number is pandigital if it makes use of
        all the digits 1 to n exactly once. For example, 2143 is a 4-digit
        pandigital and is also prime.

        What is the largest n-digit pandigital prime that exists?")]
    public class Problem041 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new[] { 1, 2, 3, 4, 5, 6, 7 }
                .GetPermutations()
                .Select(n => MathUtilities.FromDigits(n))
                .OrderByDescending(n => n)
                .First(n => PrimeGenerator.IsPrime(n));
        }
    }
}
