﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(44,
        @"Pentagonal numbers are generated by the formula, Pn=n(3n-1)/2. The
        first ten pentagonal numbers are:

                1, 5, 12, 22, 35, 51, 70, 92, 117, 145, ...

        It can be seen that P4 + P7 = 22 + 70 = 92 = P8. However, their
        difference, 70 - 22 = 48, is not pentagonal.

        Find the pair of pentagonal numbers, Pj and Pk, for which their sum and
        difference is pentagonal and D = |Pk - Pj| is minimised; what is the
        value of D?")]
    public class Problem044 : BaseProblem
    {
        public override object CalculateResult()
        {
            return GetPentagonals()
                .Take(3000)
                .SelectMany((p, i) => GetPentagonals().Take(i)
                    .Where(q => IsPentagonal(p + q))
                    .Select(q => p - q))
                .Where(d => IsPentagonal(d))
                .Min();
        }

        public IEnumerable<long> GetPentagonals()
        {
            long i = 1;
            while (true)
            {
                yield return GetPentagonal(i++);
            }
        }

        public long GetPentagonal(long index)
        {
            return (index * (3 * index - 1)) / 2;
        }

        public bool IsPentagonal(double value)
        {
            return ((Math.Sqrt(24*value + 1) + 1) / 6) % 1.0 == 0.0;
        }
    }
}
