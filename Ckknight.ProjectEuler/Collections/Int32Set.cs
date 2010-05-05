using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ckknight.ProjectEuler.Collections
{
    /// <summary>
    /// Represents a set which can contain only Int32s up to a certain capacity.
    /// 
    /// This is meant to be very fast for adding, removing, and checking to see whether an Int32 is contained.
    /// </summary>
    public class Int32Set : ICollection<int>
    {
        public Int32Set(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity", capacity, "Must be at least 0");
            }
            _capacity = capacity;
            _bucket = new BitArray(capacity);
        }

        public Int32Set(IEnumerable<int> sequence, int capacity)
            : this(capacity)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            foreach (int item in sequence)
            {
                if (item < 0 || item >= capacity)
                {
                    throw new ArgumentException("Item must be no less than 0 and not greater or equal to the provided capacity", "sequence");
                }
                _bucket[item] = true;
            }
        }

        private readonly int _capacity;
        private readonly BitArray _bucket;

        public int Capacity
        {
            get
            {
                return _capacity;
            }
        }

        public IEnumerable<int> GetInverse()
        {
            for (int i = 0; i < _capacity; i++)
            {
                if (!_bucket[i])
                {
                    yield return i;
                }
            }
        }

        #region ICollection<int> Members

        public void Add(int item)
        {
            if (item < 0)
            {
                throw new ArgumentOutOfRangeException("item", item, "Must be at least 0");
            }
            else if (item >= _capacity)
            {
                throw new ArgumentOutOfRangeException("item", item, string.Format("Must be at most {0}", _capacity - 1));
            }

            _bucket[item] = true;
        }

        public void Clear()
        {
            for (int i = 0; i < _capacity; i++)
            {
                _bucket[i] = false;
            }
        }

        public bool Contains(int item)
        {
            if (item < 0 || item >= _capacity)
            {
                return false;
            }
            return _bucket[item];
        }

        void ICollection<int>.CopyTo(int[] array, int arrayIndex)
        {
            for (int i = 0; i < _capacity; i++)
            {
                if (_bucket[i])
                {
                    array[arrayIndex++] = i;
                }
            }
        }

        int ICollection<int>.Count
        {
            get
            {
                int count = 0;
                for (int i = 0; i < _capacity; i++)
                {
                    if (_bucket[i])
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        bool ICollection<int>.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public bool Remove(int item)
        {
            if (item < 0 || item >= _capacity)
            {
                return false;
            }

            if (_bucket[item])
            {
                _bucket[item] = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region IEnumerable<int> Members

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < _capacity; i++)
            {
                if (_bucket[i])
                {
                    yield return i;
                }
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
