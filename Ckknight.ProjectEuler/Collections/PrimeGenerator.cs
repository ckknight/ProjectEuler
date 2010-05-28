using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ckknight.ProjectEuler.Collections
{
    /// <summary>
    /// Represents an enumerable which generates primes.
    /// </summary>
    public sealed class PrimeGenerator : IEnumerable<long>
    {
        private static readonly List<long> _foundPrimes = new List<long>
        {
            2,
            3,
            5,
            7,
            11,
            13,
            17,
            19,
            23,
            29,
        };
        private static readonly HashSet<long> _primeSet = new HashSet<long>(_foundPrimes);
        private static int _calculatedPrimesUpTo = 30;

        /// <summary>
        /// Return whether a given value is prime.
        /// </summary>
        /// <param name="value">The value to check for primeness.</param>
        /// <returns>Whether the value is prime.</returns>
        public static bool IsPrime(long value)
        {
            if (_primeSet.Contains(value))
            {
                return true;
            }
            else if (value <= _calculatedPrimesUpTo)
            {
                return false;
            }

            long sqrt = (long)Math.Sqrt(value);
            foreach (long i in new PrimeGenerator().TakeWhile(x => x <= sqrt))
            {
                if ((value % i) == 0L)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get the prime at the 0-based index of all primes.
        /// </summary>
        /// <param name="index">The 0-based index to get the prime of.</param>
        /// <returns>The prime at the given index.</returns>
        public static long GetPrimeAtIndex(int index)
        {
            while (_foundPrimes.Count <= index)
            {
                GetNextSection();
            }

            return _foundPrimes[index];
        }

        public static void CalculateUpTo(int maximum)
        {
            while (_calculatedPrimesUpTo < maximum)
            {
                GetNextSection();
            }
        }

        private static readonly int[] _numbersToCheck = new Range(30).ToArray();

        private static IEnumerable<long> GetNextSection()
        {
            List<long> tmp = new List<long>();

            while (tmp.Count == 0)
            {
                for (int i = 0; i < _numbersToCheck.Length; i++)
                {
                    long value = _calculatedPrimesUpTo + _numbersToCheck[i];
                    bool isComposite = false;
                    long midpoint = (long)Math.Sqrt(value);
                    foreach (long prime in _foundPrimes)
                    {
                        if (prime > midpoint)
                        {
                            break;
                        }
                        if ((value % prime) == 0)
                        {
                            isComposite = true;
                            break;
                        }
                    }
                    if (!isComposite)
                    {
                        _foundPrimes.Add(value);
                        _primeSet.Add(value);
                        tmp.Add(value);
                    }
                }
                _calculatedPrimesUpTo += _numbersToCheck.Length;
            }

            return tmp;
        }

        #region IEnumerable<long> Members

        /// <summary>
        /// Return an enumerator which iterates over all primes.
        /// </summary>
        /// <returns>The enumerator of primes.</returns>
        public IEnumerator<long> GetEnumerator()
        {
            // we need to yield all the primes we have previously found.
            for (int i = 0; i < _foundPrimes.Count; i++)
            {
                long prime = _foundPrimes[i];
                yield return prime;
            }

            // then we repeatedly get the primes of the next section and yield those values.
            while (true)
            {
                foreach (long value in GetNextSection())
                {
                    yield return value;
                }
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Return an enumerator which iterates over all primes.
        /// </summary>
        /// <returns>The enumerator of primes.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
