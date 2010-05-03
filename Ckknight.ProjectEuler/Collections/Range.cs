using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ckknight.ProjectEuler.Collections
{
    /// <summary>
    /// Represents an finite, immutable range of Int32s that may have a non-1 step and may or may not be inclusive.
    /// </summary>
    public sealed class Range : ICollection<int>, IEnumerable<int>, IEnumerable
    {
        /// <summary>
        /// Initializes a new Range that starts at 0 and ends at <paramref name="length"/> exclusively.
        /// </summary>
        /// <param name="length">The length of the range.</param>
        public Range(int length)
            : this(0, length, 1, false) { }

        /// <summary>
        /// Initializes a new Range that starts at 0 and ends at <paramref name="length"/>.
        /// </summary>
        /// <param name="length">The length of the range.</param>
        /// <param name="isInclusive">Whether the range includes the last value.</param>
        public Range(int length, bool isInclusive)
            : this(0, length, 1, isInclusive) { }

        /// <summary>
        /// Initializes a new Range that starts at <paramref name="start"/> and ends at <paramref name="finish"/> exclusively.
        /// </summary>
        /// <param name="start">The starting value of the range.</param>
        /// <param name="finish">The ending value of the range.</param>
        public Range(int start, int finish)
            : this(start, finish, 1, false) { }

        /// <summary>
        /// Initializes a new Range that starts at <paramref name="start"/> and ends at <paramref name="finish"/>.
        /// </summary>
        /// <param name="start">The starting value of the range.</param>
        /// <param name="finish">The ending value of the range.</param>
        /// <param name="isInclusive">Whether the range includes the last value.</param>
        public Range(int start, int finish, bool isInclusive)
            : this(start, finish, 1, isInclusive) { }

        /// <summary>
        /// Initializes a new Range that starts at <paramref name="start"/>, ends at <paramref name="finish"/> exclusively, and has a step of <paramref name="step"/>.
        /// </summary>
        /// <param name="start">The starting value of the range.</param>
        /// <param name="finish">The ending value of the range.</param>
        /// <param name="step">How much to step by for each iteration.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="step"/> is 0.</exception>
        public Range(int start, int finish, int step)
            : this(start, finish, step, false) { }

        /// <summary>
        /// Initializes a new Range that starts at <paramref name="start"/>, ends at <paramref name="finish"/>, and has a step of <paramref name="step"/>.
        /// </summary>
        /// <param name="start">The starting value of the range.</param>
        /// <param name="finish">The ending value of the range.</param>
        /// <param name="step">How much to step by for each iteration.</param>
        /// <param name="isInclusive">Whether the range includes the last value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="step"/> is 0.</exception>
        public Range(int start, int finish, int step, bool isInclusive)
        {
            if (step == 0)
            {
                throw new ArgumentOutOfRangeException("step", step, "Cannot be zero");
            }

            _start = start;
            _finish = finish;
            _step = step;
            _isExclusive = !isInclusive;
        }

        private readonly int _start;
        private readonly int _finish;
        private readonly int _step;
        private readonly bool _isExclusive;

        private static readonly string EmptyRangeString = "Range {/}";
        /// <summary>
        /// Return a string representation of the Range.
        /// </summary>
        /// <returns>A string representation of the current Range.</returns>
        public override string ToString()
        {
            if (_finish == _start && !_isExclusive)
            {
                return EmptyRangeString;
            }
            else if (_step > 0 ? (_finish < _start) : (_start < _finish))
            {
                return EmptyRangeString;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("Range ");
            sb.Append('[');
            sb.Append(_start);
            sb.Append(", ");
            sb.Append(_finish);
            if (_step != 1)
            {
                sb.Append(" :: ");
                sb.Append(_step);
            }
            sb.Append(_isExclusive ? ')' : ']');

            return sb.ToString();
        }

        /// <summary>
        /// Enables parallelization of the range.
        /// </summary>
        /// <returns>A parallel version of the elements of this range.</returns>
        public ParallelQuery<int> AsParallel()
        {
            int length = Count;

            if (length <= 0)
            {
                return ParallelEnumerable.Empty<int>();
            }

            if (_step == 1)
            {
                return ParallelEnumerable.Range(_start, length);
            }

            ParallelQuery<int> sequence = ParallelEnumerable.Range(0, length)
                .Select(x => x * _step);
            if (_start != 0)
            {
                sequence = sequence.Select(x => x + _start);
            }
            return sequence;
        }

        #region IEnumerable<int> Members

        /// <summary>
        /// Returns an enumerator that iterates through the Range.
        /// </summary>
        /// <returns>An enumerator for the Range.</returns>
        public IEnumerator<int> GetEnumerator()
        {
            int length = Count;

            if (length <= 0)
            {
                return Enumerable.Empty<int>().GetEnumerator();
            }

            if (_step == 1)
            {
                return Enumerable.Range(_start, length).GetEnumerator();
            }

            IEnumerable<int> sequence = Enumerable.Range(0, length)
                .Select(x => x * _step);
            if (_start != 0)
            {
                sequence = sequence.Select(x => x + _start);
            }
            return sequence.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through the Range.
        /// </summary>
        /// <returns>An enumerator for the Range.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region ICollection<int> Members

        /// <summary>
        /// Adding to a range is not supported.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <exception cref="System.NotSupportedException">Range is immutable.</exception>
        void ICollection<int>.Add(int item)
        {
            throw new NotSupportedException("Range is immutable");
        }

        /// <summary>
        /// Clearing the range is not supported.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Range is immutable.</exception>
        void ICollection<int>.Clear()
        {
            throw new NotSupportedException("Range is immutable");
        }

        /// <summary>
        /// Return whether the given <paramref name="item"/> is included in this Range.
        /// 
        /// If the item covered by the range but is not hit due to the step, this will return false.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>Whether the given <paramref name="item"/> is included in this Range.</returns>
        public bool Contains(int item)
        {
            item -= _start;
            if ((item % _step) != 0)
            {
                return false;
            }

            item /= _step;
            return item >= 0 && item < Count;
        }

        /// <summary>
        /// Copies the entire Range to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional array to copy to. Must have 0-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="System.ArgumentException">
        ///     The number of elements in the source Range is greater than the available space from <paramref name="arrayIndex"/>
        ///     to the end of the destination <paramref name="array"/>.
        /// </exception>
        void ICollection<int>.CopyTo(int[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex", arrayIndex, "Must be at least 0");
            }
            else if (array.Length + arrayIndex < Count)
            {
                throw new ArgumentException("Array is not large enough.", "array");
            }

            int finish = _finish;
            if (_step > 0)
            {
                if (_isExclusive)
                {
                    finish--;
                }
                for (int current = _start; current <= finish; current += _step)
                {
                    array[arrayIndex++] = current;
                }
            }
            else
            {
                if (_isExclusive)
                {
                    finish++;
                }
                for (int current = _start; current >= finish; current += _step)
                {
                    array[arrayIndex++] = current;
                }
            }
        }

        private int? _count;
        /// <summary>
        /// The number of elements in the Range.
        /// </summary>
        public int Count
        {
            get
            {
                if (_count.HasValue)
                {
                    return _count.Value;
                }

                int length = (_finish - _start);
                length += _step;
                if (_isExclusive)
                {
                    if (_step > 0)
                    {
                        length--;
                    }
                    else
                    {
                        length++;
                    }
                }
                length /= _step;

                if (length < 0)
                {
                    length = 0;
                }

                _count = length;
                return length;
            }
        }

        /// <summary>
        /// Ranges are immutable, and therefore are always read-only.
        /// </summary>
        bool ICollection<int>.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Removing items is not supported.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>This method never returns.</returns>
        /// <exception cref="System.NotSupportedException">Range is immutable.</exception>
        bool ICollection<int>.Remove(int item)
        {
            throw new NotSupportedException("Range is immutable");
        }

        #endregion
    }
}
