using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
