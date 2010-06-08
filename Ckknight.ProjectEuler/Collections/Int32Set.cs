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
    public class Int32Set : ISet<int>, ICollection<int>
    {
        public Int32Set(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity", capacity, "Must be at least 0");
            }
            _capacity = capacity;
            _bucket = new BooleanArray(capacity);
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
        private readonly BooleanArray _bucket;

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

        void ICollection<int>.Add(int item)
        {
            Add(item);
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

        public ParallelQuery<int> AsParallel()
        {
            return ParallelEnumerable.Range(0, _capacity)
                .Where(i => _bucket[i]);
        }

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

        public bool Add(int item)
        {
            if (item < 0)
            {
                throw new ArgumentOutOfRangeException("item", item, "Must be at least 0");
            }
            else if (item >= _capacity)
            {
                throw new ArgumentOutOfRangeException("item", item, string.Format("Must be at most {0}", _capacity - 1));
            }

            if (_bucket[item])
            {
                return false;
            }
            else
            {
                _bucket[item] = true;
                return true;
            }
        }

        public void ExceptWith(IEnumerable<int> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            foreach (int item in other)
            {
                _bucket[item] = false;
            }
        }

        public void IntersectWith(IEnumerable<int> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            ISet<int> otherSet = other as ISet<int> ?? other.ToHashSet();
            foreach (int item in this)
            {
                if (!otherSet.Contains(item))
                {
                    _bucket[item] = false;
                }
            }
        }

        public bool IsProperSubsetOf(IEnumerable<int> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            
            ISet<int> otherSet = other as ISet<int> ?? other.ToHashSet();
            foreach (int item in this)
            {
                if (!otherSet.Contains(item))
                {
                    return false;
                }
            }
            foreach (int item in otherSet)
            {
                if (!Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsProperSupersetOf(IEnumerable<int> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            ISet<int> otherSet = other as ISet<int> ?? other.ToHashSet();
            foreach (int item in otherSet)
            {
                if (!Contains(item))
                {
                    return false;
                }
            }

            foreach (int item in this)
            {
                if (!otherSet.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsSubsetOf(IEnumerable<int> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            ISet<int> otherSet = other as ISet<int> ?? other.ToHashSet();
            foreach (int item in this)
            {
                if (!otherSet.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsSupersetOf(IEnumerable<int> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            foreach (int item in other)
            {
                if (!Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        public bool Overlaps(IEnumerable<int> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            foreach (int item in other)
            {
                if (Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public bool SetEquals(IEnumerable<int> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            var otherSet = other.ToHashSet();
            foreach (int item in this)
            {
                if (!otherSet.Contains(item))
                {
                    return false;
                }
                otherSet.Remove(item);
            }
            return otherSet.Count == 0;
        }

        public void SymmetricExceptWith(IEnumerable<int> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            foreach (int item in other.Distinct())
            {
                _bucket[item] = !_bucket[item];
            }
        }

        public void UnionWith(IEnumerable<int> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            foreach (int item in other)
            {
                _bucket[item] = true;
            }
        }
    }
}
