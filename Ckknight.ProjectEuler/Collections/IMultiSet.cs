using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ckknight.ProjectEuler.Collections
{
    public interface IMultiSet<T> : ICollection<T>
    {
        int GetCount(T item);
        IEnumerable<T> Distinct();
    }
}
