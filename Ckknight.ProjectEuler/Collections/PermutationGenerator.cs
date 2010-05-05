using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ckknight.ProjectEuler.Collections
{
    public sealed class PermutationGenerator<T> : IEnumerable<T[]>
    {
        public PermutationGenerator(IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            _source = sequence;
        }

        private readonly IEnumerable<T> _source;

        private static IEnumerable<T[]> Visit(int currentIndex, int length, T[] array, bool[] visitedIndexes, T[] result)
        {
            for (int i = 0; i < length; i++)
            {
                if (!visitedIndexes[i])
                {
                    result[currentIndex] = array[i];
                    if (currentIndex == length - 1)
                    {
                        T[] item = new T[length];
                        Array.Copy(result, item, length);
                        yield return item;
                    }
                    else
                    {
                        visitedIndexes[i] = true;
                        foreach (T[] item in Visit(currentIndex + 1, length, array, visitedIndexes, result))
                        {
                            yield return item;
                        }
                        visitedIndexes[i] = false;
                    }
                }
            }
        }

        #region IEnumerable<T> Members

        public IEnumerator<T[]> GetEnumerator()
        {
            T[] array = _source as T[] ?? _source.ToArray();
            int length = array.Length;

            bool[] visitedIndexes = new bool[length];

            foreach (T[] result in Visit(0, length, array, visitedIndexes, new T[length]))
            {
                yield return result;
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
