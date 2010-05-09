using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ckknight.ProjectEuler.Collections
{
    /// <summary>
    /// Represents a sequence of elements where it is very easy to get the first element or skip elements.
    /// 
    /// This functions in a linked-list style fashion or similar to LISP's cons system.
    /// </summary>
    /// <typeparam name="T">The element type of the sequence.</typeparam>
    public class ImmutableSequence<T> : IEnumerable<T>
    {
        public ImmutableSequence(params T[] args)
            : this((IEnumerable<T>)args) { }

        public ImmutableSequence(IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            T[] array = sequence as T[] ?? sequence.ToArray();

            if (array.Length == 0)
            {
                _hasValue = false;
                _head = default(T);
                _tail = Empty;
            }
            else
            {
                ImmutableSequence<T> current = Empty;
                for (int i = array.Length - 1; i > 0; i--)
                {
                    current = new ImmutableSequence<T>(array[i], current);
                }

                _hasValue = true;
                _head = array[0];
                _tail = current;
            }
        }

        private ImmutableSequence(T head, ImmutableSequence<T> tail)
        {
            if (tail == null)
            {
                throw new ArgumentNullException("tail");
            }
            _hasValue = true;
            _head = head;
            _tail = tail;
        }

        private ImmutableSequence()
        {
            _hasValue = false;
            _head = default(T);
            _tail = Empty;
        }

        public static readonly ImmutableSequence<T> Empty = new ImmutableSequence<T>();

        private readonly bool _hasValue;
        private readonly T _head;
        private readonly ImmutableSequence<T> _tail;
        public bool HasValue
        {
            get
            {
                return _hasValue;
            }
        }

        public T First()
        {
            if (_hasValue)
            {
                return _head;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public T FirstOrDefault()
        {
            return _hasValue ? _head : default(T);
        }

        public ImmutableSequence<T> Skip(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount", amount, "Must be at least 0");
            }
            else if (amount == 0)
            {
                return this;
            }
            else if (amount == 1)
            {
                return _tail ?? Empty;
            }
            else
            {
                ImmutableSequence<T> current = this;
                while (amount > 0 && current._hasValue)
                {
                    current = current._tail ?? Empty;
                    amount--;
                }
                return current;
            }
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            ImmutableSequence<T> current = this;
            while (current._hasValue)
            {
                yield return current._head;
                current = current._tail ?? Empty;
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
