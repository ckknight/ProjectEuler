using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(33,
        @"The fraction 49/98 is a curious fraction, as an inexperienced
        mathematician in attempting to simplify it may incorrectly believe that
        49/98 = 4/8, which is correct, is obtained by cancelling the 9s.

        We shall consider fractions like, 30/50 = 3/5, to be trivial examples.

        There are exactly four non-trivial examples of this type of fraction,
        less than one in value, and containing two digits in the numerator and
        denominator.

        If the product of these four fractions is given in its lowest common
        terms, find the value of the denominator.")]
    public class Problem033 : BaseProblem
    {
        public override object CalculateResult()
        {
            int minimum = 10;
            int maximum = 99;
            return new Range(minimum, maximum - 1, true)
                .SelectMany(n => new Range(n + 1, maximum, true)
                    .Select(d => new
                    {
                        Numerator = MathUtilities.ToDigitList(n).ToArray(),
                        Denominator = MathUtilities.ToDigitList(d).ToArray()
                    }))
                .Where(f => f.Numerator[0] != 0 || f.Denominator[0] != 0)
                .Where(f => f.Numerator.Any(d => f.Denominator.Contains(d)))
                .SelectMany(f => f.Numerator
                    .SelectMany((n, i) => f.Denominator
                        .Select((d, j) => n == d ? new
                        {
                            OldNumerator = (int)MathUtilities.FromDigitList(f.Numerator),
                            OldDenominator = (int)MathUtilities.FromDigitList(f.Denominator),
                            NewNumerator = (int)MathUtilities.FromDigitList(f.Numerator.Take(i).Concat(f.Numerator.Skip(i + 1))),
                            NewDenominator = (int)MathUtilities.FromDigitList(f.Denominator.Take(j).Concat(f.Denominator.Skip(j + 1))),
                        } : null)
                        .Where(x => x != null)))
                .Where(x => x.NewDenominator != 0)
                .Where(x => x.NewDenominator * x.OldNumerator == x.NewNumerator * x.OldDenominator)
                .Select(x => new
                {
                    Numerator = x.NewNumerator,
                    Denominator = x.NewDenominator,
                })
                .Aggregate(new { Numerator = 1, Denominator = 1 }, (a, b) => new
                {
                    Numerator = a.Numerator * b.Numerator,
                    Denominator = a.Denominator * b.Denominator
                }, f =>
                {
                    int gcd = MathUtilities.GreatestCommonDivisor(f.Numerator, f.Denominator);
                    return new
                    {
                        Numerator = f.Numerator / gcd,
                        Denominator = f.Denominator / gcd,
                    };
                })
                .Denominator;
        }
    }
}
