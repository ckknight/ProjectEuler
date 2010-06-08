using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(69,
        @"Euler's Totient function, φ(n) [sometimes called the phi function],
        is used to determine the number of numbers less than n which are
        relatively prime to n. For example, as 1, 2, 4, 5, 7, and 8, are all
        less than nine and relatively prime to nine, φ(9)=6.

        n    Relatively Prime    φ(n)    n/φ(n)
        2    1                   1       2
        3    1,2                 2       1.5
        4    1,3                 2       2
        5    1,2,3,4             4       1.25
        6    1,5                 2       3
        7    1,2,3,4,5,6         6       1.1666...
        8    1,3,5,7             4       2
        9    1,2,4,5,7,8         6       1.5
        10   1,3,7,9             4       2.5

        It can be seen that n=6 produces a maximum n/φ(n) for n <= 10.

        Find the value of n <= 1,000,000 for which n/φ(n) is a maximum.")]
    public class Problem069 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, 1000000, true)
                .AsParallel()
                .WithMax(n => (double)n / Phi(n));
        }

        public long Phi(long n)
        {
            return new PrimeFactorGenerator(n)
                .Distinct()
                .Aggregate(n, (x, p) => x * (p - 1) / p);
        }
    }
}
