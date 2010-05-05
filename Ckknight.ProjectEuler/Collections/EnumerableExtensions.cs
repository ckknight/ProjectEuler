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
    }
}
