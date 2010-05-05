using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ckknight.ProjectEuler
{
    public static class MathUtilities
    {
        public static int GreatestCommonDivisor(int value, int other)
        {
            while (value != 0)
            {
                int tmp = value;
                value = other % value;
                other = tmp;
            }
            return other;
        }

        public static long GreatestCommonDivisor(long value, long other)
        {
            while (value != 0)
            {
                long tmp = value;
                value = other % value;
                other = tmp;
            }
            return other;
        }

        public static bool AreRelativelyPrime(int value, int other)
        {
            return GreatestCommonDivisor(value, other) == 1;
        }

        public static bool AreRelativelyPrime(long value, long other)
        {
            return GreatestCommonDivisor(value, other) == 1;
        }
    }
}
