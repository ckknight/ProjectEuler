using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Ckknight.ProjectEuler.Collections
{
    public class BigContinuedFraction
    {
        private BigContinuedFraction(BigInteger floor, IEnumerable<BigInteger> nonPeriodicQuotients, IEnumerable<BigInteger> periodicQuotients)
        {
            _floor = floor;
            _nonPeriodicQuotients = nonPeriodicQuotients ?? Enumerable.Empty<BigInteger>();
            _periodicQuotients = periodicQuotients ?? Enumerable.Empty<BigInteger>();
        }

        public BigContinuedFraction(BigInteger floor)
            : this(floor, default(IEnumerable<BigInteger>), default(IEnumerable<BigInteger>)) { }

        public BigContinuedFraction(BigInteger floor, IEnumerable<BigInteger> nonPeriodicQuotients)
            : this(floor, nonPeriodicQuotients, default(IEnumerable<BigInteger>))
        {
            if (nonPeriodicQuotients == null)
            {
                throw new ArgumentNullException("nonPeriodicQuotients");
            }
        }

        public BigContinuedFraction(BigInteger floor, IEnumerable<BigInteger> nonPeriodicQuotients, BigInteger[] periodicQuotients)
            : this(floor, nonPeriodicQuotients, (IEnumerable<BigInteger>)periodicQuotients)
        {
            if (nonPeriodicQuotients == null)
            {
                throw new ArgumentNullException("nonPeriodicQuotients");
            }
            else if (periodicQuotients == null)
            {
                throw new ArgumentNullException("periodicQuotients");
            }
        }

        public static readonly BigContinuedFraction Zero = new BigContinuedFraction(0);
        public static readonly BigContinuedFraction One = new BigContinuedFraction(1);

        private readonly BigInteger _floor;
        public BigInteger Floor
        {
            get
            {
                return _floor;
            }
        }

        private readonly IEnumerable<BigInteger> _nonPeriodicQuotients;
        public IEnumerable<BigInteger> NonPeriodicQuotients
        {
            get
            {
                return _nonPeriodicQuotients;
            }
        }

        private readonly IEnumerable<BigInteger> _periodicQuotients;
        public IEnumerable<BigInteger> PeriodicQuotients
        {
            get
            {
                return _periodicQuotients;
            }
        }

        public IEnumerable<BigInteger> Quotients
        {
            get
            {
                foreach (BigInteger quotient in _nonPeriodicQuotients)
                {
                    yield return quotient;
                }

                while (true)
                {
                    bool found = false;
                    foreach (BigInteger quotient in _periodicQuotients)
                    {
                        if (!found)
                        {
                            found = true;
                        }
                        yield return quotient;
                    }
                    if (!found)
                    {
                        break;
                    }
                }
            }
        }

        private const int MaxNonPeriodicLengthShown = 10;
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            sb.Append(_floor);

            BigInteger[] partialNonPeriodicQuotients = _nonPeriodicQuotients as BigInteger[];
            if (partialNonPeriodicQuotients == null)
            {
                partialNonPeriodicQuotients = _nonPeriodicQuotients.Take(MaxNonPeriodicLengthShown + 1).ToArray();
            }
            int length = partialNonPeriodicQuotients.Length;
            if (length > MaxNonPeriodicLengthShown)
            {
                length = MaxNonPeriodicLengthShown;
            }
            for (int i = 0; i < length; i++)
            {
                if (i == 0)
                {
                    sb.Append("; ");
                }
                else
                {
                    sb.Append(", ");
                }
                sb.Append(partialNonPeriodicQuotients[i]);
            }
            if (partialNonPeriodicQuotients.Length > MaxNonPeriodicLengthShown)
            {
                sb.Append(", ...");
            }

            BigInteger[] periodicQuotients = _periodicQuotients as BigInteger[] ?? _periodicQuotients.ToArray();
            if (periodicQuotients.Length > 0)
            {
                if (length == 0)
                {
                    sb.Append("; ");
                }
                else
                {
                    sb.Append(", ");
                }
                sb.Append('(');
                for (int i = 0; i < periodicQuotients.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(periodicQuotients[i]);
                }
                sb.Append(')');
            }

            sb.Append(']');
            return sb.ToString();
        }

        public static BigContinuedFraction Sqrt(BigInteger value)
        {
            BigInteger sqrt = MathUtilities.BigSqrt(value);

            if (sqrt * sqrt == value)
            {
                return new BigContinuedFraction(sqrt);
            }
            else
            {
                var period = CollectionUtilities.Repeat(default(object))
                    .SelectWithAggregate(new { m = BigInteger.Zero, d = BigInteger.One, a = sqrt }, (x, i) =>
                    {
                        BigInteger m = x.d * x.a - x.m;
                        BigInteger d = (value - m * m) / x.d;
                        BigInteger a = (sqrt + m) / d;
                        return new { m, d, a };
                    })
                    .TakeWhileDistinct()
                    .Select(x => x.a)
                    .ToArray();

                return new BigContinuedFraction(sqrt, Enumerable.Empty<BigInteger>(), period);
            }
        }

        public static BigContinuedFraction FromBigFraction(BigFraction fraction)
        {
            if (fraction.Denominator.IsZero)
            {
                throw new ArithmeticException("Cannot generate continued fraction from NaN, Infinity, or -Infinity");
            }

            bool first = true;
            BigInteger initial = BigInteger.Zero;
            List<BigInteger> quotients = new List<BigInteger>();
            while (true)
            {
                BigFraction remainder;
                BigInteger floor = BigFraction.DivRem(fraction, BigFraction.One, out remainder).Numerator;

                if (first)
                {
                    first = false;
                    initial = floor;
                }
                else
                {
                    quotients.Add(floor);
                }

                if (remainder.IsZero)
                {
                    return new BigContinuedFraction(initial, quotients.ToArray());
                }

                fraction = BigFraction.Reciprocal(remainder);
            }
        }

        public IEnumerable<BigFraction> GetBigFractions()
        {
            return this.Quotients
                .PrependItem(this.Floor)
                .SelectWithAggregate(new { Alpha = BigFraction.Zero, Bravo = BigFraction.PositiveInfinity }, (x, v) =>
                    new
                    {
                        Alpha = x.Bravo,
                        Bravo = new BigFraction(
                            v * x.Bravo.Numerator + x.Alpha.Numerator,
                            v * x.Bravo.Denominator + x.Alpha.Denominator)
                    })
                .Select(x => x.Bravo);
        }
    }
}
