using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ckknight.ProjectEuler.Collections
{
    public class MemorableEnumerable<T> : IEnumerable<MemorableEnumerable<T>.Entry>
    {
        public MemorableEnumerable(IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            _source = source;
        }

        private readonly IEnumerable<T> _source;

        public class Entry
        {
            public Entry(T value)
            {
                _value = value;
                _hasPreviousValue = false;
            }
            public Entry(T value, T previousValue)
            {
                _value = value;
                _hasPreviousValue = true;
                _previousValue = previousValue;
            }

            private readonly T _value;
            private readonly bool _hasPreviousValue;
            private readonly T _previousValue;

            public T Value
            {
                get
                {
                    return _value;
                }
            }

            public bool HasPreviousValue
            {
                get
                {
                    return _hasPreviousValue;
                }
            }

            public T PreviousValue
            {
                get
                {
                    if (!_hasPreviousValue)
                    {
                        throw new InvalidOperationException("No previous value.");
                    }
                    return _previousValue;
                }
            }
        }

        #region IEnumerable<Entry> Members

        public IEnumerator<MemorableEnumerable<T>.Entry> GetEnumerator()
        {
            bool isFirst = true;
            T previous = default(T);
            foreach (T item in _source)
            {
                if (isFirst)
                {
                    isFirst = false;
                    yield return new Entry(item);
                }
                else
                {
                    yield return new Entry(item, previous);
                }
                previous = item;
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
