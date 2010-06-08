using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ckknight.ProjectEuler.Collections
{
    /// <summary>
    /// Represents an object which generates the prime factors of a given number.
    /// </summary>
    public class PrimeFactorGenerator : IEnumerable<long>
    {
        /// <summary>
        /// Initializes a PrimeFactorGenerator.
        /// </summary>
        /// <param name="number">The number to calculate the prime factors for.</param>
        public PrimeFactorGenerator(long number)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException("number", number, "Must be at least 0");
            }

            _number = number;
        }

        private readonly long _number;

        #region IEnumerable<long> Members

        /// <summary>
        /// Return an enumerator which iterates over the prime factors of the current number.
        /// </summary>
        /// <returns>The prime factor enumerator.</returns>
        public IEnumerator<long> GetEnumerator()
        {
            if (_number <= 1)
            {
                yield break;
            }

            long remainingNumber = _number;
            long midpoint = (long)(Math.Sqrt(_number));
            foreach (long prime in PrimeGenerator.Instance)
            {
                if (prime > midpoint)
                {
                    break;
                }

                while ((remainingNumber % prime) == 0)
                {
                    yield return prime;
                    remainingNumber /= prime;
                    midpoint = (long)(Math.Sqrt(remainingNumber));
                }
            }

            if (remainingNumber > 1)
            {
                yield return remainingNumber;
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Return an enumerator which iterates over the prime factors of the current number.
        /// </summary>
        /// <returns>The prime factor enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
