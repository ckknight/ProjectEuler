using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(91,
        @"The points P (x1, y1) and Q (x2, y2) are plotted at integer
        co-ordinates and are joined to the origin, O(0,0), to form ΔOPQ.

        Graph shows a triangle with ∠POQ ≈ 30°, ∠OPQ ≈ 60°, ∟PQO = 90°

        There are exactly fourteen triangles containing a right angle that can
        be formed when each co-ordinate lies between 0 and 2 inclusive; that
        is,
        0 <= x1, y1, x2, y2 <= 2.
        
        (0, 1) and (1, 0)
        (1, 1) and (1, 0)
        (0, 2) and (1, 0)
        (1, 2) and (1, 0)
        (0, 1) and (2, 0)
        (1, 1) and (2, 0)
        (2, 1) and (2, 0)
        (0, 2) and (2, 0)
        (2, 2) and (2, 0)
        (0, 1) and (1, 1)
        (0, 1) and (2, 1)
        (0, 2) and (1, 1)
        (0, 2) and (1, 2)
        (0, 2) and (2, 2)

        Given that 0 <= x1, y1, x2, y2 <= 50, how many right triangles can be formed?")]
    public class Problem091 : BaseProblem
    {
        public override object CalculateResult()
        {
            int maximum = 51;
            return new Range(0, MathUtilities.Pow(maximum, 4))
                .AsParallel()
                .Select(q => new
                {
                    x1 = q % maximum,
                    y1 = (q / maximum) % maximum,
                    x2 = (q / maximum / maximum) % maximum,
                    y2 = (q / maximum / maximum / maximum) % maximum,
                })
                .Where(q => q.y2 - q.x2 < q.y1 - q.x1)
                .Where(q => q.x1 != 0 || (q.y1 != 0 && q.x2 != 0))
                .Where(q => q.y2 != 0 || (q.x2 != 0 && q.y1 != 0))
                .Select(q => new
                {
                    a = q.x1 * q.x1 + q.y1 * q.y1,
                    b = q.x2 * q.x2 + q.y2 * q.y2,
                    c = MathUtilities.Pow(q.x1 - q.x2, 2) + MathUtilities.Pow(q.y1 - q.y2, 2)
                })
                .Count(q => q.a + q.b == q.c || q.a + q.c == q.b || q.b + q.c == q.a);
        }
    }
}
