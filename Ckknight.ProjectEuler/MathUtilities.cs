using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

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

        public static int[] ToDigits(long value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException("value", value, "Must be at least 1");
            }

            int max = (int)Math.Floor(Math.Log10(value)) + 1;
            var array = new int[max];

            for (int i = 0; i < max; i++)
            {
                array[i] = (int)(value % 10);
                value /= 10;
            }

            return array;
        }

        public static long FromDigits(IEnumerable<int> digits)
        {
            if (digits == null)
            {
                throw new ArgumentNullException("digits");
            }

            long total = 0;
            long power = 1;

            foreach (int digit in digits)
            {
                total += digit * power;
                power *= 10;
            }

            return total;
        }

        public static BitArray ToBits(long value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException("value", value, "Must be at least 1");
            }

            int max = (int)Math.Floor(Math.Log(value, 2)) + 1;
            var array = new BitArray(max, false);

            for (int i = 0; i < max; i++)
            {
                array[i] = (value & 1) == 1;
                value /= 2;
            }

            return array;
        }

        public static long FromBits(BitArray bits)
        {
            if (bits == null)
            {
                throw new ArgumentNullException("bits");
            }

            long total = 0;
            int power = 0;

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    total += (1L << power);
                }
                power++;
            }

            return total;
        }

        public static long FromBits(IEnumerable<bool> bits)
        {
            if (bits == null)
            {
                throw new ArgumentNullException("bits");
            }

            long total = 0;
            int power = 0;

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    total += (1L << power);
                }
                power++;
            }

            return total;
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
