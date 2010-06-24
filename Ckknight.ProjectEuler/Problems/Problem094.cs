using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;
using System.Threading.Tasks;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(94,
        @"It is easily proved that no equilateral triangle exists with integral
        length sides and integral area. However, the almost equilateral
        triangle 5-5-6 has an area of 12 square units.

        We shall define an almost equilateral triangle to be a triangle for
        which two sides are equal and the third differs by no more than one
        unit.

        Find the sum of the perimeters of all almost equilateral triangles with
        integral side lengths and area and whose perimeters do not exceed one
        billion (1,000,000,000).")]
    public class Problem094 : BaseProblem
    {
        public override object CalculateResult()
        {
            return new Range(3, 1000000000 / 3, 2)
                .AsParallel()
                .Sum(n =>
                {
                    long i = n;
                    long sum = 0;
                    if (MathUtilities.IsPerfectSquare(i * i - (i - 1) * (i - 1) / 4))
                    {
                        sum += 3 * i - 1;
                    }
                    if (MathUtilities.IsPerfectSquare(i * i - (i + 1) * (i + 1) / 4))
                    {
                        sum += 3 * i + 1;
                    }
                    return sum;
                });
        }
    }
}
