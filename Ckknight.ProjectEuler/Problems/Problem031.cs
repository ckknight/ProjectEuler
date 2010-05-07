using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(31,
        @"In England the currency is made up of pound, £, and pence, p, and
        there are eight coins in general circulation:

        1p, 2p, 5p, 10p, 20p, 50p, £1 (100p) and £2 (200p).
        It is possible to make £2 in the following way:

        1£1 + 150p + 220p + 15p + 12p + 31p
        How many different ways can £2 be made using any number of coins?")]
    public class Problem031 : BaseProblem
    {
        public override object CalculateResult()
        {
            return GetNumPossibilities(200, new ImmutableSequence<int>(200, 100, 50, 20, 10, 5, 2, 1));
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
