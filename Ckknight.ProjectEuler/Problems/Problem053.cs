using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(53,
        @"There are exactly ten ways of selecting three from five, 12345:

        123, 124, 125, 134, 135, 145, 234, 235, 245, and 345

        In combinatorics, we use the notation, C(5, 3) = 10.

        In general,

        C(n, r) = n!/(r!(n-r)!), where r <= n, n! = n*(n-1)*...*3*2*1,
        and 0! = 1.
        
        It is not until n = 23, that a value exceeds one-million:
        C(23, 10) = 1144066.

        How many, not necessarily distinct, values of C(n, r), for
        1 <= n <= 100, are greater than one-million?")]
    public class Problem053 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, 100, true)
                .SelectMany(n => new Range(1, n, true)
                    .Where(r =>
                    {
                        var numerators = new Range(n - r + 1, n, true).Select(x => (long)x).ToArray();
                        var denominators = new Range(2, r, true).Select(x => (long)x).ToArray();

                        for (int i = 0; i < numerators.Length; i++)
                        {
                            long numerator = numerators[i];
                            if (numerator > 1)
                            {
                                for (int j = 0; j < denominators.Length; j++)
                                {
                                    long denominator = denominators[j];
                                    if (denominator > 1)
                                    {
                                        long divisor = MathUtilities.GreatestCommonDivisor(numerator, denominator);
                                        numerator /= divisor;
                                        denominator /= divisor;

                                        denominators[j] = denominator;
                                    }
                                }
                                numerators[i] = numerator;
                            }
                        }

                        if (denominators.Any(d => d != 1))
                        {
                            throw new InvalidOperationException();
                        }

                        long value = 1;
                        for (int i = 0; i < numerators.Length; i++)
                        {
                            value *= numerators[i];
                            if (value > 1000000)
                            {
                                return true;
                            }
                        }
                        return false;
                    }))
                .Count();
        }
    }
}
