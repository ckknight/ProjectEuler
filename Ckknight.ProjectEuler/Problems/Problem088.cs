using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(88,
        @"A natural number, N, that can be written as the sum and product of a
        given set of at least two natural numbers, {a1, a2, ... , ak} is called
        a product-sum number: N = a1 + a2 + ... + ak = a1 * a2 * ... * ak.

        For example, 6 = 1 + 2 + 3 = 1 * 2 * 3.

        For a given set of size, k, we shall call the smallest N with this
        property a minimal product-sum number. The minimal product-sum numbers
        for sets of size, k = 2, 3, 4, 5, and 6 are as follows.

        k=2: 4 = 2 * 2 = 2 + 2
        k=3: 6 = 1 * 2 * 3 = 1 + 2 + 3
        k=4: 8 = 1 * 1 * 2 * 4 = 1 + 1 + 2 + 4
        k=5: 8 = 1 * 1 * 2 * 2 * 2 = 1 + 1 + 2 + 2 + 2
        k=6: 12 = 1 * 1 * 1 * 1 * 2 * 6 = 1 + 1 + 1 + 1 + 2 + 6

        Hence for 2 <= k <= 6, the sum of all the minimal product-sum numbers
        is 4+6+8+12 = 30; note that 8 is only counted once in the sum.

        In fact, as the complete set of minimal product-sum numbers for
        2 <= k <= 12 is {4, 6, 8, 12, 15, 16}, the sum is 61.

        What is the sum of all the minimal product-sum numbers for
        2 <= k <= 12000?")]
    public class Problem088 : BaseProblem
    {
        public override object CalculateResult()
        {
            int maximum = 12000;

            return new Range(2, int.MaxValue)
                .TakeWhile(l => MathUtilities.Pow(2, l) - l <= maximum)
                .SelectMany(l => CollectionUtilities.Repeat(default(object))
                    .SelectWithAggregate(Enumerable.Repeat(2, l - 1).AppendItem(1).ToArray(), (factors, o) =>
                    {
                        for (int i = factors.Length - 1; i >= 0; i--)
                        {
                            int value = factors[i] + 1;
                            for (int j = i; j < factors.Length; j++)
                            {
                                factors[j] = value;
                            }

                            if (factors.Product() - factors.Sum() <= maximum - l)
                            {
                                return factors;
                            }
                        }
                        return null;
                    })
                    .TakeWhile(factors => factors != null)
                    .Select(factors =>
                    {
                        int product = factors.Product();
                        return new
                        {
                            k = product - factors.Sum() + l,
                            product,
                        };
                    }))
                .Where(x => x.k >= 0 && x.k <= maximum)
                .GroupBy(x => x.k, x => x.product)
                .Select(g => g.Min())
                .Distinct()
                .Sum();
        }
    }
}
