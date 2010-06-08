using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(49,
        @"The arithmetic sequence, 1487, 4817, 8147, in which each of the terms
        increases by 3330, is unusual in two ways: (i) each of the three terms
        are prime, and, (ii) each of the 4-digit numbers are permutations of
        one another.

        There are no arithmetic sequences made up of three 1-, 2-, or 3-digit
        primes, exhibiting this property, but there is one other 4-digit
        increasing sequence.

        What 12-digit number do you form by concatenating the three terms in
        this sequence?")]
    public class Problem049 : BaseProblem
    {
        public override object CalculateResult()
        {
            return PrimeGenerator.Instance
                .SkipWhile(n => n < 1000)
                .TakeWhile(n => n < 10000)
                .Where(n => n != 1487)
                .Select(n => new
                {
                    A = n,
                    B = n + 3330,
                    C = n + 3330*2,
                })
                .Where(x => PrimeGenerator.Instance.IsPrime(x.B))
                .Where(x => PrimeGenerator.Instance.IsPrime(x.C))
                .Where(x =>
                {
                    var set = MathUtilities.ToDigits(x.A)
                        .GetPermutations()
                        .Select(p => MathUtilities.FromDigits(p))
                        .ToHashSet();

                    return set.Contains(x.B) && set.Contains(x.C);
                })
                .Select(x => string.Concat(x.A, x.B, x.C))
                .Single();
        }
    }
}
