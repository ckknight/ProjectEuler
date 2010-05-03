using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(16,
        @"2^15 = 32768 and the sum of its digits is 3 + 2 + 7 + 6 + 8 = 26.

        What is the sum of the digits of the number 2^1000?")]
    public class Problem016 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(0, 1000)
                .Aggregate(new List<int> { 1 }, (digits, x) =>
                {
                    int carry = 0;
                    for (int i = 0; i < digits.Count; i++)
                    {
                        int digit = digits[i] * 2 + carry;
                        carry = digit / 10;
                        digit %= 10;
                        digits[i] = digit;
                    }
                    if (carry > 0)
                    {
                        digits.Add(carry);
                    }
                    return digits;
                }, x => x.Sum());
        }
    }
}
