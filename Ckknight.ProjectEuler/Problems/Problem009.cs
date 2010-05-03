using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(9,
        @"A Pythagorean triplet is a set of three natural numbers, a < b < c, for which,

        a^2 + b^2 = c^2
        For example, 3^2 + 4^2 = 9 + 16 = 25 = 5^2.

        There exists exactly one Pythagorean triplet for which a + b + c = 1000.
        Find the product abc.")]
    public class Problem009 : BaseProblem
    {
        public override object CalculateResult()
        {
            int maximum = 1000;
            return new Range(1, maximum, true)
                .SelectMany(a => new Range(a + 1, maximum, true)
                    .Select(b => new { a, b, c = maximum - a - b }))
                .Where(x => x.c > x.b)
                .Where(x => x.a * x.a + x.b * x.b == x.c * x.c)
                .Select(x => x.a * x.b * x.c)
                .Single();
        }
    }
}
