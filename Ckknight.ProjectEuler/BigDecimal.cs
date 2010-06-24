using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Runtime.Serialization;

namespace Ckknight.ProjectEuler
{
    [Serializable]
    public struct BigDecimal : IComparable<BigDecimal>, IEquatable<BigDecimal>, ISerializable
    {
        public BigDecimal(BigInteger coefficient)
            : this(coefficient, BigInteger.Zero) { }

        public BigDecimal(BigInteger coefficient, BigInteger exponent)
        {
            Reduce(ref coefficient, ref exponent);

            _coefficient = coefficient;
            _exponent = exponent;
        }

        public BigDecimal(int value)
            : this((BigInteger)value) { }

        public BigDecimal(long value)
            : this((BigInteger)value) { }

        public BigDecimal(decimal value)
        {
            if (value == 0m)
            {
                _coefficient = BigInteger.Zero;
                _exponent = BigInteger.Zero;
            }
            else
            {
                bool negative = value < 0m;
                if (negative)
                {
                    value = -value;
                }
                BigInteger bigValue = (BigInteger)value;
                value %= 1m;
                int shifts = 0;
                while (value > 0m)
                {
                    value *= 10m;
                    bigValue *= BigTen;
                    bigValue += (int)value;
                    value %= 1m;
                    shifts--;
                }
                if (negative)
                {
                    bigValue = -bigValue;
                }

                BigInteger exponent = shifts;

                Reduce(ref bigValue, ref exponent);
                _coefficient = bigValue;
                _exponent = exponent;
            }
        }
            
        private static readonly BigInteger BigTen = new BigInteger(10);
        private static void Reduce(ref BigInteger coefficient, ref BigInteger exponent)
        {
            if (coefficient.IsZero)
            {
                exponent = BigInteger.Zero;
            }
            else
            {
                while (true)
                {
                    BigInteger remainder;
                    BigInteger result = BigInteger.DivRem(coefficient, BigTen, out remainder);
                    if (!remainder.IsZero)
                    {
                        break;
                    }
                    coefficient = result;
                    exponent++;
                }
            }
        }

        private readonly BigInteger _coefficient;
        private readonly BigInteger _exponent;
        public BigInteger Coefficient
        {
            get
            {
                return _coefficient;
            }
        }
        public BigInteger Exponent
        {
            get
            {
                return _exponent;
            }
        }

        public static readonly BigDecimal Zero = new BigDecimal();
        public static readonly BigDecimal One = new BigDecimal(1);
        public static readonly BigDecimal MinusOne = new BigDecimal(-1);

        public bool IsZero
        {
            get
            {
                return _coefficient.IsZero;
            }
        }

        public bool IsOne
        {
            get
            {
                return _coefficient.IsOne && _exponent.IsZero;
            }
        }

        public bool IsInteger
        {
            get
            {
                return _exponent.Sign >= 0;
            }
        }

        public int Sign
        {
            get
            {
                return _coefficient.Sign;
            }
        }

        private static readonly int _maximumExponentToStringSize = 1000;
        public override string ToString()
        {
            string coefficientString = _coefficient.ToString();
            if (_exponent.IsZero)
            {
                return coefficientString;
            }
            else
            {
                StringBuilder sb = new StringBuilder(coefficientString);
                if (_exponent > 0)
                {
                    if (_exponent > _maximumExponentToStringSize)
                    {
                        sb.Append('e');
                        sb.Append(_exponent);
                    }
                    else
                    {
                        int exponent = (int)_exponent;
                        for (int i = 0; i < exponent; i++)
                        {
                            sb.Append('0');
                        }
                    }
                }
                else
                {
                    if (_exponent < -_maximumExponentToStringSize)
                    {
                        sb.Append('e');
                        sb.Append(_exponent);
                    }
                    else
                    {
                        int exponent = (int)-_exponent;
                        if (sb.Length <= exponent)
                        {
                            int max = exponent - sb.Length + 2;
                            sb.Insert(0, "0.");
                            for (int i = 2; i < max; i++)
                            {
                                sb.Insert(i, '0');
                            }
                        }
                        else
                        {
                            sb.Insert(sb.Length - exponent, '.');
                        }
                    }
                }
                return sb.ToString();
            }
        }

        #region IEquatable<BigDecimal> Members

        public bool Equals(BigDecimal other)
        {
            return this._coefficient == other._coefficient && this._exponent == other._exponent;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj is BigDecimal)
            {
                return Equals((BigDecimal)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return _coefficient.GetHashCode() ^ _exponent.GetHashCode();
        }

        public static double Log10(BigDecimal number)
        {
            return BigInteger.Log10(number._coefficient) + (double)number._exponent;
        }

        public static BigInteger Log10Floor(BigDecimal number)
        {
            if (number.IsZero)
            {
                throw new DivideByZeroException();
            }
            return (BigInteger)Math.Floor(BigInteger.Log10(number._coefficient)) + number._exponent;
        }

        public static BigInteger Log10Ceiling(BigDecimal number)
        {
            if (number.IsZero)
            {
                throw new DivideByZeroException();
            }
            return (BigInteger)Math.Ceiling(BigInteger.Log10(number._coefficient)) + number._exponent;
        }

        private static readonly double _log10 = Math.Log(10);
        public static double Log(BigDecimal number, double baseValue)
        {
            if (baseValue == 10.0)
            {
                return Log10(number);
            }
            else
            {
                return Log10(number) * baseValue / _log10;
            }
        }

        public static double Log(BigDecimal number)
        {
            return Log(number, Math.E);
        }

        private static readonly BigInteger _longMaxValueAsBigInteger = long.MaxValue;
        public static BigDecimal Pow(BigDecimal number, BigInteger exponent)
        {
            if (number.IsZero)
            {
                return BigDecimal.Zero;
            }
            else if (exponent.IsZero || number.IsOne)
            {
                return BigDecimal.One;
            }
            else if (exponent.IsOne)
            {
                return number;
            }
            else if (exponent.Sign < 0)
            {
                throw new ArgumentOutOfRangeException("exponent", exponent, "Must be at least 0");
            }
            else if (exponent <= _longMaxValueAsBigInteger)
            {
                return Pow(number, (long)exponent);
            }
            else if (number == BigDecimal.MinusOne)
            {
                if (exponent.IsEven)
                {
                    return BigDecimal.One;
                }
                else
                {
                    return BigDecimal.MinusOne;
                }
            }

            BigDecimal result = BigDecimal.One;
            while (!exponent.IsZero)
            {
                if (!exponent.IsEven)
                {
                    result *= number;
                }
                exponent >>= 1;
                number *= number;
            }

            return result;
        }

        public static BigDecimal Pow(BigDecimal number, long exponent)
        {
            if (number.IsZero)
            {
                return BigDecimal.Zero;
            }
            else if (exponent == 0L || number.IsOne)
            {
                return BigDecimal.One;
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
            else if (number == BigDecimal.MinusOne)
            {
                if ((exponent & 1L) == 0)
                {
                    return BigDecimal.One;
                }
                else
                {
                    return BigDecimal.MinusOne;
                }
            }

            BigDecimal result = BigDecimal.One;
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

        #region IComparable<BigDecimal> Members

        public int CompareTo(BigDecimal other)
        {
            int thisSign = this._coefficient.Sign;
            int otherSign = other._coefficient.Sign;
            if (thisSign != otherSign)
            {
                if (thisSign < otherSign)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else if (thisSign == 0)
            {
                return 0;
            }

            if (this._exponent.Equals(other._exponent))
            {
                return _coefficient.CompareTo(other._coefficient);
            }

            BigInteger alpha = this._coefficient;
            BigInteger bravo = other._coefficient;
            int multiplier = 1;
            if (thisSign < 0)
            {
                multiplier = -1;
                alpha = -alpha;
                bravo = -bravo;
            }

            BigInteger alphaLog10 = (BigInteger)Math.Floor(BigInteger.Log10(alpha)) + _exponent;
            BigInteger bravoLog10 = (BigInteger)Math.Floor(BigInteger.Log10(bravo)) + other._exponent;

            if (!alphaLog10.Equals(bravoLog10))
            {
                if (alphaLog10.CompareTo(bravoLog10) > 0)
                {
                    return multiplier;
                }
                else
                {
                    return -multiplier;
                }
            }

            BigInteger exponentDifference = _exponent - other._exponent;
            if (exponentDifference.Sign > 0)
            {
                bravo /= MathUtilities.Pow(BigTen, exponentDifference);
            }
            else
            {
                alpha /= MathUtilities.Pow(BigTen, -exponentDifference);
            }

            int cmp = alpha.CompareTo(bravo);
            switch (alpha.CompareTo(bravo))
            {
                case 1:
                    return multiplier;
                case -1:
                    return -multiplier;
                default:
                    return -multiplier * exponentDifference.Sign;
            }
        }

        #endregion

        public static bool operator ==(BigDecimal alpha, BigDecimal bravo)
        {
            return alpha.Equals(bravo);
        }

        public static bool operator !=(BigDecimal alpha, BigDecimal bravo)
        {
            return !alpha.Equals(bravo);
        }

        public static bool operator >(BigDecimal alpha, BigDecimal bravo)
        {
            return alpha.CompareTo(bravo) > 0;
        }

        public static bool operator <(BigDecimal alpha, BigDecimal bravo)
        {
            return alpha.CompareTo(bravo) < 0;
        }

        public static bool operator >=(BigDecimal alpha, BigDecimal bravo)
        {
            return alpha.CompareTo(bravo) >= 0;
        }

        public static bool operator <=(BigDecimal alpha, BigDecimal bravo)
        {
            return alpha.CompareTo(bravo) <= 0;
        }

        public static implicit operator BigDecimal(int value)
        {
            return new BigDecimal(value);
        }

        public static implicit operator BigDecimal(long value)
        {
            return new BigDecimal(value);
        }

        public static implicit operator BigDecimal(BigInteger value)
        {
            return new BigDecimal(value);
        }

        public static implicit operator BigDecimal(decimal value)
        {
            return new BigDecimal(value);
        }

        public static explicit operator BigDecimal(double value)
        {
            return (BigDecimal)((decimal)value);
        }

        public static explicit operator double(BigDecimal value)
        {
            return Math.Pow(10.0, BigDecimal.Log10(value));
        }

        public static explicit operator BigInteger(BigDecimal value)
        {
            BigDecimal remainder;
            return DivRem(value, One, out remainder);
        }

        public static BigDecimal operator +(BigDecimal value)
        {
            return value;
        }

        public static BigDecimal operator -(BigDecimal value)
        {
            return new BigDecimal(-value._coefficient, value._exponent);
        }

        public static BigDecimal operator +(BigDecimal alpha, BigDecimal bravo)
        {
            BigInteger exponentDifference = alpha._exponent - bravo._exponent;

            BigInteger alphaCoefficient;
            BigInteger bravoCoefficient;
            BigInteger resultantExponent;
            switch (exponentDifference.Sign)
            {
                case 1:
                    alphaCoefficient = alpha._coefficient * MathUtilities.Pow(BigTen, exponentDifference);
                    bravoCoefficient = bravo._coefficient;
                    resultantExponent = bravo._exponent;
                    break;
                case -1:
                    alphaCoefficient = alpha._coefficient;
                    bravoCoefficient = bravo._coefficient * MathUtilities.Pow(BigTen, -exponentDifference);
                    resultantExponent = alpha._exponent;
                    break;
                default:
                    alphaCoefficient = alpha._coefficient;
                    bravoCoefficient = bravo._coefficient;
                    resultantExponent = alpha._exponent;
                    break;
            }

            return new BigDecimal(alphaCoefficient + bravoCoefficient, resultantExponent);
        }

        public static BigDecimal operator -(BigDecimal alpha, BigDecimal bravo)
        {
            return alpha + (-bravo);
        }

        public static BigDecimal operator *(BigDecimal alpha, BigDecimal bravo)
        {
            if (alpha.IsZero || bravo.IsZero)
            {
                return BigDecimal.Zero;
            }
            else if (alpha.IsOne)
            {
                return bravo;
            }
            else if (bravo.IsOne)
            {
                return alpha;
            }
            else
            {
                return new BigDecimal(alpha._coefficient * bravo._coefficient, alpha._exponent + bravo._exponent);
            }
        }

        public static BigInteger DivRem(BigDecimal dividend, BigDecimal divisor, out BigDecimal remainder)
        {
            if (divisor.IsZero)
            {
                throw new DivideByZeroException();
            }
            else if (dividend.IsZero)
            {
                remainder = BigDecimal.Zero;
                return BigInteger.Zero;
            }
            else if (divisor.Equals(dividend))
            {
                remainder = BigDecimal.Zero;
                return BigInteger.One;
            }
            else
            {
                BigInteger dividendCoefficient = dividend._coefficient;
                BigInteger divisorCoefficient = divisor._coefficient;
                bool negative = false;
                if (dividendCoefficient.Sign < 0)
                {
                    dividendCoefficient = -dividendCoefficient;
                    negative = !negative;
                }
                if (divisorCoefficient.Sign < 0)
                {
                    divisorCoefficient = -divisorCoefficient;
                    negative = !negative;
                }
                BigInteger resultantExponent;
                BigInteger exponentDifference = dividend._exponent - divisor._exponent;
                switch (exponentDifference.Sign)
                {
                    case 1:
                        dividendCoefficient *= MathUtilities.Pow(BigTen, exponentDifference);
                        resultantExponent = divisor._exponent;
                        break;
                    case -1:
                        divisorCoefficient *= MathUtilities.Pow(BigTen, -exponentDifference);
                        resultantExponent = dividend._exponent;
                        break;
                    default:
                        resultantExponent = dividend._exponent;
                        break;
                }

                BigInteger mod;
                BigInteger quotient = BigInteger.DivRem(dividendCoefficient, divisorCoefficient, out mod);
                remainder = new BigDecimal(mod, resultantExponent);
                if (negative)
                {
                    quotient = -quotient;
                }
                return quotient;
            }
        }

        private static readonly BigInteger _divisionLimit = 1500;
        private const int _digitsPerModulo = 20;
        private static readonly BigInteger _divisionTenMultiplier = MathUtilities.Pow(BigTen, _digitsPerModulo);
        public static BigDecimal operator /(BigDecimal dividend, BigDecimal divisor)
        {
            if (divisor.IsZero)
            {
                throw new DivideByZeroException();
            }
            else if (divisor.IsOne)
            {
                return dividend;
            }
            else if (divisor.Equals(dividend))
            {
                return BigDecimal.One;
            }
            else if (divisor._coefficient == dividend._coefficient)
            {
                return new BigDecimal(BigInteger.One, dividend._exponent - divisor._exponent);
            }
            else if (divisor._coefficient.IsOne)
            {
                return new BigDecimal(dividend._coefficient, dividend._exponent - divisor._exponent);
            }
            else
            {
                BigInteger dividendCoefficient = dividend._coefficient;
                BigInteger divisorCoefficient = divisor._coefficient;
                bool negative = false;
                if (dividendCoefficient.Sign < 0)
                {
                    dividendCoefficient = -dividendCoefficient;
                    negative = !negative;
                }
                if (divisorCoefficient.Sign < 0)
                {
                    divisorCoefficient = -divisorCoefficient;
                    negative = !negative;
                }
                BigInteger exponentDifference = dividend._exponent - divisor._exponent;
                switch (exponentDifference.Sign)
                {
                    case 1:
                        dividendCoefficient *= MathUtilities.Pow(BigTen, exponentDifference);
                        break;
                    case -1:
                        divisorCoefficient *= MathUtilities.Pow(BigTen, -exponentDifference);
                        break;
                }

                BigInteger mod;
                BigInteger quotient = BigInteger.DivRem(dividendCoefficient, divisorCoefficient, out mod);

                BigInteger exponent = BigInteger.Zero;

                BigInteger currentResult = quotient;
                while (!mod.IsZero && exponent < _divisionLimit)
                {
                    BigInteger addition = BigInteger.DivRem(mod * _divisionTenMultiplier, divisor._coefficient, out mod);
                    exponent += _digitsPerModulo;
                    currentResult = currentResult * _divisionTenMultiplier + addition;
                }
                if (exponent >= _divisionLimit)
                {
                    BigInteger addition = BigInteger.DivRem(mod * BigTen, divisor._coefficient, out mod);
                    if (addition >= 5)
                    {
                        currentResult++;
                    }
                }

                if (negative)
                {
                    currentResult = -currentResult;
                }

                return new BigDecimal(currentResult, -exponent);
            }
        }

        public static BigDecimal operator %(BigDecimal dividend, BigDecimal divisor)
        {
            BigDecimal remainder;
            DivRem(dividend, divisor, out remainder);
            return remainder;
        }

        #region ISerializable Members

        private BigDecimal(SerializationInfo info, StreamingContext context)
        {
            BigInteger coefficient = (BigInteger)info.GetValue("c", typeof(BigInteger));
            BigInteger exponent = (BigInteger)info.GetValue("e", typeof(BigInteger));

            Reduce(ref coefficient, ref exponent);

            _coefficient = coefficient;
            _exponent = exponent;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("c", _coefficient, typeof(BigInteger));
            info.AddValue("e", _exponent, typeof(BigInteger));
        }

        #endregion
    }
}
