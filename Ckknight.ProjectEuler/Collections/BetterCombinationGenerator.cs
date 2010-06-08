using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ckknight.ProjectEuler.Collections
{
    public class BetterCombinationGenerator<T> : IEnumerable<T[]>
    {
        /// <summary>
        /// Initialize a new CombinationGenerator based on the conditions.
        /// </summary>
        /// <param name="source">The source enumerable to pull data from.</param>
        /// <param name="amount">The size of each combination.</param>
        public BetterCombinationGenerator(IEnumerable<T> source, int amount)
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
        
        /// <summary>
        /// Return an enumerator which iterates over all combinations.
        /// </summary>
        /// <returns>The combination enumerator.</returns>
        public IEnumerator<T[]> GetEnumerator()
        {
            if (_amount == 0)
            {
                return Enumerable.Empty<T[]>()
                    .GetEnumerator();
            }
            else if (_amount == 1)
            {
                return _source
                    .Select(x => new[] { x })
                    .GetEnumerator();
            }
            else
            {
                T[] array = _source as T[] ?? _source.ToArray();

                int length = array.Length;
                if (_amount > length)
                {
                    return Enumerable.Empty<T[]>()
                        .GetEnumerator();
                }
                
                return new Range(_amount)
                    .Aggregate(new List<ImmutableSequence<int>> { ImmutableSequence<int>.Empty }.AsEnumerable(),
                    (x, i) => x
                        .SelectMany(seq => new Range(seq.HasValue ? seq.First() + 1 : 0, length - _amount + i, true)
                            .Select(n => new ImmutableSequence<int>(n, seq))))
                    .Select(s => s
                        .Reverse()
                        .Select(i => array[i])
                        .ToArray())
                    .GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
