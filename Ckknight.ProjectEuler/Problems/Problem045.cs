﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(45,
        @"Triangle, pentagonal, and hexagonal numbers are generated by the following formulae:

        Triangle        Tn=n(n+1)/2     1, 3, 6, 10, 15, ...
        Pentagonal      Pn=n(3n-1)/2    1, 5, 12, 22, 35, ...
        Hexagonal       Hn=n(2n-1)      1, 6, 15, 28, 45, ...
        
        It can be verified that T(285) = P(165) = H(143) = 40755.

        Find the next triangle number that is also pentagonal and hexagonal.")]
    public class Problem045 : BaseProblem
    {
        public override object CalculateResult()
        {
            return GetTriangles()
                .OrderedIntersect(GetHexagons())
                .OrderedIntersect(GetPentagons())
                .ElementAt(2);
        }

        public IEnumerable<long> GetTriangles()
        {
            long n = 0;
            while (true)
            {
                n++;
                yield return (n * (n + 1)) / 2;
            }
        }

        public IEnumerable<long> GetPentagons()
        {
            long n = 0;
            while (true)
            {
                n++;
                yield return (n * (3 * n - 1)) / 2;
            }
        }

        public IEnumerable<long> GetHexagons()
        {
            long n = 0;
            while (true)
            {
                n++;
                yield return n * (2 * n - 1);
            }
        }
    }
}