using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(81,
        @"In the 5 by 5 matrix below, the minimal path sum from the top left to
        the bottom right, by only moving to the right and down, is indicated in
        bold red and is equal to 2427.

        [131]   673    234    103     18
        [201]  [ 96]  [342]   965    150
         630    803   [746]  [422]   111
         537    699    497   [121]   956
         805    732    524   [ 37]  [331]

        Find the minimal path sum, in matrix.txt
        (http://projecteuler.net/project/matrix.txt), a 31K text file
        containing a 80 by 80 matrix, from the top left to the bottom right by
        only moving right and down.")]
    public class Problem081 : BaseProblem
    {
        public Problem081()
        {
            GetText();
        }

        public override object CalculateResult()
        {
            int[,] grid = GetText()
                .Split('\n')
                .Select(l => l.Trim())
                .Where(l => l != string.Empty)
                .Select(l => l.Split(',')
                    .Select(w => w.Trim())
                    .Where(w => w != string.Empty)
                    .Select(w => int.Parse(w))
                    .ToArray())
                .ToArray()
                .ToMatrix();

            Func<int, int, int> calculate = null;
            calculate = (x, y) =>
            {
                if (x == 0)
                {
                    if (y == 0)
                    {
                        return grid[x, y];
                    }
                    else
                    {
                        return grid[x, y] + calculate(x, y - 1);
                    }
                }
                else
                {
                    if (y == 0)
                    {
                        return grid[x, y] + calculate(x - 1, y);
                    }
                    else
                    {
                        int left = calculate(x - 1, y);
                        int up = calculate(x, y - 1);
                        int smaller = left < up ? left : up;
                        return grid[x, y] + smaller;
                    }
                }
            };
            calculate = calculate.Memoize();
            return calculate(grid.GetLength(0) - 1, grid.GetLength(1) - 1);
        }

        private readonly string Url = "http://projecteuler.net/project/matrix.txt";
        private string _textCache;
        public string GetText()
        {
            if (_textCache == null)
            {
                WebClient client = new WebClient();
                _textCache = client.DownloadString(Url);
            }

            return _textCache;
        }
    }
}
