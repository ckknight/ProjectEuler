using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ckknight.ProjectEuler
{
    public static class ObjectExtensions
    {
        public static TResult Let<T, TResult>(this T target, Func<T, TResult> converter)
        {
            return converter(target);
        }
    }
}
