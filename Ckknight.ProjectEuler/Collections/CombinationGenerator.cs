using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ckknight.ProjectEuler.Collections
{
    /// <summary>
    /// Represents an object which generates combinations based on a given
    /// enumerable and the size of the combinations to generate.
    /// 
    /// The generated combinations will be in the same order as passed in, and
    /// will have n!/(k!(n - k)!) results, where n and k are the source length
    /// and the combination length.
    /// </summary>
    /// <typeparam name="T">The element type of the sequence</typeparam>
    public sealed class CombinationGenerator<T> : IEnumerable<T[]>
    {
        /// <summary>
        /// Initialize a new CombinationGenerator based on the conditions.
        /// </summary>
        /// <param name="source">The source enumerable to pull data from.</param>
        /// <param name="amount">The size of each combination.</param>
        public CombinationGenerator(IEnumerable<T> source, int amount)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            else if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount", amount, "Must be at least 0");
            }

            _source = source;
            _amount = amount;
        }

        private readonly IEnumerable<T> _source;
        private readonly int _amount;

        private static bool UpdatePositions(int[] positions, int sourceLength, int index)
        {
            int amount = positions.Length;
            int position = positions[index];
            if (position >= sourceLength + index - amount)
            {
                if (index == 0)
                {
                    return false;
                }
                if (!UpdatePositions(positions, sourceLength, index - 1))
                {
                    return false;
                }
                positions[index] = positions[index - 1] + 1;
            }
            else
            {
                positions[index]++;
            }
            return true;
        }

        #region IEnumerable<T[]> Members

        /// <summary>
        /// Return an enumerator which iterates over all combinations.
        /// </summary>
        /// <returns>The combination enumerator.</returns>
        public IEnumerator<T[]> GetEnumerator()
        {
            if (_amount == 0)
            {
                yield break;
            }

            T[] array = _source as T[] ?? _source.ToArray();

            int length = array.Length;
            if (_amount > length)
            {
                yield break;
            }

            int[] positions = new int[_amount];
            for (int i = 0; i < _amount - 1; i++)
            {
                positions[i] = i;
            }
            positions[_amount - 1] = _amount - 2;

            while (UpdatePositions(positions, length, _amount - 1))
            {
                T[] result = new T[_amount];
                for (int i = 0; i < _amount; i++)
                {
                    result[i] = array[positions[i]];
                }
                yield return result;
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Return an enumerator which iterates over all combinations.
        /// </summary>
        /// <returns>The combination enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
