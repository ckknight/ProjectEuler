using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Numerics;
using Ckknight.ProjectEuler.Collections;

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

        public static bool AreRelativelyPrime(BigInteger value, BigInteger other)
        {
            return BigInteger.GreatestCommonDivisor(value, other).IsOne;
        }

        public static int[] ToDigits(long value)
        {
            return ToDigits(value, false);
        }

        public static int[] ToDigits(long value, bool reverse)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException("value", value, "Must be at least 1");
            }

            int max = (int)Math.Floor(Math.Log10(value)) + 1;
            var array = new int[max];

            if (reverse)
            {
                for (int i = max - 1; i >= 0; i--)
                {
                    array[i] = (int)(value % 10);
                    value /= 10;
                }
            }
            else
            {
                for (int i = 0; i < max; i++)
                {
                    array[i] = (int)(value % 10);
                    value /= 10;
                }
            }

            return array;
        }

        public static int[] ToDigits(BigInteger value)
        {
            return ToDigits(value, false);
        }

        public static int[] ToDigits(BigInteger value, bool reverse)
        {
            if (value.Sign <= 0)
            {
                throw new ArgumentOutOfRangeException("value", value, "Must be at least 1");
            }

            int max = (int)Math.Floor(BigInteger.Log10(value)) + 1;
            var array = new int[max];

            if (reverse)
            {
                for (int i = max - 1; i >= 0; i--)
                {
                    BigInteger remainder;
                    value = BigInteger.DivRem(value, 10, out remainder);
                    array[i] = (int)remainder;
                }
            }
            else
            {
                for (int i = 0; i < max; i++)
                {
                    BigInteger remainder;
                    value = BigInteger.DivRem(value, 10, out remainder);
                    array[i] = (int)remainder;
                }
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

        public static BigInteger FromDigitsToBigInteger(IEnumerable<int> digits)
        {
            if (digits == null)
            {
                throw new ArgumentNullException("digits");
            }

            BigInteger total = 0;
            BigInteger power = 1;

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
            else if (exponent == 0 || number == 1)
            {
                return 1;
            }
            else if (exponent == 1)
            {
                return number;
            }
            else if (exponent == 2)
            {
                return number * number;
            }
            else if (exponent < 0)
            {
                throw new ArgumentOutOfRangeException("exponent", exponent, "Must be at least 0");
            }
            else if (number == -1)
            {
                if ((exponent & 1) == 0)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
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
            else if (exponent == 0L || number == 1L)
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
            else if (number == -1L)
            {
                if ((exponent & 1L) == 0)
                {
                    return 1L;
                }
                else
                {
                    return -1L;
                }
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

        public static long ModPow(long number, long exponent, long modulus)
        {
            if (modulus == 1L)
            {
                return 0L;
            }
            else if (modulus < 1L)
            {
                throw new ArgumentOutOfRangeException("modulus", modulus, "Must be at least 1");
            }
            number %= modulus;
            if (number == 0L)
            {
                return 0L;
            }
            else if (exponent == 0L || number == 1L)
            {
                return 1L;
            }
            else if (exponent == 1L)
            {
                return number;
            }
            else if (exponent == 2L)
            {
                return (number * number) % modulus;
            }
            else if (exponent < 0L)
            {
                throw new ArgumentOutOfRangeException("exponent", exponent, "Must be at least 0");
            }

            long result = 1L;
            for (long i = 0; i < exponent; i++)
            {
                result = (result * number) % modulus;
            }

            return result;
        }

        public static bool IsPerfectSquare(long number)
        {
            if ((number < 0) || ((number & 2) != 0) || ((number & 7) == 5) || ((number & 11) == 8))
            {
                return false;
            }
            else if (number == 0)
            {
                return true;
            }
            else
            {
                long root = (long)(Math.Sqrt(number));
                return root * root == number;
            }
        }

        public static bool IsPerfectSquare(BigInteger number)
        {
            if (number.Sign < 0)
            {
                return false;
            }
            if (number.IsZero || number.IsOne)
            {
                return true;
            }
            else
            {
                BigInteger root = BigSqrt(number);
                return root*root == number;
            }
        }

        public static bool IsPerfectSquare(int number)
        {
            if ((number < 0) || ((number & 2) != 0) || ((number & 7) == 5) || ((number & 11) == 8))
            {
                return false;
            }
            else if (number == 0)
            {
                return true;
            }
            else
            {
                int root = (int)(Math.Sqrt(number));
                return root * root == number;
            }
        }

        public static BigInteger BigSqrt(BigInteger value)
        {
            if (value.Sign < 0)
            {
                throw new ArithmeticException("Cannot take the square root of a negative");
            }
            else if (value.IsZero)
            {
                return BigInteger.Zero;
            }
            else if (value.IsOne)
            {
                return BigInteger.One;
            }
            else
            {
                BigInteger a = BigInteger.One;
                BigInteger b = (value >> 5) + 8;

                while (b >= a)
                {
                    BigInteger m = (a + b) >> 1;

                    if ((m * m) > value)
                    {
                        b = m - 1;
                    }
                    else
                    {
                        a = m + 1;
                    }
                }

                return a - BigInteger.One;
            }
        }

        public static long Totient(long n)
        {
            return new PrimeFactorGenerator(n)
                .Distinct()
                .Aggregate(n, (x, p) => x * (p - 1) / p);
        }
    }
}
