using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(70,
        @"Euler's Totient function, φ(n) [sometimes called the phi function],
        is used to determine the number of positive numbers less than or equal
        to n which are relatively prime to n. For example, as 1, 2, 4, 5, 7,
        and 8, are all less than nine and relatively prime to nine, φ(9)=6.

        The number 1 is considered to be relatively prime to every positive
        number, so φ(1)=1.

        Interestingly, φ(87109)=79180, and it can be seen that 87109 is a
        permutation of 79180.

        Find the value of n, 1 < n < 10^7, for which φ(n) is a permutation of n
        and the ratio n/φ(n) produces a minimum.")]
    public class Problem070 : BaseProblem
    {
        public override object CalculateResult()
        {
            return PrimeGenerator.Instance.GetParallelSemiprimesUpTo(10000000, false)
                .Select(x => new
                {
                    n = x.Item1 * x.Item2,
                    t = (x.Item1 - 1) * (x.Item2 - 1)
                })
                .Concat(PrimeGenerator.Instance.AsParallel((long)Math.Sqrt(10000000))
                    .Select(p => new
                    {
                        n = p*p,
                        t = p*p - p,
                    }))
                .Where(x => MathUtilities.ToDigits(x.t).IsPermutation(MathUtilities.ToDigits(x.n)))
                .WithMin(x => (double)x.n / (double)x.t)
                .n;
        }
    }
}
