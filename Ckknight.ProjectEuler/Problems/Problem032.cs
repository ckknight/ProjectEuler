using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(32,
        @"We shall say that an n-digit number is pandigital if it makes use of
        all the digits 1 to n exactly once; for example, the 5-digit number,
        15234, is 1 through 5 pandigital.

        The product 7254 is unusual, as the identity, 39 * 186 = 7254,
        containing multiplicand, multiplier, and product is 1 through 9
        pandigital.

        Find the sum of all products whose multiplicand/multiplier/product
        identity can be written as a 1 through 9 pandigital.

        HINT: Some products can be obtained in more than one way so be sure to
        only include it once in your sum.")]
    public class Problem032 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, 98, true)
                .Select(a => new
                {
                    Value = a,
                    Digits = MathUtilities.ToDigits(a),
                })
                .Where(a => !a.Digits.Contains(0))
                .Where(a => a.Digits.Distinct().Count() == a.Digits.Length)
                .SelectMany(a => new Range(123, 9876, true)
                    .Select(b => new
                    {
                        Value = b,
                        Digits = MathUtilities.ToDigits(b),
                    })
                    .Where(b => !b.Digits.Contains(0))
                    .Where(b => b.Digits.Distinct().Count() == b.Digits.Length)
                    .Where(b => !a.Digits.Any(d => b.Digits.Contains(d)))
                    .Select(b => new
                    {
                        a = a,
                        b = b,
                        c = new
                        {
                            Value = a.Value * b.Value,
                            Digits = MathUtilities.ToDigits(a.Value * b.Value)
                        }
                    }))
                .Where(x => x.a.Digits.Length + x.b.Digits.Length + x.c.Digits.Length == 9)
                .Where(x => !x.c.Digits.Contains(0))
                .Where(x => x.c.Digits.Distinct().Count() == x.c.Digits.Length)
                .Where(x => !x.a.Digits.Any(d => x.c.Digits.Contains(d)))
                .Where(x => !x.b.Digits.Any(d => x.c.Digits.Contains(d)))
                .Select(x => x.c.Value)
                .Distinct()
                .Sum();
        }
    }
}
