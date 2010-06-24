using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(87,
        @"The smallest number expressible as the sum of a prime square, prime
        cube, and prime fourth power is 28. In fact, there are exactly four
        numbers below fifty that can be expressed in such a way:

        28 = 2^2 + 2^3 + 2^4
        33 = 3^2 + 2^3 + 2^4
        49 = 5^2 + 2^3 + 2^4
        47 = 2^2 + 3^3 + 2^4

        How many numbers below fifty million can be expressed as the sum of a
        prime square, prime cube, and prime fourth power?")]
    public class Problem087 : BaseProblem
    {
        public override object CalculateResult()
        {
            return PrimeGenerator.Instance
                .Select(a => PrimeGenerator.Instance
                    .Select(b => PrimeGenerator.Instance
                        .Select(c => a*a + b*b*b + c*c*c*c)
                        .TakeWhile(x => x < 50000000))
                    .TakeWhile(s => s.Any())
                    .SelectMany(s => s))
                .TakeWhile(s => s.Any())
                .SelectMany(s => s)
                .Distinct()
                .Count();
        }
    }
}
