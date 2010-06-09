using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(77,
        @"It is possible to write ten as the sum of primes in exactly five
        different ways:

            7 + 3
            5 + 5
            5 + 3 + 2
            3 + 3 + 2 + 2
            2 + 2 + 2 + 2 + 2

        What is the first value which can be written as the sum of primes in
        over five thousand different ways?")]
    public class Problem077 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(2, int.MaxValue)
                .First(n => GetNumPossibilities(n, PrimeGenerator.Instance.GetUpTo(n)
                    .Select(p => (int)p)
                    .Reverse()
                    .ToImmutableSequence()) > 5000);
        }

        public int GetNumPossibilities(int current, ImmutableSequence<int> possibilities)
        {
            if (current == 0)
            {
                return 1;
            }

            ImmutableSequence<int> otherPossibilities = possibilities.Skip(1);

            int possibility = possibilities.First();
            if (!otherPossibilities.HasValue)
            {
                if ((current % possibility) == 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            return new Range(current, 0, -possibility, true)
                .Sum(n => GetNumPossibilities(n, otherPossibilities));
        }
    }
}
