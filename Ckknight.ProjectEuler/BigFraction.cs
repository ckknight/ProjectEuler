using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Runtime.Serialization;

namespace Ckknight.ProjectEuler
{
    [Serializable]
    public struct BigFraction : IComparable<BigFraction>, IEquatable<BigFraction>, ISerializable
    {
        public BigFraction(BigInteger numerator)
            : this(numerator, 1) { }

        public BigFraction(BigInteger numerator, BigInteger denominator)
        {
            if (denominator == 0)
            {
                _numerator = numerator.Sign;
                _denominator = 0;
            }
            else if (numerator == 0)
            {
                _numerator = 0;
                _denominator = 1;
            }
            else
            {
                BigInteger gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);

                if (!gcd.IsOne)
                {
                    numerator /= gcd;
                    denominator /= gcd;
                }

                if (denominator < 0)
                {
                    numerator = -numerator;
                    denominator = -denominator;
                }
                _numerator = numerator;
                _denominator = denominator;
            }
        }

        private readonly BigInteger _numerator;
        private readonly BigInteger _denominator;

        public BigInteger Numerator
        {
            get
            {
                return _numerator;
            }
        }

        public BigInteger Denominator
        {
            get
            {
                return _denominator;
            }
        }

        public bool IsZero
        {
            get
            {
                return _numerator.IsZero && !_denominator.IsZero;
            }
        }

        public bool IsOne
        {
            get
            {
                return _numerator.IsOne && _denominator.IsOne;
            }
        }

        public bool IsInteger
        {
            get
            {
                return _denominator.IsOne;
            }
        }

        public bool IsNaN
        {
            get
            {
                return _denominator.IsZero && _numerator.IsZero;
            }
        }

        public bool IsPositiveInfinity
        {
            get
            {
                return _denominator.IsZero && _numerator.Sign > 0;
            }
        }

        public bool IsNegativeInfinity
        {
            get
            {
                return _denominator.IsZero && _numerator.Sign < 0;
            }
        }

        public int Sign
        {
            get
            {
                if (IsNaN)
                {
                    throw new ArithmeticException("This BigFraction is not a number");
                }
                else
                {
                    return _numerator.Sign;
                }
            }
        }

        public static readonly BigFraction NaN = default(BigFraction);
        public static readonly BigFraction PositiveInfinity = new BigFraction(1, 0);
        public static readonly BigFraction NegativeInfinity = new BigFraction(-1, 0);
        public static readonly BigFraction Zero = new BigFraction(0);
        public static readonly BigFraction One = new BigFraction(1);

        public override string ToString()
        {
            if (_denominator == 0)
            {
                if (_numerator == 0)
                {
                    return "NaN";
                }
                else if (_numerator > 0)
                {
                    return "Infinity";
                }
                else
                {
                    return "-Infinity";
                }
            }
            else if (IsZero)
            {
                return "0";
            }
            else if (IsInteger)
            {
                return _numerator.ToString();
            }
            else
            {
                return string.Format("{0}/{1}", _numerator, _denominator);
            }
        }

        public bool Equals(BigFraction other)
        {
            return this._numerator.Equals(other._numerator) && this._denominator.Equals(other._denominator);
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }
            else if (other is BigFraction)
            {
                return Equals((BigFraction)other);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return _numerator.GetHashCode() ^ _denominator.GetHashCode();
        }

        public int CompareTo(BigFraction other)
        {
            if (this.Equals(other))
            {
                return 0;
            }
            else if (this.IsNaN)
            {
                return -1;
            }
            else if (other.IsNaN)
            {
                return 1;
            }
            else if (this.IsNegativeInfinity)
            {
                return -1;
            }
            else if (other.IsNegativeInfinity)
            {
                return 1;
            }
            else if (this.IsPositiveInfinity)
            {
                return 1;
            }
            else if (other.IsPositiveInfinity)
            {
                return -1;
            }
            else
            {
                BigFraction difference = this - other;

                return difference.Sign;
            }
        }

        public static BigFraction Pow(BigFraction number, long exponent)
        {
            if (number.IsNaN)
            {
                return BigFraction.NaN;
            }
            else if (number.IsZero)
            {
                return BigFraction.Zero;
            }
            else if (exponent == 0L)
            {
                return BigFraction.One;
            }
            else if (number.IsNegativeInfinity)
            {
                if ((exponent & 1L) != 0L)
                {
                    return BigFraction.NegativeInfinity;
                }
                else
                {
                    return BigFraction.PositiveInfinity;
                }
            }
            else if (number.IsPositiveInfinity)
            {
                return BigFraction.PositiveInfinity;
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

            BigFraction result = BigFraction.One;
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

        public static BigFraction Floor(BigFraction number)
        {
            if (number._denominator == 0 || number.IsInteger)
            {
                return number;
            }
            else
            {
                return new BigFraction((number._numerator - (number._numerator % number._denominator)) / number._denominator, 1);
            }
        }

        public static BigFraction Ceiling(BigFraction number)
        {
            if (number._denominator == 0 || number.IsInteger)
            {
                return number;
            }
            else
            {
                return new BigFraction((number._numerator - (number._numerator % number._denominator)) / number._denominator + 1, 1);
            }
        }

        public static BigFraction Abs(BigFraction number)
        {
            if (number._numerator >= 0)
            {
                return number;
            }
            else
            {
                return -number;
            }
        }

        public static double Log(BigFraction number, double baseValue)
        {
            if (number._denominator == 0)
            {
                if (number._numerator > 0)
                {
                    return double.PositiveInfinity;
                }
                else
                {
                    return double.NaN;
                }
            }
            else if (number._numerator == 0)
            {
                return double.NegativeInfinity;
            }
            else
            {
                return BigInteger.Log(number._numerator, baseValue) - BigInteger.Log(number._denominator, baseValue);
            }
        }

        public static double Log(BigFraction number)
        {
            return Log(number, Math.E);
        }

        public static double Log10(BigFraction number)
        {
            return Log(number, 10.0);
        }

        public static BigFraction operator *(BigFraction multiplicand, BigFraction multiplier)
        {
            if (multiplicand._denominator == 0 || multiplier._denominator == 0)
            {
                int sign = multiplicand._numerator.Sign * multiplier._numerator.Sign;
                if (sign == 0)
                {
                    return BigFraction.NaN;
                }
                else if (sign < 0)
                {
                    return BigFraction.NegativeInfinity;
                }
                else
                {
                    return BigFraction.PositiveInfinity;
                }
            }
            else
            {
                return new BigFraction(multiplicand._numerator * multiplier._numerator, multiplicand._denominator * multiplier._denominator);
            }
        }

        public static BigFraction Reciprocal(BigFraction number)
        {
            return new BigFraction(number._denominator, number._numerator);
        }

        public static BigFraction operator /(BigFraction dividend, BigFraction divisor)
        {
            return dividend * BigFraction.Reciprocal(divisor);
        }

        public static BigFraction operator %(BigFraction dividend, BigFraction divisor)
        {
            BigFraction remainder;
            DivRem(dividend, divisor, out remainder);
            return remainder;
        }

        public static BigFraction DivRem(BigFraction dividend, BigFraction divisor, out BigFraction remainder)
        {
            if (divisor._denominator == 0)
            {
                if (divisor._numerator == 0 || dividend._denominator == 0)
                {
                    remainder = BigFraction.NaN;
                    return BigFraction.NaN;
                }
                else
                {
                    remainder = dividend;
                    return BigFraction.Zero;
                }
            }
            else
            {
                if (divisor.IsOne)
                {
                    BigFraction quotient = BigFraction.Floor(dividend);
                    remainder = dividend - quotient;
                    return quotient;
                }
                else
                {
                    BigFraction quotient = BigFraction.Floor(dividend / divisor);
                    remainder = dividend - (quotient * divisor);
                    return quotient;
                }
            }
        }

        public static BigFraction operator +(BigFraction augend, BigFraction addend)
        {
            if (augend._denominator == 0)
            {
                if (augend._numerator == 0)
                {
                    return BigFraction.NaN;
                }
                else if (augend._numerator < 0)
                {
                    if (addend._denominator == 0 && addend._numerator >= 0)
                    {
                        return BigFraction.NaN;
                    }
                    else
                    {
                        return BigFraction.NegativeInfinity;
                    }
                }
                else
                {
                    if (addend._denominator == 0 && addend._numerator <= 0)
                    {
                        return BigFraction.NaN;
                    }
                    else
                    {
                        return BigFraction.PositiveInfinity;
                    }
                }
            }
            else if (addend._denominator == 0)
            {
                return addend;
            }
            else
            {
                return new BigFraction(augend._numerator * addend._denominator + augend._denominator * addend._numerator, augend._denominator * addend._denominator);
            }
        }

        public static BigFraction operator -(BigFraction minuend, BigFraction subtrahend)
        {
            return minuend + (-subtrahend);
        }

        public static BigFraction operator +(BigFraction number)
        {
            return number;
        }

        public static BigFraction operator -(BigFraction number)
        {
            return new BigFraction(-number._numerator, number._denominator);
        }

        public static bool operator ==(BigFraction alpha, BigFraction bravo)
        {
            return alpha.Equals(bravo);
        }

        public static bool operator !=(BigFraction alpha, BigFraction bravo)
        {
            return !alpha.Equals(bravo);
        }

        public static bool operator <(BigFraction alpha, BigFraction bravo)
        {
            return alpha.CompareTo(bravo) < 0;
        }

        public static bool operator >(BigFraction alpha, BigFraction bravo)
        {
            return alpha.CompareTo(bravo) > 0;
        }

        public static bool operator <=(BigFraction alpha, BigFraction bravo)
        {
            return alpha.CompareTo(bravo) <= 0;
        }

        public static bool operator >=(BigFraction alpha, BigFraction bravo)
        {
            return alpha.CompareTo(bravo) >= 0;
        }

        public static implicit operator BigFraction(Fraction number)
        {
            return new BigFraction(number.Numerator, number.Denominator);
        }

        public static implicit operator BigFraction(BigInteger number)
        {
            return new BigFraction(number);
        }

        public static implicit operator BigFraction(long number)
        {
            return new BigFraction(number, 1);
        }

        public static implicit operator BigFraction(int number)
        {
            return new BigFraction(number, 1);
        }

        public static explicit operator BigInteger(BigFraction value)
        {
            return value._numerator / value._denominator;
        }

        public static explicit operator ulong(BigFraction value)
        {
            return (ulong)((BigInteger)value);
        }

        public static explicit operator long(BigFraction value)
        {
            return (long)((BigInteger)value);
        }

        public static explicit operator uint(BigFraction value)
        {
            return (uint)((BigInteger)value);
        }

        public static explicit operator int(BigFraction value)
        {
            return (int)((BigInteger)value);
        }

        public static explicit operator ushort(BigFraction value)
        {
            return (ushort)((BigInteger)value);
        }

        public static explicit operator short(BigFraction value)
        {
            return (short)((BigInteger)value);
        }

        public static explicit operator byte(BigFraction value)
        {
            return (byte)((BigInteger)value);
        }

        public static explicit operator sbyte(BigFraction value)
        {
            return (sbyte)((BigInteger)value);
        }

        public static explicit operator decimal(BigFraction value)
        {
            return (decimal)value._numerator / (decimal)value._denominator;
        }

        public static explicit operator double(BigFraction value)
        {
            return Math.Exp(BigFraction.Log(value));
        }

        public static explicit operator float(BigFraction value)
        {
            return (float)((double)value);
        }

        public static explicit operator BigFraction(double number)
        {
            if (number == double.NaN)
            {
                return BigFraction.NaN;
            }
            else
            {
                int power = 0;
                while (number % 1.0 != 0.0)
                {
                    number *= 2;
                    power++;
                }

                return new BigFraction((BigInteger)number, BigInteger.Pow(2, power));
            }
        }

        public static explicit operator BigFraction(decimal number)
        {
            int power = 0;
            while (number % 1m != 0m)
            {
                number *= 10m;
                power++;
            }

            return new BigFraction((BigInteger)number, BigInteger.Pow(10, power));
        }
        
        #region ISerializable Members
        
        private BigFraction(SerializationInfo info, StreamingContext context)
            : this((BigInteger)info.GetValue("n", typeof(BigInteger)), (BigInteger)info.GetValue("d", typeof(BigInteger))) { }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("n", _numerator, typeof(BigInteger));
            info.AddValue("d", _denominator, typeof(BigInteger));
        }

        #endregion
    }
}
