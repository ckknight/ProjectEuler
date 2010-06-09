using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(76,
        @"It is possible to write five as a sum in exactly six different ways:

        4 + 1
        3 + 2
        3 + 1 + 1
        2 + 2 + 1
        2 + 1 + 1 + 1
        1 + 1 + 1 + 1 + 1

        How many different ways can one hundred be written as a sum of at least
        two positive integers?")]
    public class Problem076 : BaseProblem
    {
        public override object CalculateResult()
        {
            return GetNumUnrestrictedPartitions(100) - 1;
        }

        private readonly Dictionary<int, int> _numPartitions = new Dictionary<int, int>();
        public int GetNumUnrestrictedPartitions(int value)
        {
            return _numPartitions.GetOrAdd(value, n =>
            {
                if (n == 0)
                {
                    return 1;
                }
                else if (n < 0)
                {
                    return 0;
                }
                else
                {
                    return new Range(1, n, true)
                        .Sum(k => MathUtilities.Pow(-1, k + 1)
                                * (GetNumUnrestrictedPartitions(n - k * (3 * k - 1) / 2)
                                 + GetNumUnrestrictedPartitions(n - k * (3 * k + 1) / 2)));
                }
            });
        }
    }
}
