using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(66,
        @"Consider quadratic Diophantine equations of the form:

                x^2 – Dy^2 = 1

        For example, when D=13, the minimal solution in x is 6492 – 131802 = 1.

        It can be assumed that there are no solutions in positive integers when
        D is square.

        By finding minimal solutions in x for D = {2, 3, 5, 6, 7}, we obtain
        the following:

        3^2 – 2*2^2 = 1
        2^2 – 3*1^2 = 1
        9^2 – 5*4^2 = 1
        5^2 – 6*2^2 = 1
        8^2 – 7*3^2 = 1

        Hence, by considering minimal solutions in x for D <= 7, the largest x
        is obtained when D=5.

        Find the value of D <= 1000 in minimal solutions of x for which the
        largest value of x is obtained.")]
    public class Problem066 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, 1000, true)
                .Where(n => !MathUtilities.IsPerfectSquare(n))
                .WithMax(n => ContinuedFraction.Sqrt(n)
                    .GetBigFractions()
                    .Where(f => f.Numerator * f.Numerator - n * f.Denominator * f.Denominator == 1)
                    .Select(f => f.Numerator)
                    .First());
        }
    }
}
