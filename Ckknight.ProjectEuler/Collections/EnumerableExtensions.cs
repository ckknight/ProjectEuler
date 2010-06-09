using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Numerics;

namespace Ckknight.ProjectEuler.Collections
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Return whether the given <paramref name="sequence"/> is
        /// palindromatic.
        /// 
        /// A sequence is considered a palindrome if its first object is equal
        /// to its last, its second is equal to its second-to-last, and so on
        /// until the middle is reached.
        /// </summary>
        /// <typeparam name="T">The type of element of the sequence.</typeparam>
        /// <param name="sequence">The sequence to check for palindromaticity.</param>
        /// <returns>True if the <paramref name="sequence"/> is a palindrome.</returns>
        public static bool IsPalindrome<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            T[] array = sequence as T[] ?? sequence.ToArray();
            return IsPalindrome(array);
        }

        /// <summary>
        /// Return whether the given <paramref name="array"/> is palindromatic.
        /// 
        /// A sequence is considered a array if its first object is equal
        /// to its last, its second is equal to its second-to-last, and so on
        /// until the middle is reached.
        /// </summary>
        /// <typeparam name="T">The type of element of the array.</typeparam>
        /// <param name="array">The array to check for palindromaticity.</param>
        /// <returns>True if the <paramref name="array"/> is a palindrome.</returns>
        public static bool IsPalindrome<T>(this T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            int length = array.Length;
            int midLength = length / 2;
            for (int i = 0; i < midLength; i++)
            {
                if (!object.Equals(array[i], array[length - i - 1]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Return a sequence of <paramref name="amount"/>-sized combinations from the given <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="sequence">The sequence to pull data from.</param>
        /// <param name="amount">The size of each combination.</param>
        /// <returns>A sequence of combinations.</returns>
        public static IEnumerable<T[]> GetCombinations<T>(this IEnumerable<T> sequence, int amount)
        {
            return new CombinationGenerator<T>(sequence, amount);
        }

        public static string StringJoin<T>(this IEnumerable<T> sequence, string separator)
        {
            return string.Join(separator, sequence);
        }

        /// <summary>
        /// Return an enumerable that skips the last <paramref name="count"/> items of the given <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The element type of the sequence.</typeparam>
        /// <param name="sequence">The sequence with which to skip the last elements of.</param>
        /// <param name="count">The amount of items to skip at the end.</param>
        /// <returns>The enumerable with the last <paramref name="count"/> items skipped.</returns>
        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> sequence, int count)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", count, "Must be at least 0");
            }

            Queue<T> queue = new Queue<T>(count);

            foreach (T item in sequence)
            {
                if (queue.Count == count)
                {
                    yield return queue.Dequeue();
                }
                queue.Enqueue(item);
            }
        }

        /// <summary>
        /// Create a new HashSet from the provided <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The element type of the sequence</typeparam>
        /// <param name="sequence">The enumerable to create a HashSet from.</param>
        /// <returns>A HashSet.</returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            return new HashSet<T>(sequence);
        }

        /// <summary>
        /// Create a new HashSet from the provided <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The element type of the sequence</typeparam>
        /// <param name="sequence">The enumerable to create a HashSet from.</param>
        /// <param name="comparer">The comparer to use for the HashSet.</param>
        /// <returns>A HashSet.</returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> sequence, IEqualityComparer<T> comparer)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            return new HashSet<T>(sequence, comparer);
        }

        public static ImmutableSequence<T> ToImmutableSequence<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            return new ImmutableSequence<T>(sequence);
        }

        /// <summary>
        /// Create a new Int32Set based on the elements of the provided <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">The sequence to take the initial data from.</param>
        /// <param name="capacity">The capacity of the Int32Set.</param>
        /// <returns>An Int32Set of the given data.</returns>
        public static Int32Set ToInt32Set(this IEnumerable<int> sequence, int capacity)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            
            return new Int32Set(sequence, capacity);
        }

        /// <summary>
        /// Return the mathematical product of the sequence of ints.
        /// </summary>
        /// <param name="sequence">The sequence of ints.</param>
        /// <returns>The product of all the ints.</returns>
        public static int Product(this IEnumerable<int> sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            int value = 1;
            foreach (int item in sequence)
            {
                value *= item;
            }
            return value;
        }

        /// <summary>
        /// Return the mathematical product of the sequence of longs.
        /// </summary>
        /// <param name="sequence">The sequence of longs.</param>
        /// <returns>The product of all the longs.</returns>
        public static long Product(this IEnumerable<long> sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            long value = 1;
            foreach (int item in sequence)
            {
                value *= item;
            }
            return value;
        }

        public static BigInteger Sum<T>(this IEnumerable<T> sequence, Func<T, BigInteger> selector)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            return sequence.Select(selector).Sum();
        }

        public static BigInteger Sum(this IEnumerable<BigInteger> sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            BigInteger value = BigInteger.Zero;
            foreach (BigInteger item in sequence)
            {
                value += item;
            }
            return value;
        }

        public static BigInteger Product<T>(this IEnumerable<T> sequence, Func<T, BigInteger> selector)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            return sequence.Select(selector).Product();
        }

        public static BigInteger Product(this IEnumerable<BigInteger> sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            BigInteger value = BigInteger.One;
            foreach (BigInteger item in sequence)
            {
                value *= item;
            }
            return value;
        }

        /// <summary>
        /// Return all permutations of the given <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The element type of the sequence.</typeparam>
        /// <param name="sequence">The sequence to permute.</param>
        /// <returns>An enumerable containing all permutations of the sequence.</returns>
        public static IEnumerable<T[]> GetPermutations<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            return new PermutationGenerator<T>(sequence);
        }

        public static IEnumerable<T> ElementsAt<T>(this IEnumerable<T> sequence, params int[] indexes)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else if (indexes == null)
            {
                throw new ArgumentNullException("indexes");
            }

            if (indexes.Length == 0)
            {
                return Enumerable.Empty<T>();
            }

            foreach (int index in indexes)
            {
                if (index < 0)
                {
                    throw new ArgumentException("All items must be at least 0", "indexes");
                }
            }

            var indexSet = new HashSet<int>(indexes);
            var dict = new Dictionary<int, T>();
            int maximum = indexSet.Max();

            int count = 0;
            foreach (T item in sequence)
            {
                if (indexSet.Contains(count))
                {
                    dict[count] = item;
                }

                count++;
                if (count > maximum)
                {
                    break;
                }
            }

            T[] results = new T[indexes.Length];
            for (int i = 0; i < indexes.Length; i++)
            {
                int index = indexes[i];
                if (!dict.ContainsKey(index))
                {
                    throw new ArgumentOutOfRangeException("Index greater than the number of elements in the sequence", "indexes");
                }

                results[i] = dict[index];
            }
            return results;
        }

        public static IEnumerable<T> OrderedIntersect<T>(this IEnumerable<T> sequence, IEnumerable<T> other) where T : IComparable<T>
        {
            return OrderedIntersect(sequence, other, null);
        }

        public static IEnumerable<T> OrderedIntersect<T>(this IEnumerable<T> sequence, IEnumerable<T> other, IComparer<T> comparer) where T : IComparable<T>
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            if (comparer == null)
            {
                comparer = Comparer<T>.Default;
            }

            using (IEnumerator<T> first = sequence.GetEnumerator())
            {
                using (IEnumerator<T> second = other.GetEnumerator())
                {
                    if (!first.MoveNext())
                    {
                        yield break;
                    }
                    else if (!second.MoveNext())
                    {
                        yield break;
                    }

                    T firstValue = first.Current;
                    T secondValue = second.Current;

                    while (true)
                    {
                        int comparison = comparer.Compare(firstValue, secondValue);
                        if (comparison < 0)
                        {
                            if (!first.MoveNext())
                            {
                                yield break;
                            }
                            firstValue = first.Current;
                        }
                        else if (comparison > 0)
                        {
                            if (!second.MoveNext())
                            {
                                yield break;
                            }
                            secondValue = second.Current;
                        }
                        else if (comparison == 0)
                        {
                            yield return first.Current;
                            if (!first.MoveNext())
                            {
                                yield break;
                            }
                            if (!second.MoveNext())
                            {
                                yield break;
                            }
                            firstValue = first.Current;
                            secondValue = second.Current;
                        }
                    }
                }
            }
        }

        public static IEnumerable<TResult> ToMemorableEnumerable<TElement, TResult>(this IEnumerable<TElement> sequence, int count, Func<TElement[], TResult> resultCreator)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", count, "Must be at least 0");
            }
            else if (resultCreator == null)
            {
                throw new ArgumentNullException("resultCreator");
            }

            count++;
            var queue = new Queue<TElement>(count);
            foreach (TElement item in sequence)
            {
                queue.Enqueue(item);
                yield return resultCreator(queue.ToArray());
                if (queue.Count == count)
                {
                    queue.Dequeue();
                }
            }
        }

        public static IEnumerable<TResult> SelectWithAggregate<T, TResult>(this IEnumerable<T> sequence, TResult seed, Func<TResult, T, TResult> generator)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            foreach (T item in sequence)
            {
                seed = generator(seed, item);
                yield return seed;
            }
        }

        public static IEnumerable<TElement> Skip<TElement>(this TElement[] array, int amount)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount", amount, "Must be at least 0");
            }
            else if (amount == 0)
            {
                return array;
            }

            int length = array.Length;
            if (amount >= array.Length)
            {
                return Enumerable.Empty<TElement>();
            }

            return SkipHelper(array, amount, length);
        }

        private static IEnumerable<TElement> SkipHelper<TElement>(TElement[] array, int amount, int length)
        {
            for (int i = amount; i < length; i++)
            {
                yield return array[i];
            }
        }

        public static IEnumerable<TElement> Take<TElement>(this TElement[] array, int amount)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount", amount, "Must be at least 0");
            }
            else if (amount == 0)
            {
                return Enumerable.Empty<TElement>();
            }

            int length = array.Length;
            if (length == 0)
            {
                return array;
            }

            if (amount < length)
            {
                length = amount;
            }

            return TakeHelper(array, length);
        }

        private static IEnumerable<TElement> TakeHelper<TElement>(TElement[] array, int length)
        {
            for (int i = 0; i < length; i++)
            {
                yield return array[i];
            }
        }

        public static IEnumerable<T> Replace<T>(this IEnumerable<T> sequence, T oldValue, T newValue)
        {
            return Replace(sequence, oldValue, newValue, null);
        }

        public static IEnumerable<T> Replace<T>(this IEnumerable<T> sequence, T oldValue, T newValue, IEqualityComparer<T> comparer)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            foreach (T item in sequence)
            {
                if (comparer.Equals(item, oldValue))
                {
                    yield return newValue;
                }
                else
                {
                    yield return item;
                }
            }
        }

        public static IGrouping<TKey, TElement> ToGrouping<TKey, TElement>(this IEnumerable<TElement> sequence, TKey key)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            return new Grouping<TKey, TElement>(key, sequence);
        }

        public static IEnumerable<IGrouping<TKey, TElement>> OrderedGroupBy<TKey, TElement>(this IEnumerable<TElement> sequence, Func<TElement, TKey> keySelector)
        {
            return OrderedGroupBy<TKey, TElement>(sequence, keySelector, null);
        }
        public static IEnumerable<IGrouping<TKey, TElement>> OrderedGroupBy<TKey, TElement>(this IEnumerable<TElement> sequence, Func<TElement, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            if (comparer == null)
            {
                comparer = EqualityComparer<TKey>.Default;
            }

            TKey currentKey = default(TKey);
            List<TElement> currentElements = null;
            foreach (TElement item in sequence)
            {
                if (currentElements == null)
                {
                    currentElements = new List<TElement>();
                    currentElements.Add(item);
                    currentKey = keySelector(item);
                }
                else
                {
                    TKey newKey = keySelector(item);
                    if (!comparer.Equals(currentKey, newKey))
                    {
                        yield return new Grouping<TKey, TElement>(currentKey, currentElements);
                        currentKey = newKey;
                        currentElements = new List<TElement>();
                    }
                    currentElements.Add(item);
                }
            }
            if (currentElements != null)
            {
                yield return new Grouping<TKey, TElement>(currentKey, currentElements);
            }
        }

        public static int SequenceCompare<T>(this IEnumerable<T> sequence, IEnumerable<T> other)
        {
            return SequenceCompare<T>(sequence, other, null);
        }

        public static int SequenceCompare<T>(this IEnumerable<T> sequence, IEnumerable<T> other, IComparer<T> comparer)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            if (comparer == null)
            {
                comparer = Comparer<T>.Default;
            }

            using (IEnumerator<T> alpha = sequence.GetEnumerator())
            {
                using (IEnumerator<T> bravo = other.GetEnumerator())
                {
                    while (true)
                    {
                        if (!alpha.MoveNext())
                        {
                            if (bravo.MoveNext())
                            {
                                return -1;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                        else if (!bravo.MoveNext())
                        {
                            return 1;
                        }

                        int cmp = comparer.Compare(alpha.Current, bravo.Current);
                        if (cmp != 0)
                        {
                            return cmp;
                        }
                    }
                }
            }
        }

        public static MultiHashSet<T> ToMultiHashSet<T>(this IEnumerable<T> sequence)
        {
            return ToMultiHashSet<T>(sequence, null);
        }

        public static MultiHashSet<T> ToMultiHashSet<T>(this IEnumerable<T> sequence, IEqualityComparer<T> comparer)
        {
            return new MultiHashSet<T>(sequence, comparer);
        }

        public static IEnumerable<T> PrependItem<T>(this IEnumerable<T> sequence, T item)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            yield return item;
            foreach (T member in sequence)
            {
                yield return member;
            }
        }

        public static IEnumerable<T> AppendItem<T>(this IEnumerable<T> sequence, T item)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            foreach (T member in sequence)
            {
                yield return member;
            }
            yield return item;
        }

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> sequence) where T : class
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            return sequence
                .Where(x => x != null);
        }

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> sequence) where T : struct
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            return sequence
                .Where(x => x.HasValue)
                .Select(x => x.Value);
        }

        public static IEnumerable<T> TakeWhileDistinct<T>(this IEnumerable<T> sequence)
        {
            return TakeWhileDistinct(sequence, false);
        }

        public static IEnumerable<T> TakeWhileDistinct<T>(this IEnumerable<T> sequence, bool includeLast)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            HashSet<T> set = new HashSet<T>();
            foreach (T item in sequence)
            {
                if (set.Contains(item))
                {
                    if (includeLast)
                    {
                        yield return item;
                    }
                    yield break;
                }

                yield return item;
                set.Add(item);
            }
        }

        public static TElement WithMax<TElement, TValue>(this IEnumerable<TElement> sequence, Func<TElement, TValue> selector)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            IComparer<TValue> comparer = Comparer<TValue>.Default;

            return sequence
                .Select(e => new
                {
                    Element = e,
                    Value = selector(e)
                })
                .Aggregate((a, b) => comparer.Compare(a.Value, b.Value) >= 0 ? a : b)
                .Element;
        }

        public static TElement WithMin<TElement, TValue>(this IEnumerable<TElement> sequence, Func<TElement, TValue> selector)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            IComparer<TValue> comparer = Comparer<TValue>.Default;

            return sequence
                .Select(e => new
                {
                    Element = e,
                    Value = selector(e)
                })
                .Aggregate((a, b) => comparer.Compare(a.Value, b.Value) <= 0 ? a : b)
                .Element;
        }

        public static TElement WithMax<TElement, TValue>(this ParallelQuery<TElement> sequence, Func<TElement, TValue> selector)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            IComparer<TValue> comparer = Comparer<TValue>.Default;

            return sequence
                .Select(e => new
                {
                    Element = e,
                    Value = selector(e)
                })
                .Aggregate((a, b) => comparer.Compare(a.Value, b.Value) >= 0 ? a : b)
                .Element;
        }

        public static TElement WithMin<TElement, TValue>(this ParallelQuery<TElement> sequence, Func<TElement, TValue> selector)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            IComparer<TValue> comparer = Comparer<TValue>.Default;

            return sequence
                .Select(e => new
                {
                    Element = e,
                    Value = selector(e)
                })
                .Aggregate((a, b) => comparer.Compare(a.Value, b.Value) <= 0 ? a : b)
                .Element;
        }

        public static bool IsPermutation<T>(this IEnumerable<T> sequence, IEnumerable<T> other)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            List<T> otherList = other.ToList();
            ICollection<T> collection = sequence as ICollection<T>;
            if (collection != null && collection.Count != otherList.Count)
            {
                return false;
            }
            foreach (T item in sequence)
            {
                if (!otherList.Remove(item))
                {
                    return false;
                }
            }
            return otherList.Count == 0;
        }

        public static IEnumerable<T> AssertEach<T>(this IEnumerable<T> sequence, Func<T, bool> tester)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            List<T> list = new List<T>();

            foreach (T item in sequence)
            {
                if (!tester(item))
                {
                    throw new InvalidOperationException(string.Format("Item #{0} ({1}) failed test", list.Count + 1, item));
                }
                list.Add(item);
            }

            return list;
        }
    }
}
