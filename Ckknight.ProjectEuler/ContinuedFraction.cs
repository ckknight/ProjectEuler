using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler
{
    public class ContinuedFraction
    {
        private ContinuedFraction(long floor, IEnumerable<long> nonPeriodicQuotients, IEnumerable<long> periodicQuotients)
        {
            _floor = floor;
            _nonPeriodicQuotients = nonPeriodicQuotients ?? Enumerable.Empty<long>();
            _periodicQuotients = periodicQuotients ?? Enumerable.Empty<long>();
        }
        
        public ContinuedFraction(long floor)
            : this(floor, default(IEnumerable<long>), default(IEnumerable<long>)) { }

        public ContinuedFraction(long floor, IEnumerable<long> nonPeriodicQuotients)
            : this(floor, nonPeriodicQuotients, default(IEnumerable<long>))
        {
            if (nonPeriodicQuotients == null)
            {
                throw new ArgumentNullException("nonPeriodicQuotients");
            }
        }

        public ContinuedFraction(long floor, IEnumerable<long> nonPeriodicQuotients, long[] periodicQuotients)
            : this(floor, nonPeriodicQuotients, (IEnumerable<long>)periodicQuotients)
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

        public static readonly ContinuedFraction Zero = new ContinuedFraction(0);
        public static readonly ContinuedFraction One = new ContinuedFraction(1);

        private readonly long _floor;
        public long Floor
        {
            get
            {
                return _floor;
            }
        }

        private readonly IEnumerable<long> _nonPeriodicQuotients;
        public IEnumerable<long> NonPeriodicQuotients
        {
            get
            {
                return _nonPeriodicQuotients;
            }
        }

        private readonly IEnumerable<long> _periodicQuotients;
        public IEnumerable<long> PeriodicQuotients
        {
            get
            {
                return _periodicQuotients;
            }
        }

        public IEnumerable<long> Quotients
        {
            get
            {
                foreach (long quotient in _nonPeriodicQuotients)
                {
                    yield return quotient;
                }

                while (true)
                {
                    bool found = false;
                    foreach (long quotient in _periodicQuotients)
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

            long[] partialNonPeriodicQuotients = _nonPeriodicQuotients as long[];
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

            long[] periodicQuotients = _periodicQuotients as long[] ?? _periodicQuotients.ToArray();
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

        public static ContinuedFraction Sqrt(long value)
        {
            long sqrt = (long)Math.Floor(Math.Sqrt(value));
            if (sqrt * sqrt == value)
            {
                return new ContinuedFraction(sqrt);
            }
            else
            {
                var period = CollectionUtilities.Repeat(default(object))
                    .SelectWithAggregate(new { m = 0L, d = 1L, a = sqrt }, (x, i) =>
                    {
                        long m = x.d * x.a - x.m;
                        long d = (value - m * m) / x.d;
                        long a = (sqrt + m) / d;
                        return new { m, d, a };
                    })
                    .TakeWhileDistinct()
                    .Select(x => x.a)
                    .ToArray();

                return new ContinuedFraction(sqrt, Enumerable.Empty<long>(), period);
            }
        }

        public static ContinuedFraction FromFraction(Fraction fraction)
        {
            if (fraction.Denominator == 0)
            {
                throw new ArithmeticException("Cannot generate continued fraction from NaN, Infinity, or -Infinity");
            }

            bool first = true;
            long initial = 0;
            List<long> quotients = new List<long>();
            while (true)
            {
                Fraction remainder;
                long floor = Fraction.DivRem(fraction, Fraction.One, out remainder).Numerator;

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
                    return new ContinuedFraction(initial, quotients.ToArray());
                }

                fraction = Fraction.Reciprocal(remainder);
            }
        }

        public IEnumerable<Fraction> GetFractions()
        {
            return this.Quotients
                .PrependItem(this.Floor)
                .SelectWithAggregate(new { Alpha = Fraction.Zero, Bravo = Fraction.PositiveInfinity }, (x, v) =>
                    new
                    {
                        Alpha = x.Bravo,
                        Bravo = new Fraction(
                            v * x.Bravo.Numerator + x.Alpha.Numerator,
                            v * x.Bravo.Denominator + x.Alpha.Denominator)
                    })
                .Select(x => x.Bravo);
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
