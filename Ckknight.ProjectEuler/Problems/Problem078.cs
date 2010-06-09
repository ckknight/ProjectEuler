using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;
using System.Numerics;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(78,
        @"Let p(n) represent the number of different ways in which n coins can be separated into piles. For example, five coins can separated into piles in exactly seven different ways, so p(5)=7.

        OOOOO
        OOOO   O
        OOO   OO
        OOO   O   O
        OO   OO   O
        OO   O   O   O
        O   O   O   O   O

        Find the least value of n for which p(n) is divisible by one million.")]
    public class Problem078 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, int.MaxValue)
                .First(n => GetNumUnrestrictedPartitions(n) == 0);
        }

        private readonly Dictionary<int, long> _numPartitions = new Dictionary<int, long>();
        public long GetNumUnrestrictedPartitions(int value)
        {
            if (value < 0)
            {
                return 0L;
            }
            return _numPartitions.GetOrAdd(value, n =>
            {
                if (n == 0)
                {
                    return 1L;
                }
                else
                {
                    return new Range(1, (int)n, true)
                        .Select(k => (long)k)
                        .Select(k => new
                        {
                            s = MathUtilities.Pow(-1L, k + 1L),
                            m1 = (int)(n - k * (3 * k - 1) / 2),
                            m2 = (int)(n - k * (3 * k + 1) / 2)
                        })
                        .TakeWhile(x => x.m1 >= 0 || x.m2 >= 0)
                        .Select(x =>
                        {
                            return Truncate(x.s
                                * (GetNumUnrestrictedPartitions(x.m1)
                                 + GetNumUnrestrictedPartitions(x.m2)), 1000000L);
                        })
                        .Aggregate(0L, (a, b) => Truncate(a + b, 1000000L));
                }
            });
        }

        public long Truncate(long value, long number)
        {
            value %= number;
            if (value < 0)
            {
                value += number;
            }
            return value;
        }
    }
}
