using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ckknight.ProjectEuler
{
    public struct Fraction : IComparable<Fraction>, IEquatable<Fraction>
    {
        public Fraction(long numerator)
            : this(numerator, 1) { }

        public Fraction(long numerator, long denominator)
        {
            if (denominator == 0)
            {
                _numerator = Math.Sign(numerator);
                _denominator = 0;
            }
            else if (numerator == 0)
            {
                _numerator = 0;
                _denominator = 1;
            }
            else
            {
                long gcd = MathUtilities.GreatestCommonDivisor(numerator, denominator);

                if (gcd != 1)
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

        private readonly long _numerator;
        private readonly long _denominator;

        public long Numerator
        {
            get
            {
                return _numerator;
            }
        }

        public long Denominator
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
                return _numerator == 0 && _denominator != 0;
            }
        }

        public bool IsOne
        {
            get
            {
                return _numerator == 1 && _denominator == 1;
            }
        }

        public bool IsInteger
        {
            get
            {
                return _denominator == 1;
            }
        }

        public bool IsNaN
        {
            get
            {
                return _denominator == 0 && _numerator == 0;
            }
        }

        public bool IsPositiveInfinity
        {
            get
            {
                return _denominator == 0 && _numerator > 0;
            }
        }

        public bool IsNegativeInfinity
        {
            get
            {
                return _denominator == 0 && _numerator < 0;
            }
        }

        public int Sign
        {
            get
            {
                if (IsNaN)
                {
                    throw new ArithmeticException("This fraction is not a number");
                }
                else
                {
                    return Math.Sign(_numerator);
                }
            }
        }

        public static readonly Fraction NaN = default(Fraction);
        public static readonly Fraction PositiveInfinity = new Fraction(1, 0);
        public static readonly Fraction NegativeInfinity = new Fraction(-1, 0);
        public static readonly Fraction Zero = new Fraction(0);
        public static readonly Fraction One = new Fraction(1);
        public static readonly Fraction Epsilon = new Fraction(1, long.MaxValue);

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

        public bool Equals(Fraction other)
        {
            return this._numerator == other._numerator && this._denominator == other._denominator;
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }
            else if (other is Fraction)
            {
                return Equals((Fraction)other);
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

        public int CompareTo(Fraction other)
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
                Fraction difference = this - other;

                return difference.Sign;
            }
        }

        public static Fraction Pow(Fraction number, long exponent)
        {
            if (number.IsNaN)
            {
                return Fraction.NaN;
            }
            else if (number.IsZero)
            {
                return Fraction.Zero;
            }
            else if (exponent == 0L)
            {
                return Fraction.One;
            }
            else if (number.IsNegativeInfinity)
            {
                if ((exponent & 1L) != 0L)
                {
                    return Fraction.NegativeInfinity;
                }
                else
                {
                    return Fraction.PositiveInfinity;
                }
            }
            else if (number.IsPositiveInfinity)
            {
                return Fraction.PositiveInfinity;
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

            Fraction result = Fraction.One;
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

        public static Fraction Floor(Fraction number)
        {
            if (number._denominator == 0 || number.IsInteger)
            {
                return number;
            }
            else
            {
                return new Fraction((number._numerator - (number._numerator % number._denominator)) / number._denominator, 1);
            }
        }

        public static Fraction Ceiling(Fraction number)
        {
            if (number._denominator == 0 || number.IsInteger)
            {
                return number;
            }
            else
            {
                return new Fraction((number._numerator - (number._numerator % number._denominator)) / number._denominator + 1, 1);
            }
        }

        public static Fraction Abs(Fraction number)
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

        public static double Log(Fraction number, double baseValue)
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
                return Math.Log(number._numerator, baseValue) - Math.Log(number._denominator, baseValue);
            }
        }

        public static double Log(Fraction number)
        {
            return Log(number, Math.E);
        }

        public static double Log10(Fraction number)
        {
            return Log(number, 10.0);
        }

        public static Fraction operator *(Fraction multiplicand, Fraction multiplier)
        {
            if (multiplicand._denominator == 0 || multiplier._denominator == 0)
            {
                int sign = Math.Sign(multiplicand._numerator) * Math.Sign(multiplier._numerator);
                if (sign == 0)
                {
                    return Fraction.NaN;
                }
                else if (sign < 0)
                {
                    return Fraction.NegativeInfinity;
                }
                else
                {
                    return Fraction.PositiveInfinity;
                }
            }
            else
            {
                return new Fraction(multiplicand._numerator * multiplier._numerator, multiplicand._denominator * multiplier._denominator);
            }
        }

        public static Fraction Reciprocal(Fraction number)
        {
            return new Fraction(number._denominator, number._numerator);
        }

        public static Fraction operator /(Fraction dividend, Fraction divisor)
        {
            return dividend * Fraction.Reciprocal(divisor);
        }

        public static Fraction operator %(Fraction dividend, Fraction divisor)
        {
            Fraction remainder;
            DivRem(dividend, divisor, out remainder);
            return remainder;
        }

        public static Fraction DivRem(Fraction dividend, Fraction divisor, out Fraction remainder)
        {
            if (divisor._denominator == 0)
            {
                if (divisor._numerator == 0 || dividend._denominator == 0)
                {
                    remainder = Fraction.NaN;
                    return Fraction.NaN;
                }
                else
                {
                    remainder = dividend;
                    return Fraction.Zero;
                }
            }
            else
            {
                if (divisor.IsOne)
                {
                    Fraction quotient = Fraction.Floor(dividend);
                    remainder = dividend - quotient;
                    return quotient;
                }
                else
                {
                    Fraction quotient = Fraction.Floor(dividend / divisor);
                    remainder = dividend - (quotient * divisor);
                    return quotient;
                }
            }
        }

        public static Fraction operator +(Fraction augend, Fraction addend)
        {
            if (augend._denominator == 0)
            {
                if (augend._numerator == 0)
                {
                    return Fraction.NaN;
                }
                else if (augend._numerator < 0)
                {
                    if (addend._denominator == 0 && addend._numerator >= 0)
                    {
                        return Fraction.NaN;
                    }
                    else
                    {
                        return Fraction.NegativeInfinity;
                    }
                }
                else
                {
                    if (addend._denominator == 0 && addend._numerator <= 0)
                    {
                        return Fraction.NaN;
                    }
                    else
                    {
                        return Fraction.PositiveInfinity;
                    }
                }
            }
            else if (addend._denominator == 0)
            {
                return addend;
            }
            else
            {
                return new Fraction(augend._numerator * addend._denominator + augend._denominator * addend._numerator, augend._denominator * addend._denominator);
            }
        }

        public static Fraction operator -(Fraction minuend, Fraction subtrahend)
        {
            return minuend + (-subtrahend);
        }

        public static Fraction operator +(Fraction number)
        {
            return number;
        }

        public static Fraction operator -(Fraction number)
        {
            return new Fraction(-number._numerator, number._denominator);
        }

        public static bool operator ==(Fraction alpha, Fraction bravo)
        {
            return alpha.Equals(bravo);
        }

        public static bool operator !=(Fraction alpha, Fraction bravo)
        {
            return !alpha.Equals(bravo);
        }

        public static bool operator <(Fraction alpha, Fraction bravo)
        {
            return alpha.CompareTo(bravo) < 0;
        }

        public static bool operator >(Fraction alpha, Fraction bravo)
        {
            return alpha.CompareTo(bravo) > 0;
        }

        public static bool operator <=(Fraction alpha, Fraction bravo)
        {
            return alpha.CompareTo(bravo) <= 0;
        }

        public static bool operator >=(Fraction alpha, Fraction bravo)
        {
            return alpha.CompareTo(bravo) >= 0;
        }

        public static explicit operator Fraction(ulong number)
        {
            return new Fraction((long)number, 1);
        }

        public static implicit operator Fraction(long number)
        {
            return new Fraction(number, 1);
        }

        public static implicit operator Fraction(uint number)
        {
            return new Fraction(number, 1);
        }

        public static implicit operator Fraction(int number)
        {
            return new Fraction(number, 1);
        }

        public static implicit operator Fraction(ushort number)
        {
            return new Fraction(number, 1);
        }

        public static implicit operator Fraction(short number)
        {
            return new Fraction(number, 1);
        }

        public static implicit operator Fraction(byte number)
        {
            return new Fraction(number, 1);
        }

        public static implicit operator Fraction(sbyte number)
        {
            return new Fraction(number, 1);
        }

        public static explicit operator ulong(Fraction value)
        {
            return (ulong)(value._numerator / value._denominator);
        }

        public static explicit operator long(Fraction value)
        {
            return value._numerator / value._denominator;
        }

        public static explicit operator uint(Fraction value)
        {
            return (uint)((long)value);
        }

        public static explicit operator int(Fraction value)
        {
            return (int)((long)value);
        }

        public static explicit operator ushort(Fraction value)
        {
            return (ushort)((long)value);
        }

        public static explicit operator short(Fraction value)
        {
            return (short)((long)value);
        }

        public static explicit operator byte(Fraction value)
        {
            return (byte)((long)value);
        }

        public static explicit operator sbyte(Fraction value)
        {
            return (sbyte)((long)value);
        }

        public static explicit operator decimal(Fraction value)
        {
            return (decimal)value._numerator / (decimal)value._denominator;
        }

        public static explicit operator double(Fraction value)
        {
            return (double)value._numerator / (double)value._denominator;
        }

        public static explicit operator float(Fraction value)
        {
            return (float)((double)value);
        }

        public static explicit operator Fraction(double number)
        {
            if (number == double.NaN)
            {
                return Fraction.NaN;
            }
            else
            {
                long denominator = 1;
                while (number % 1.0 != 0.0)
                {
                    number *= 2;
                    denominator <<= 1;
                }

                return new Fraction((long)number, denominator);
            }
        }

        public static explicit operator Fraction(decimal number)
        {
            long denominator = 1;
            while (number % 1m != 0m)
            {
                number *= 10;
                denominator *= 10;
            }

            return new Fraction((long)number, denominator);
        }
    }
}
