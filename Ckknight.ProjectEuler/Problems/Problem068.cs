using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(68,
        @"Consider the following ""magic"" 3-gon ring, filled with the numbers
        1 to 6, and each line adding to nine.

                 (4)
                   \
                   (3)
                   / \
                 (1)-(2)-(6)
                 /
               (5)

        Working clockwise, and starting from the group of three with the
        numerically lowest external node (4,3,2 in this example), each solution
        can be described uniquely. For example, the above solution can be
        described by the set: 4,3,2; 6,2,1; 5,1,3.

        It is possible to complete the ring with four different totals: 9, 10,
        11, and 12. There are eight solutions in total.

        Total       Solution Set
        9       4,2,3; 5,3,1; 6,1,2
        9       4,3,2; 6,2,1; 5,1,3
        10      2,3,5; 4,5,1; 6,1,3
        10      2,5,3; 6,3,1; 4,1,5
        11      1,4,6; 3,6,2; 5,2,4
        11      1,6,4; 5,4,2; 3,2,6
        12      1,5,6; 2,6,4; 3,4,5
        12      1,6,5; 3,5,4; 2,4,6
        
        By concatenating each group it is possible to form 9-digit strings; the
        maximum string for a 3-gon ring is 432621513.

        Using the numbers 1 to 10, and depending on arrangements, it is
        possible to form 16- and 17-digit strings. What is the maximum 16-digit
        string for a ""magic"" 5-gon ring?

                     ( )_      ( )
                        _( )_  /
                    _( )     ( )
                 ( )   \     /
                       ( )-( )-( )
                         \
                         ( )


        ")]
    public class Problem068 : BaseProblem
    {
        public override object CalculateResult()
        {
            var _digitCache = new DefaultDictionary<int, int[]>(n => MathUtilities.ToDigits(n, true));
            return new Range(7, 10, true)
                .GetPermutations()
                .Select(o => o.PrependItem(6).ToArray())
                .SelectMany(o => new Range(1, 5, true)
                    .GetPermutations()
                    .Select(i => new[] { new[] { o[0], i[0], i[1] }, new[] { o[1], i[1], i[2] }, new[] { o[2], i[2], i[3] }, new[] { o[3], i[3], i[4] }, new[] { o[4], i[4], i[0] } }))
                .Where(z => {
                    int value = z[0].Sum();
                    return z.Skip(1).All(x => x.Sum() == value);
                })
                .Select(z => z
                    .SelectMany(x => x.SelectMany(y => _digitCache[y]))
                    .ToArray())
                .Where(x => x.Length == 16)
                .Aggregate((a, b) => a.SequenceCompare(b) > 0 ? a : b)
                .StringJoin(string.Empty);
        }
    }
}
