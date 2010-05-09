using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(39,
        @"If p is the perimeter of a right angle triangle with integral length
        sides, {a,b,c}, there are exactly three solutions for p = 120.

        {20,48,52}, {24,45,51}, {30,40,50}

        For which value of p <= 1000, is the number of solutions maximised?")]
    public class Problem039 : BaseProblem
    {
        public override object CalculateResult()
        {
            int maximum = 1000;

            return new Range(1, maximum / 3)
                .SelectMany(a => new Range(a + 1, (maximum - a) / 2)
                    .Select(b => new
                    {
                        a = a,
                        b = b,
                        cSquared = a * a + b * b
                    }))
                .Where(x => MathUtilities.IsPerfectSquare(x.cSquared))
                .Select(x => new
                {
                    a = x.a,
                    b = x.b,
                    c = (int)Math.Sqrt(x.cSquared),
                })
                .GroupBy(x => x.a + x.b + x.c)
                .Select(g => new
                {
                    Perimeter = g.Key,
                    Count = g.Count()
                })
                .Aggregate((a, b) => a.Count > b.Count ? a : b)
                .Perimeter;
        }
    }
}
