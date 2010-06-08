using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(71,
        @"Consider the fraction, n/d, where n and d are positive integers. If
        n<d and HCF(n,d)=1, it is called a reduced proper fraction.

        If we list the set of reduced proper fractions for d <= 8 in ascending
        order of size, we get:

        1/8, 1/7, 1/6, 1/5, 1/4, 2/7, 1/3, 3/8, 2/5, 3/7, 1/2, 4/7, 3/5, 5/8,
        2/3, 5/7, 3/4, 4/5, 5/6, 6/7, 7/8

        It can be seen that 2/5 is the fraction immediately to the left of 3/7.

        By listing the set of reduced proper fractions for d <= 1,000,000 in
        ascending order of size, find the numerator of the fraction immediately
        to the left of 3/7.")]
    public class Problem071 : BaseProblem
    {
        public override object CalculateResult()
        {
            Fraction threeSevenths = new Fraction(3, 7);

            return new Range(2, 1000000, true)
                .Select(d => {
                    long numerator = Fraction.Floor(d * threeSevenths).Numerator;
                    Fraction f;
                    do
                    {
                        f = new Fraction(numerator, d);
                        numerator--;
                    }
                    while (f >= threeSevenths);
                    return f;
                })
                .Max()
                .Numerator;
        }
    }
}
