using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(56,
        @"A googol (10^100) is a massive number: one followed by one-hundred
        zeros; 100^100 is almost unimaginably large: one followed by
        two-hundred zeros. Despite their size, the sum of the digits in each
        number is only 1.

        Considering natural numbers of the form, a^b, where a, b < 100, what is
        the maximum digital sum?")]
    public class Problem056 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(2, 100)
                .SelectMany(a => new Range(1, 100)
                    .SelectWithAggregate(new[] { 1 }, (x, i) => {
                        var result = x.Select(v => v * a).ToList();

                        int carry = 0;
                        for (int j = 0; j < result.Count; j++)
                        {
                            int current = result[j] + carry;
                            result[j] = current % 10;
                            carry = current / 10;
                        }
                        while (carry > 0)
                        {
                            result.Add(carry % 10);
                            carry /= 10;
                        }

                        return result.ToArray();
                    }))
                .Select(d => d.Sum())
                .Max();
        }
    }
}
