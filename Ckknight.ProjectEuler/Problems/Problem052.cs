using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(52,
        @"It can be seen that the number, 125874, and its double, 251748,
        contain exactly the same digits, but in a different order.

        Find the smallest positive integer, x, such that 2x, 3x, 4x, 5x, and
        6x, contain the same digits.")]
    public class Problem052 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(0, int.MaxValue)
                .SelectMany(p => {
                    int x = MathUtilities.Pow(10, p);
                    return new Range(x, (x * 10) / 6, true);
                })
                .First(x =>
                {
                    var orderedDigits = MathUtilities.ToDigits(x)
                        .OrderBy(d => d)
                        .ToArray();

                    return new Range(2, 6, true)
                        .All(n => MathUtilities.ToDigits(n * x)
                            .OrderBy(d => d)
                            .SequenceEqual(orderedDigits));
                });
        }
    }
}
