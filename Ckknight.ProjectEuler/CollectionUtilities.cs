using System;
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

        public static List<T> NewList<T>(params T[] args)
        {
            return new List<T>(args);
        }

        public static HashSet<T> NewHashSet<T>(params T[] args)
        {
            return new HashSet<T>(args);
        }
    }
}
