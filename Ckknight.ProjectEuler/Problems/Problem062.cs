using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(62,
        @"The cube, 41063625 (345^3), can be permuted to produce two other
        cubes: 56623104 (384^3) and 66430125 (405^3). In fact, 41063625 is the
        smallest cube which has exactly three permutations of its digits which
        are also cube.

        Find the smallest cube for which exactly five permutations of its
        digits are cube.")]
    public class Problem062 : BaseProblem
    {
        public override object CalculateResult()
        {
            var digitRepresentations = new Range(0, 9, true)
                .Select(n => PrimeGenerator.Instance.GetPrimeAtIndex(n))
                .ToArray();

            return new Range(1, int.MaxValue)
                .Select(numDigits =>
                {
                    long num = MathUtilities.Pow(10L, numDigits - 1);
                    long maximum = num * 10;

                    return new Range((int)Math.Pow(num, 1.0 / 3.0), int.MaxValue)
                        .Select(n => (long)n)
                        .Select(n => n * n * n)
                        .SkipWhile(n => n < num)
                        .TakeWhile(n => n < maximum)
                        .GroupBy(n => MathUtilities.ToDigits(n)
                            .Select(d => digitRepresentations[d])
                            .Product())
                        .Where(g => g.Count() == 5)
                        .SelectMany(x => x)
                        .Select(x => (long?)x)
                        .DefaultIfEmpty()
                        .Min();
                })
                .WhereNotNull()
                .First();
        }
    }
}
