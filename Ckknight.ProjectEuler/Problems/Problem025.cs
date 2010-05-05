using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(25,
        @"The Fibonacci sequence is defined by the recurrence relation:

        F(n) = F(n - 1) + F(n - 2), where F(1) = 1 and F(2) = 1.
        Hence the first 12 terms will be:

        F(1) = 1
        F(2) = 1
        F(3) = 2
        F(4) = 3
        F(5) = 5
        F(6) = 8
        F(7) = 13
        F(8) = 21
        F(9) = 34
        F(10) = 55
        F(11) = 89
        F(12) = 144
        The 12th term, F(12), is the first term to contain three digits.

        What is the first term in the Fibonacci sequence to contain 1000 digits?")]
    public class Problem025 : BaseProblem
    {
        public override object CalculateResult()
        {
            return FibonacciSequence.Create(new[] { 1 }, new[] { 1 }, (a, b) =>
                {
                    List<int> sum = new List<int>();
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (sum.Count <= i)
                        {
                            sum.Add(0);
                        }
                        sum[i] += a[i];
                    }
                    for (int i = 0; i < b.Length; i++)
                    {
                        if (sum.Count <= i)
                        {
                            sum.Add(0);
                        }
                        sum[i] += b[i];
                    }

                    int carry = 0;
                    for (int i = 0; i < sum.Count; i++)
                    {
                        int current = sum[i] + carry;
                        sum[i] = current % 10;
                        carry = current / 10;
                    }
                    while (carry > 0)
                    {
                        sum.Add(carry % 10);
                        carry /= 10;
                    }

                    return sum.ToArray();
                })
                .Select((n, i) => new { Index = i + 1, DigitCount = n.Length })
                .First(x => x.DigitCount >= 1000)
                .Index;
        }
    }
}
