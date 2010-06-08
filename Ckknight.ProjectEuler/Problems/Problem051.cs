using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(51,
        @"By replacing the 1st digit of *3, it turns out that six of the nine
        possible values: 13, 23, 43, 53, 73, and 83, are all prime.

        By replacing the 3rd and 4th digits of 56**3 with the same digit, this
        5-digit number is the first example having seven primes among the ten
        generated numbers, yielding the family: 56003, 56113, 56333, 56443,
        56663, 56773, and 56993. Consequently 56003, being the first member of
        this family, is the smallest prime with this property.

        Find the smallest prime which, by replacing part of the number (not
        necessarily adjacent digits) with the same digit, is part of an eight
        prime value family.")]
    public class Problem051 : BaseProblem
    {
        public override object CalculateResult()
        {
            return PrimeGenerator.Instance
                .OrderedGroupBy(p => (int)Math.Log10(p))
                .Select(group => {
                    var primes = group.ToHashSet();
                    return primes
                        .SelectMany(p =>
                        {
                            var digits = MathUtilities.ToDigits(p);
                            return digits
                                .Distinct()
                                .Where(m => m <= 2)
                                .Select(m => new Range(0, 9, true)
                                    .Select(d => digits.Replace(m, d))
                                    .Select(q => MathUtilities.FromDigits(q))
                                    .Where(q => primes.Contains(q))
                                    .Distinct()
                                    .ToArray())
                                .Where(q => q.Length == 8)
                                .SelectMany(q => q);
                        })
                        .DefaultIfEmpty(int.MaxValue)
                        .Min();
                })
                .First(x => x < int.MaxValue);
        }
    }
}
