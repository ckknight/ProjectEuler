using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(38,
        @"Take the number 192 and multiply it by each of 1, 2, and 3:

        192 * 1 = 192
        192 * 2 = 384
        192 * 3 = 576
        
        By concatenating each product we get the 1 to 9 pandigital, 192384576.
        We will call 192384576 the concatenated product of 192 and (1,2,3)

        The same can be achieved by starting with 9 and multiplying by 1, 2, 3,
        4, and 5, giving the pandigital, 918273645, which is the concatenated
        product of 9 and (1,2,3,4,5).

        What is the largest 1 to 9 pandigital 9-digit number that can be formed
        as the concatenated product of an integer with (1,2, ... , n) where
        n > 1?")]
    public class Problem038 : BaseProblem
    {
        public override object CalculateResult()
        {
            int theoreticalMaximum = 987654321;
            return new Range(1, (int)Math.Ceiling(Math.Sqrt(theoreticalMaximum)), true)
                .Select(i =>
                {
                    var digits = new List<int>();
                    int n = 1;
                    while (digits.Count < 9)
                    {
                        digits.AddRange(MathUtilities.ToDigits(i * n).Reverse());
                        n++;
                    }
                    digits.Reverse();
                    return digits;
                })
                .Where(d => d.Count == 9)
                .Where(d => d.Distinct().Count() == 9)
                .Where(d => !d.Contains(0))
                .Select(d => MathUtilities.FromDigits(d))
                .Max();
        }
    }
}
