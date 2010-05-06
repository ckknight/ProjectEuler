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

        public static List<int> ToDigitList(int value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException("value", value, "Must be at least 1");
            }
            var list = new List<int>();

            while (value > 0)
            {
                list.Add(value % 10);
                value /= 10;
            }

            return list;
        }

        public static List<int> ToDigitList(long value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException("value", value, "Must be at least 1");
            }
            var list = new List<int>();

            while (value > 0)
            {
                list.Add((int)(value % 10));
                value /= 10;
            }

            return list;
        }

        public static int Pow(int number, int exponent)
        {
            if (number == 0)
            {
                return 0;
            }
            else if (exponent == 0)
            {
                return 1;
            }
            else if (exponent == 1)
            {
                return number;
            }
            else if (exponent < 0)
            {
                throw new ArgumentOutOfRangeException("exponent", exponent, "Must be at least 0");
            }

            int result = 1;
            while (exponent != 0)
            {
                if ((exponent & 1) != 0)
                {
                    result *= number;
                }
                exponent >>= 1;
                number *= number;
            }

            return result;
        }

        public static long Pow(long number, long exponent)
        {
            if (number == 0L)
            {
                return 0L;
            }
            else if (exponent == 0L)
            {
                return 1L;
            }
            else if (exponent == 1L)
            {
                return number;
            }
            else if (exponent == 2L)
            {
                return number * number;
            }
            else if (exponent < 0L)
            {
                throw new ArgumentOutOfRangeException("exponent", exponent, "Must be at least 0");
            }

            long result = 1L;
            while (exponent != 0L)
            {
                if ((exponent & 1L) != 0L)
                {
                    result *= number;
                }
                exponent >>= 1;
                number *= number;
            }

            return result;
        }
    }
}
