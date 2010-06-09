using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Ckknight.ProjectEuler.Collections
{
    public static class DictionaryExtensions
    {
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
        {
            if (dict == null)
            {
                throw new ArgumentNullException("dict");
            }
            else if (addValueFactory == null)
            {
                throw new ArgumentNullException("addValueFactory");
            }
            else if (updateValueFactory == null)
            {
                throw new ArgumentNullException("updateValueFactory");
            }

            ConcurrentDictionary<TKey, TValue> concurrentDict = dict as ConcurrentDictionary<TKey, TValue>;
            if (concurrentDict != null)
            {
                return concurrentDict.AddOrUpdate(key, addValueFactory, updateValueFactory);
            }

            TValue result;
            TValue current;
            if (!dict.TryGetValue(key, out current))
            {
                dict[key] = result = addValueFactory(key);
            }
            else
            {
                dict[key] = result = updateValueFactory(key, current);
            }
            return result;
        }

        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
        {
            if (dict == null)
            {
                throw new ArgumentNullException("dict");
            }
            else if (updateValueFactory == null)
            {
                throw new ArgumentNullException("updateValueFactory");
            }

            ConcurrentDictionary<TKey, TValue> concurrentDict = dict as ConcurrentDictionary<TKey, TValue>;
            if (concurrentDict != null)
            {
                return concurrentDict.AddOrUpdate(key, addValue, updateValueFactory);
            }

            TValue result;
            TValue current;
            if (!dict.TryGetValue(key, out current))
            {
                dict[key] = result = addValue;
            }
            else
            {
                dict[key] = result = updateValueFactory(key, current);
            }
            return result;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueFactory)
        {
            if (dict == null)
            {
                throw new ArgumentNullException("dict");
            }
            else if (valueFactory == null)
            {
                throw new ArgumentNullException("valueFactory");
            }

            ConcurrentDictionary<TKey, TValue> concurrentDict = dict as ConcurrentDictionary<TKey, TValue>;
            if (concurrentDict != null)
            {
                return concurrentDict.GetOrAdd(key, valueFactory);
            }

            TValue result;
            if (!dict.TryGetValue(key, out result))
            {
                dict[key] = result = valueFactory(key);
            }
            return result;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict == null)
            {
                throw new ArgumentNullException("dict");
            }

            ConcurrentDictionary<TKey, TValue> concurrentDict = dict as ConcurrentDictionary<TKey, TValue>;
            if (concurrentDict != null)
            {
                return concurrentDict.GetOrAdd(key, value);
            }

            TValue result;
            if (!dict.TryGetValue(key, out result))
            {
                dict[key] = result = value;
            }
            return result;
        }
    }
}
