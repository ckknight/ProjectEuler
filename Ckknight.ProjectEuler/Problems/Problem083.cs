using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Ckknight.ProjectEuler.Collections;
using System.Threading.Tasks;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(83,
        @"NOTE: This problem is a significantly more challenging version of
        Problem 81.

        In the 5 by 5 matrix below, the minimal path sum from the top left to
        the bottom right, by moving left, right, up, and down, is indicated in
        bold red and is equal to 2297.


        [131]   673   [234]  [103]  [ 18]
        [201]  [ 96]  [342]   965   [150]
         630    803    746   [422]  [111]
         537    699    497   [121]   956
         805    732    524   [ 37]  [331]

        Find the minimal path sum, in matrix.txt (right click and 'Save Link/Target As...'), a 31K text file containing a 80 by 80 matrix, from the top left to the bottom right by moving left, right, up, and down.")]
    public class Problem083 : BaseProblem
    {
        public Problem083()
        {
            GetText();
        }

        private enum FromDirection
        {
            Left,
            Right,
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

            int[,] resultGrid = new int[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    resultGrid[x, y] = -1;
                }
            }
            resultGrid[0, 0] = grid[0, 0];

            bool hasChanges;
            do
            {
                hasChanges = false;

                for (int x = 0; x < width; x++)
                {
                    for (int y = (x == 0 ? 1 : 0); y < height; y++)
                    {
                        List<int> possibilities = new List<int>();

                        if (x > 0)
                        {
                            possibilities.Add(resultGrid[x - 1, y]);
                        }
                        if (x < width - 1)
                        {
                            possibilities.Add(resultGrid[x + 1, y]);
                        }
                        if (y > 0)
                        {
                            possibilities.Add(resultGrid[x, y - 1]);
                        }
                        if (y < height - 1)
                        {
                            possibilities.Add(resultGrid[x, y + 1]);
                        }

                        possibilities.RemoveAll(p => p == -1);
                        if (possibilities.Any())
                        {
                            int best = grid[x, y] + possibilities.Min();
                            if (best != resultGrid[x, y])
                            {
                                hasChanges = true;
                                resultGrid[x, y] = best;
                            }
                        }
                    }
                }
            } while (hasChanges);

            return resultGrid[width - 1, height - 1];
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
