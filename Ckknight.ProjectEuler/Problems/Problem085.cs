using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(85,
        @"By counting carefully it can be seen that a rectangular grid
        measuring 3 by 2 contains eighteen rectangles:

        +--+  +  +   +--+--+  +   +--+--+--+
        |  |         |     |      |        |
        +--+  +  +   +--+--+  +   +--+--+--+

        +  +  +  +   +  +  +  +   +  +  +  +
             6            4            2

        +--+  +  +   +--+--+  +   +--+--+--+
        |  |         |     |      |        |
        +  +  +  +   +     +  +   +        +
        |  |         |     |      |        |
        +--+  +  +   +--+--+  +   +--+--+--+
             6            4            2

        Although there exists no rectangular grid that contains exactly two
        million rectangles, find the area of the grid with the nearest
        solution.")]
    public class Problem085 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(1, int.MaxValue)
                .Select(x => new Range(1, x, true)
                    .SelectWithAggregate(new { x = 0, y = 0, num = 0, previous = 0 }, (a, y) => new
                        {
                            x = x,
                            y = y,
                            num = GetNumRectangles(x, y),
                            previous = a.num
                        })
                    .TakeWhile(q => q.previous < 2000000 && q.num < 2100000)
                    .TakeLast(2)
                    .ToArray())
                .TakeWhile(s => s.Any())
                .SelectMany(s => s)
                .WithMin(q => Math.Abs(2000000 - q.num))
                .Let(q => q.x * q.y);
        }

        private static readonly Dictionary<int, Dictionary<int, int>> _cache = new Dictionary<int, Dictionary<int, int>>();
        private static int GetNumRectangles(int x, int y)
        {
            if (x == 0 || y == 0)
            {
                return 0;
            }

            Dictionary<int, int> dict;
            if (!_cache.TryGetValue(x, out dict))
            {
                _cache[x] = dict = new Dictionary<int, int>();
            }

            int result;
            if (!dict.TryGetValue(y, out result))
            {
                dict[y] = result = GetNumRectangles(x - 1, y) + x * y * (y + 1) / 2;
            }
            return result;
        }
    }
}
