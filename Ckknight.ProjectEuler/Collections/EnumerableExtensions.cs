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
    }
}
