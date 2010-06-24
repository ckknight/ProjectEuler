using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ckknight.ProjectEuler
{
    public static class FuncExtensions
    {
        public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            var cache = new Dictionary<T, TResult>();

            return t =>
            {
                TResult result;
                if (!cache.TryGetValue(t, out result))
                {
                    cache[t] = result = func(t);
                }
                return result;
            };
        }

        public static Func<T1, T2, TResult> Memoize<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            var cache = new Dictionary<T1, Dictionary<T2, TResult>>();

            return (t1, t2) =>
            {
                Dictionary<T2, TResult> cache2;
                if (!cache.TryGetValue(t1, out cache2))
                {
                    cache[t1] = cache2 = new Dictionary<T2, TResult>();
                }

                TResult result;
                if (!cache2.TryGetValue(t2, out result))
                {
                    cache2[t2] = result = func(t1, t2);
                }
                return result;
            };
        }

        public static Func<T1, T2, T3, TResult> Memoize<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            var cache = new Dictionary<T1, Dictionary<T2, Dictionary<T3, TResult>>>();

            return (t1, t2, t3) =>
            {
                Dictionary<T2, Dictionary<T3, TResult>> cache2;
                if (!cache.TryGetValue(t1, out cache2))
                {
                    cache[t1] = cache2 = new Dictionary<T2, Dictionary<T3, TResult>>();
                }

                Dictionary<T3, TResult> cache3;
                if (!cache2.TryGetValue(t2, out cache3))
                {
                    cache2[t2] = cache3 = new Dictionary<T3, TResult>();
                }

                TResult result;
                if (!cache3.TryGetValue(t3, out result))
                {
                    cache3[t3] = result = func(t1, t2, t3);
                }
                return result;
            };
        }

        public static Func<T1, T2, T3, T4, TResult> Memoize<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            var cache = new Dictionary<T1, Dictionary<T2, Dictionary<T3, Dictionary<T4, TResult>>>>();

            return (t1, t2, t3, t4) =>
            {
                Dictionary<T2, Dictionary<T3, Dictionary<T4, TResult>>> cache2;
                if (!cache.TryGetValue(t1, out cache2))
                {
                    cache[t1] = cache2 = new Dictionary<T2, Dictionary<T3, Dictionary<T4, TResult>>>();
                }

                Dictionary<T3, Dictionary<T4, TResult>> cache3;
                if (!cache2.TryGetValue(t2, out cache3))
                {
                    cache2[t2] = cache3 = new Dictionary<T3, Dictionary<T4, TResult>>();
                }

                Dictionary<T4, TResult> cache4;
                if (!cache3.TryGetValue(t3, out cache4))
                {
                    cache3[t3] = cache4 = new Dictionary<T4, TResult>();
                }

                TResult result;
                if (!cache4.TryGetValue(t4, out result))
                {
                    cache4[t4] = result = func(t1, t2, t3, t4);
                }
                return result;
            };
        }
    }
}
