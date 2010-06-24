using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(86,
        @"A spider, S, sits in one corner of a cuboid room, measuring 6 by 5 by
        3, and a fly, F, sits in the opposite corner. By travelling on the
        surfaces of the room the shortest ""straight line"" distance from S to
        F is 10 and the path is shown on the diagram.

         ---------F
        |        /|
        |       / | 3
        |      /  |
         -----/---
        |    /    |
        |   /     |
        |  /      | 5
        | /       |
        |/        |
        S---------
             6

        However, there are up to three ""shortest"" path candidates for any
        given cuboid and the shortest route is not always integer.

        By considering all cuboid rooms with integer dimensions, up to a
        maximum size of M by M by M, there are exactly 2060 cuboids for which
        the shortest distance is integer when M=100, and this is the least
        value of M for which the number of solutions first exceeds two
        thousand; the number of solutions is 1975 when M=99.

        Find the least value of M such that the number of solutions first
        exceeds one million.")]
    public class Problem086 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, int.MaxValue, true)
                .SelectWithAggregate(new { m = 0, count = 0 }, (a, z) => new
                {
                    m = z,
                    count = a.count + new Range(2, 2 * z, true)
                        .Where(xy => MathUtilities.IsPerfectSquare(xy * xy + z * z))
                        .Sum(xy => (xy > z + 1) ? z + 1 - (xy + 1) / 2 : xy / 2)
                })
                .First(q => q.count > 1000000)
                .m;
        }
    }
}
