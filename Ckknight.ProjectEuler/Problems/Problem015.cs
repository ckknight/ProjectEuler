using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(15,
        @"Starting in the top left corner of a 22 grid, there are 6 routes (without backtracking) to the bottom right corner.

                XXXXXXX    XXXX--     XXXX--
                |  |  X    |  X  |    |  X  |
                 -- --X     --XXXX     --X--
                |  |  X    |  |  X    |  X  |
                 -- --X     -- --X     --XXXX

                X-- --     X-- --     X-- --
                X  |  |    X  |  |    X  |  |
                XXXXXXX    XXXX--     X-- --
                |  |  X    |  X  |    X  |  |
                 -- --X     --XXXX    XXXXXXX

        How many routes are there through a 20x20 grid?")]
    public class Problem015 : BaseProblem
    {
        public override object CalculateResult()
        {
            return GetNumPaths(20, 20);
        }

        private readonly Dictionary<Tuple<int, int>, int> _cache = new Dictionary<Tuple<int, int>, int>
        {
            { Tuple.Create(0, 0), 1 }
        };
        public int GetNumPaths(int x, int y)
        {
            var key = Tuple.Create(x, y);
            int numRoutes;
            if (!_cache.TryGetValue(key, out numRoutes))
            {
                numRoutes = 0;
                if (x > 0)
                {
                    numRoutes += GetNumPaths(x - 1, y);
                }
                if (y > 0)
                {
                    numRoutes += GetNumPaths(x, y - 1);
                }
                _cache[key] = numRoutes;
            }
            return numRoutes;
        }
    }
}
