using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ckknight.ProjectEuler.Collections
{
    public class DefaultDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary
    {
        public DefaultDictionary(Func<TKey, TValue> populator)
            : this(populator, null, null) { }

        public DefaultDictionary(Func<TKey, TValue> populator, IDictionary<TKey, TValue> dictionary)
            : this(populator, dictionary, null) { }

        public DefaultDictionary(Func<TKey, TValue> populator, IEqualityComparer<TKey> comparer)
            : this(populator, null, comparer) { }

        public DefaultDictionary(Func<TKey, TValue> populator, IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            if (dictionary == null)
            {
                _data = new Dictionary<TKey, TValue>(comparer);
            }
            else
            {
                _data = new Dictionary<TKey, TValue>(dictionary, comparer);
            }
            _populator = populator;
        }

        private readonly Dictionary<TKey, TValue> _data;
        private readonly Func<TKey, TValue> _populator;

        public void Add(TKey key, TValue value)
        {
            _data.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return true;
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return _data.Keys;
            }
        }

        public bool Remove(TKey key)
        {
            _data.Remove(key);
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_data.TryGetValue(key, out value))
            {
                return true;
            }
            else
            {
                _data[key] = value = _populator(key);
                return true;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return _data.Values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                TryGetValue(key, out value);
                return value;
            }
            set
            {
                _data[key] = value;
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _data.Clear();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            TryGetValue(item.Key, out value);
            return object.Equals(item.Value, value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_data).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return _data.Count;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            TryGetValue(item.Key, out value);
            if (object.Equals(item.Value, value))
            {
                _data.Remove(item.Key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void IDictionary.Add(object key, object value)
        {
            if (!(key is TKey))
            {
                throw new ArgumentException(string.Format("Must be a {0}", typeof(TKey)), "key");
            }
            else if (!(value is TValue))
            {
                throw new ArgumentException(string.Format("Must be a {0}", typeof(TValue)), "value");
            }

            Add((TKey)key, (TValue)value);
        }

        bool IDictionary.Contains(object key)
        {
            if (key is TKey)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)_data).GetEnumerator();
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return ((IDictionary)_data).Keys;
            }
        }

        public void Remove(object key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (key is TKey)
            {
                _data.Remove((TKey)key);
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return ((IDictionary)_data).Values;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                if (!(key is TKey))
                {
                    return null;
                }
                else
                {
                    return this[(TKey)key];
                }
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }
                else if (!(key is TKey))
                {
                    throw new ArgumentException(string.Format("Must be a {0}", typeof(TKey)), "key");
                }
                else if (!(value is TValue))
                {
                    throw new ArgumentException(string.Format("Must be a {0}", typeof(TValue)), "value");
                }

                this[(TKey)key] = (TValue)value;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)_data).CopyTo(array, index);
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return true;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return _data;
            }
        }
    }
}
