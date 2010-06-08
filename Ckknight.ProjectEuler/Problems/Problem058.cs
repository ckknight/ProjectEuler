using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(58,
        @"Starting with 1 and spiralling anticlockwise in the following way, a
        square spiral with side length 7 is formed.

               [37]36 35 34 33 32[31]
                38[17]16 15 14[13]30
                39 18 [5] 4 [3]12 29
                40 19  6  1  2 11 28
                41 20 [7] 8  9 10 27
                42 21 22 23 24 25 26
               [43]44 45 46 47 48 49

        It is interesting to note that the odd squares lie along the bottom
        right diagonal, but what is more interesting is that 8 out of the 13
        numbers lying along both diagonals are prime; that is, a ratio of
        8/13 ≈ 62%.

        If one complete new layer is wrapped around the spiral above, a square
        spiral with side length 9 will be formed. If this process is continued,
        what is the side length of the square spiral for which the ratio of
        primes along both diagonals first falls below 10%?")]
    public class Problem058 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(int.MaxValue)
                .SelectWithAggregate(new
                {
                    SideLength = 1,
                    Count = 1,
                    NumPrimes = 0,
                }, (x, i) =>
                {
                    int n = x.SideLength + 2;
                    int[] values = new[]
                    {
                        n * n - 3*n + 3,
                        n * n - 2*n + 2,
                        n * n - n + 1,
                        n * n,
                    };
                    return new
                    {
                        SideLength = n,
                        Count = x.Count + values.Length,
                        NumPrimes = x.NumPrimes + values.Where(p => PrimeGenerator.Instance.IsPrime(p)).Count()
                    };
                })
                .First(x => x.NumPrimes * 10 <= x.Count)
                .SideLength;
        }
    }
}
