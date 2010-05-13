using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(48,
        @"The series, 1^1 + 2^2 + 3^3 + ... + 10^10 = 10405071317.
        
        Find the last ten digits of the series, 1^1 + 2^2 + 3^3 + ... + 1000^1000.")]
    public class Problem048 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, 1000, true)
                .Select(n => MathUtilities.ModPow(n, n, 10000000000))
                .Aggregate((a, b) => (a + b) % 10000000000);
        }
    }
}
