﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(29,
        @"Consider all integer combinations of ab for 2 <= a <= 5 and
        2 <= b <= 5:

            2^2=4, 2^3=8, 2^4=16, 2^5=32
            3^2=9, 3^3=27, 3^4=81, 3^5=243
            4^2=16, 4^3=64, 4^4=256, 4^5=1024
            5^2=25, 5^3=125, 5^4=625, 5^5=3125
        
        If they are then placed in numerical order, with any repeats removed,
        we get the following sequence of 15 distinct terms:

            4, 8, 9, 16, 25, 27, 32, 64, 81, 125, 243, 256, 625, 1024, 3125

        How many distinct terms are in the sequence generated by a^b for
        2 <= a <= 100 and 2 <= b <= 100?")]
    public class Problem029 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(2, 100, true)
                .Select(a => GetBaseAndExponent(a))
                .SelectMany(x => new Range(2, 100, true)
                    .Select(b => new { Base = x.Item1, Exponent = x.Item2 * b }))
                .Distinct()
                .Count();
        }

        public Tuple<int, int> GetBaseAndExponent(int number)
        {
            double log = Math.Log(number);
            int maximum = (int)Math.Sqrt(number);
            for (int i = 2; i <= maximum; i++)
            {
                double result = log / Math.Log(i);
                if ((result % 1.0) == 0)
                {
                    return Tuple.Create(i, (int)result);
                }
            }
            return Tuple.Create(number, 1);
        }
    }
}
