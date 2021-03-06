﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ckknight.ProjectEuler
{
    public static class CollectionUtilities
    {
        public static T[] EmptyArray<T>(Func<T> typeCreator)
        {
            return new T[0];
        }

        public static IEnumerable<T> EmptyEnumerable<T>(Func<T> typeCreator)
        {
            return Enumerable.Empty<T>();
        }

        public static List<T> EmptyList<T>(Func<T> typeCreator)
        {
            return new List<T>();
        }

        public static HashSet<T> EmptyHashSet<T>(Func<T> typeCreator)
        {
            return new HashSet<T>();
        }

        public static Dictionary<TKey, TValue> EmptyDictionary<TKey, TValue>(Func<TKey> keyTypeCreator, Func<TValue> valueTypeCreator)
        {
            return new Dictionary<TKey, TValue>();
        }

        public static List<T> NewList<T>(params T[] args)
        {
            return new List<T>(args);
        }

        public static HashSet<T> NewHashSet<T>(params T[] args)
        {
            return new HashSet<T>(args);
        }

        public static IEqualityComparer<T> NewEqualityComparer<T>(Func<T, T, bool> equalityChecker, Func<T, int> hashCodeCreator)
        {
            if (equalityChecker == null)
            {
                throw new ArgumentNullException("equalityChecker");
            }
            else if (hashCodeCreator == null)
            {
                throw new ArgumentNullException("hashCodeCreator");
            }
            return new EqualityComparerHelper<T>(equalityChecker, hashCodeCreator); 
        }

        private class EqualityComparerHelper<T> : IEqualityComparer<T>
        {
            public EqualityComparerHelper(Func<T, T, bool> equalityChecker, Func<T, int> hashCodeCreator)
            {
                _equalityChecker = equalityChecker;
                _hashCodeCreator = hashCodeCreator;
            }
            private readonly Func<T, T, bool> _equalityChecker;
            private readonly Func<T, int> _hashCodeCreator;

            #region IEqualityComparer<T> Members

            public bool Equals(T x, T y)
            {
                return _equalityChecker(x, y);
            }

            public int GetHashCode(T obj)
            {
                return _hashCodeCreator(obj);
            }

            #endregion
        }

        public static IEqualityComparer<T> NewEqualityComparer<T>(Func<T, T, bool> equalityChecker, Func<T, int> hashCodeCreator, Func<T> typeCreator)
        {
            return NewEqualityComparer<T>(equalityChecker, hashCodeCreator);
        }

        public static IEqualityComparer<IEnumerable<T>> GetSequenceEqualityComparer<T>()
        {
            return SequenceEqualityComparerHelper<T>.Instance;
        }

        private class SequenceEqualityComparerHelper<T> : IEqualityComparer<IEnumerable<T>>
        {
            private SequenceEqualityComparerHelper() { }

            private static readonly SequenceEqualityComparerHelper<T> _instance = new SequenceEqualityComparerHelper<T>();
            public static SequenceEqualityComparerHelper<T> Instance
            {
                get
                {
                    return _instance;
                }
            }

            public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
            {
                if (x == null)
                {
                    return y == null;
                }
                else if (y == null)
                {
                    return false;
                }
                else
                {
                    return x.SequenceEqual(y);
                }
            }

            public int GetHashCode(IEnumerable<T> obj)
            {
                int hashCode = 0;
                foreach (T item in obj)
                {
                    hashCode ^= item.GetHashCode();
                }
                return hashCode;
            }
        }

        public static IEnumerable<T> Repeat<T>(T item)
        {
            while (true)
            {
                yield return item;
            }
        }
    }
}
