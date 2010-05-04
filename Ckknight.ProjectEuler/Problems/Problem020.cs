using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(20,
        @"n! means n * (n - 1) * ... * 3 * 2 * 1

        Find the sum of the digits in the number 100!")]
    public class Problem020 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, 100, true)
                .Aggregate(new List<int> { 1 },
                    (accumulate, multiplier) =>
                    {
                        for (int i = 0; i < accumulate.Count; i++)
                        {
                            accumulate[i] *= multiplier;
                        }

                        int carry = 0;
                        for (int i = 0; i < accumulate.Count; i++)
                        {
                            int current = accumulate[i] + carry;
                            carry = current / 10;
                            current %= 10;
                            accumulate[i] = current;
                        }

                        while (carry > 0)
                        {
                            accumulate.Add(carry % 10);
                            carry /= 10;
                        }
                        return accumulate;
                    })
                .Sum();
        }
    }
}
