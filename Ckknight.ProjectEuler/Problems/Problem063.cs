using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(63,
        @"The 5-digit number, 16807=7^5, is also a fifth power. Similarly, the
        9-digit number, 134217728=8^9, is a ninth power.
        
        How many n-digit positive integers exist which are also an nth power?")]
    public class Problem063 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, int.MaxValue)
                .Select(n => new Range(1, 10)
                    .Where(x => (int)Math.Log10(Math.Pow(x, n)) + 1 == n)
                    .Count())
                .TakeWhile(c => c > 0)
                .Sum();
        }
    }
}
