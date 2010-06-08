using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Ckknight.ProjectEuler.Collections
{
    public class PrimeGenerator : IEnumerable<long>, IEnumerable
    {
        private PrimeGenerator()
        {
        }

        private static readonly PrimeGenerator _instance = new PrimeGenerator();
        public static PrimeGenerator Instance
        {
            get
            {
                return _instance;
            }
        }

        private long _calculatedUpTo = 1;
        private const int StandardPrimeCalculationChunkSize = 10000;
        private const int MaxPrimeCalculationChunkSize = 8000000;
        private readonly List<long> _primes = new List<long>();
        private readonly HashSet<long> _primeSet = new HashSet<long>();

        public void CalculateUpTo(long value)
        {
            if (value % StandardPrimeCalculationChunkSize != 0)
            {
                value += StandardPrimeCalculationChunkSize - (value % StandardPrimeCalculationChunkSize);
            }
            long start = _calculatedUpTo;

            for (long i = _calculatedUpTo + 1; i < value; i += MaxPrimeCalculationChunkSize)
            {
                long max = i + MaxPrimeCalculationChunkSize;
                if (max > value)
                {
                    max = value;
                }
                CalculatePrimes(i, (int)(max - i));
            }
        }

        public void CalculateNextChuck()
        {
            CalculatePrimes(_calculatedUpTo + 1, StandardPrimeCalculationChunkSize);
        }

        private readonly object _syncRoot = new object();
        private void CalculatePrimes(long start, int length)
        {
            long max = start + length - 1;
            if (max > _calculatedUpTo)
            {
                lock (_syncRoot)
                {
                    if (max > _calculatedUpTo)
                    {
                        BooleanArray data = new BooleanArray(length);

                        long maxValue = (long)Math.Ceiling(Math.Sqrt(length + start));

                        for (int i = 0; i < _primes.Count; i++)
                        {
                            long value = _primes[i];
                            if (value > maxValue)
                            {
                                break;
                            }

                            long iterStart = (value * value) - start;
                            if (iterStart < 0)
                            {
                                iterStart -= value * (iterStart / value);
                                if (start % value != 0)
                                {
                                    iterStart += value;
                                }
                            }
                            for (long j = iterStart; j < length; j += value)
                            {
                                data[(int)j] = true;
                            }
                        }

                        for (int i = 0; i < length; i++)
                        {
                            if (!data[i])
                            {
                                long value = start + i;
                                _primes.Add(value);
                                _primeSet.Add(value);

                                if (value <= maxValue)
                                {
                                    for (long j = ((value * value) - start); j < length; j += value)
                                    {
                                        data[(int)j] = true;
                                    }
                                }
                            }
                        }
                        _calculatedUpTo = max;
                    }
                }
            }
        }

        public IEnumerator<long> GetEnumerator()
        {
            int index = 0;

            while (true)
            {
                long count;
                do
                {
                    count = _primes.Count;
                    for (; index < count; index++)
                    {
                        long prime = _primes[index];
                        yield return prime;
                    }
                } while (count < _primes.Count);

                CalculateNextChuck();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Get the prime at the 0-based index of all primes.
        /// </summary>
        /// <param name="index">The 0-based index to get the prime of.</param>
        /// <returns>The prime at the given index.</returns>
        public long GetPrimeAtIndex(int index)
        {
            if (_primes.Count > index)
            {
                return _primes[index];
            }

            if (index > 6)
            {
                double log = Math.Log(index);
                long approximatePrimeValue = (long)Math.Floor(index * log + index * Math.Log(log));
                CalculateUpTo(approximatePrimeValue);
            }

            while (_primes.Count <= index)
            {
                CalculateNextChuck();
            }

            return _primes[index];
        }

        /// <summary>
        /// Return whether a given value is prime.
        /// </summary>
        /// <param name="value">The value to check for primeness.</param>
        /// <returns>Whether the value is prime.</returns>
        public bool IsPrime(long value)
        {
            if (_primeSet.Contains(value))
            {
                return true;
            }
            else if (value <= _calculatedUpTo)
            {
                return false;
            }

            long sqrt = (long)Math.Sqrt(value);
            CalculateUpTo(sqrt);

            int count = _primes.Count;
            for (int i = 0; i < count; i++)
            {
                long prime = _primes[i];
                if (prime > sqrt)
                {
                    break;
                }
                else if ((value % prime) == 0L)
                {
                    return false;
                }
            }
            _primeSet.Add(value);
            return true;
        }

        public IEnumerable<long> GetUpTo(long amount)
        {
            CalculateUpTo(amount);

            return this
                .TakeWhile(n => n <= amount);
        }

        public ParallelQuery<long> AsParallel(long upToAmount)
        {
            return Partitioner.Create<long>(GetUpTo(upToAmount))
                .AsParallel();
        }

        public IEnumerable<long> GetComposites()
        {
            long value = 2;
            foreach (long prime in this)
            {
                while (value < prime)
                {
                    yield return value++;
                }
                value = prime + 1;
            }
        }

        public IEnumerable<long> GetCompositesUpTo(long amount)
        {
            long value = 2;
            foreach (long prime in GetUpTo(amount))
            {
                while (value < prime)
                {
                    yield return value++;
                }
                value = prime + 1;
            }
        }

        public IEnumerable<Tuple<long, long>> GetSemiprimesUpTo(long amount, bool includePerfectSquares)
        {
            CalculateUpTo(amount / 2);

            long sqrt = (long)Math.Floor(Math.Sqrt(amount));

            int skipAmount = includePerfectSquares ? 0 : 1;

            return GetUpTo(sqrt)
                .SelectMany((p, i) => GetUpTo(amount / p)
                    .Skip(skipAmount + i)
                    .Select(q => new Tuple<long, long>(p, q)));
        }

        public ParallelQuery<Tuple<long, long>> GetParallelSemiprimesUpTo(long amount, bool includePerfectSquares)
        {
            CalculateUpTo(amount / 2);

            long sqrt = (long)Math.Floor(Math.Sqrt(amount));

            int skipAmount = includePerfectSquares ? 0 : 1;

            return AsParallel(sqrt)
                .SelectMany((p, i) => GetUpTo(amount / p)
                    .Skip(skipAmount + i)
                    .Select(q => new Tuple<long, long>(p, q)));
        }
    }
}
