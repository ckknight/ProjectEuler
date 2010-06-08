using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(67,
        @"By starting at the top of the triangle below and moving to adjacent
        numbers on the row below, the maximum total from top to bottom is 23.

                   3
                  7 4
                 2 4 6
                8 5 9 3

        That is, 3 + 7 + 4 + 9 = 23.

        Find the maximum total from top to bottom in triangle.txt
        (http://projecteuler.net/project/triangle.txt), a 15K text file
        containing a triangle with one-hundred rows.

        NOTE: This is a much more difficult version of Problem 18. It is not
        possible to try every route to solve this problem, as there are 2^99
        altogether! If you could check one trillion (10^12) routes every second
        it would take over twenty billion years to check them all. There is an
        efficient algorithm to solve it. ;o)")]
    public class Problem067 : BaseProblem
    {
        public Problem067()
        {
            GetText();
        }

        public override object CalculateResult()
        {
            return GetText()
                .Split('\n')
                .Select(l => l.Trim())
                .Where(l => l != string.Empty)
                .Select(l => l
                    .Split(' ')
                    .Select(c => int.Parse(c))
                    .ToArray())
                .Aggregate((previous, row) =>
                {
                    for (int i = 0; i < row.Length; i++)
                    {
                        List<int> parents = new List<int> { 0 };
                        if (i > 0)
                        {
                            parents.Add(previous[i - 1]);
                        }

                        if (i < previous.Length)
                        {
                            parents.Add(previous[i]);
                        }

                        row[i] += parents.Max();
                    }
                    return row;
                })
                .Max();
        }

        private readonly string Url = "http://projecteuler.net/project/triangle.txt";
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
