using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ckknight.ProjectEuler.Collections
{
    public class MultiHashSet<T> : IMultiSet<T>
    {
        public MultiHashSet()
            : this(null, null) { }

        public MultiHashSet(IEnumerable<T> sequence)
            : this(sequence, null) { }

        public MultiHashSet(IEqualityComparer<T> comparer)
            : this(null, comparer) { }

        public MultiHashSet(IEnumerable<T> sequence, IEqualityComparer<T> comparer)
        {
            _data = new Dictionary<T, int>(comparer);

            if (sequence != null)
            {
                IMultiSet<T> otherMultiSet = sequence as IMultiSet<T>;
                if (otherMultiSet == null)
                {
                    foreach (T item in sequence)
                    {
                        Add(item);
                    }
                }
                else
                {
                    foreach (T item in otherMultiSet.Distinct())
                    {
                        int count = otherMultiSet.GetCount(item);
                        if (count > 0)
                        {
                            _data[item] = count;
                        }
                    }
                }
            }
        }

        private readonly Dictionary<T, int> _data;

        public IEqualityComparer<T> Comparer
        {
            get
            {
                return _data.Comparer;
            }
        }

        public int GetCount(T item)
        {
            int count;
            if (_data.TryGetValue(item, out count))
            {
                return count;
            }
            else
            {
                return 0;
            }
        }

        public IEnumerable<T> Distinct()
        {
            return _data.Keys;
        }

        public void Add(T item)
        {
            int count;
            if (!_data.TryGetValue(item, out count))
            {
                count = 0;
            }
            count++;

            _data[item] = count;
        }

        public void Clear()
        {
            _data.Clear();
        }

        public bool Contains(T item)
        {
            return _data.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (T item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        public int Count
        {
            get
            {
                return _data.Values.Sum();
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(T item)
        {
            int count;
            if (!_data.TryGetValue(item, out count))
            {
                return false;
            }

            count--;
            if (count <= 0)
            {
                _data.Remove(item);
            }
            else
            {
                _data[item] = count;
            }
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (KeyValuePair<T, int> pair in _data)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    yield return pair.Key;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
