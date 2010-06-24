using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(82,
        @"NOTE: This problem is a more challenging version of Problem 81.

        The minimal path sum in the 5 by 5 matrix below, by starting in any
        cell in the left column and finishing in any cell in the right column,
        and only moving up, down, and right, is indicated in red and bold; the
        sum is equal to 994.

         131    673   [234]  [103]  [ 18]
        [201]  [ 96]  [342]   965    150
         630    803    746    422    111
         537    699    497    121    956
         805    732    524     37    331

        Find the minimal path sum, in matrix.txt
        (http://projecteuler.net/project/matrix.txt), a 31K text file
        containing a 80 by 80 matrix, from the left column to the right
        column.")]
    public class Problem082 : BaseProblem
    {
        public Problem082()
        {
            GetText();
        }

        private enum FromDirection
        {
            Left,
            Up,
            Down,
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
            
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);

            Func<int, int, FromDirection, int> calculate = null;
            calculate = (x, y, d) =>
            {
                if (x == 0)
                {
                    return grid[x, y];
                }
                else if (x == width - 1)
                {
                    return grid[x, y] + calculate(x - 1, y, FromDirection.Left);
                }
                else
                {
                    List<int> possibilities = new List<int> { calculate(x - 1, y, FromDirection.Left) };

                    if (y > 0 && d != FromDirection.Up)
                    {
                        possibilities.Add(calculate(x, y - 1, FromDirection.Down));
                    }

                    if (y < height - 1 && d != FromDirection.Down)
                    {
                        possibilities.Add(calculate(x, y + 1, FromDirection.Up));
                    }

                    return grid[x, y] + possibilities.Min();
                }
            };
            calculate = calculate.Memoize();

            return new Range(grid.GetLength(1))
                .Select(y => calculate(width - 1, y, FromDirection.Left))
                .Min();
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
