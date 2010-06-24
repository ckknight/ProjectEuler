using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(90,
        @"Each of the six faces on a cube has a different digit (0 to 9)
        written on it; the same is done to a second cube. By placing the two
        cubes side-by-side in different positions we can form a variety of
        2-digit numbers.

        For example, the square number 64 could be formed:

              +---+   +---+
             /   /|  /   /|
            +---+ + +---+ +
            | 6 |/  | 4 |/
            +---+   +---+

        In fact, by carefully choosing the digits on both cubes it is possible
        to display all of the square numbers below one-hundred: 01, 04, 09,
        16, 25, 36, 49, 64, and 81.

        For example, one way this can be achieved is by placing {0, 5, 6, 7, 8,
        9} on one cube and {1, 2, 3, 4, 8, 9} on the other cube.

        However, for this problem we shall allow the 6 or 9 to be turned
        upside-down so that an arrangement like {0, 5, 6, 7, 8, 9} and {1, 2,
        3, 4, 6, 7} allows for all nine square numbers to be displayed;
        otherwise it would be impossible to obtain 09.

        In determining a distinct arrangement we are interested in the digits
        on each cube, not the order.

            {1, 2, 3, 4, 5, 6} is equivalent to {3, 6, 4, 1, 2, 5}
            {1, 2, 3, 4, 5, 6} is distinct from {1, 2, 3, 4, 5, 9}

        But because we are allowing 6 and 9 to be reversed, the two distinct
        sets in the last example both represent the extended set {1, 2, 3, 4,
        5, 6, 9} for the purpose of forming 2-digit numbers.

        How many distinct arrangements of the two cubes allow for all of the
        square numbers to be displayed?")]
    public class Problem090 : BaseProblem
    {
        public override object CalculateResult()
        {
            var cubes = new Range(0, 9, true)
                .GetCombinations(6)
                .ToArray();

            var squares = new Range(1, 9, true)
                .Select(n => n * n)
                .ToHashSet();

            var numberTransforms = new Range(0, 9, true)
                .ToDictionary(n => n, n => n == 6 || n == 9 ? new[] { 6, 9 } : new[] { n });

            return cubes
                .SelectMany((c1, i) => cubes.Skip(i)
                    .Select(c2 => new { Cube1 = c1, Cube2 = c2 }))
                .Select(x => x.Cube1
                    .SelectMany(a => x.Cube2
                        .SelectMany(b => numberTransforms[a]
                            .SelectMany(i => numberTransforms[b]
                                .SelectMany(j => new[] { i * 10 + j, j * 10 + i })))))
                .Count(x => squares.IsSubsetOf(x));
        }
    }
}
