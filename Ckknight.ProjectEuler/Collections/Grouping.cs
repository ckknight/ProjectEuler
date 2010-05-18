using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ckknight.ProjectEuler.Collections
{
    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        public Grouping(TKey key, IEnumerable<TElement> elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }

            _key = key;
            _elements = elements;
        }

        private readonly TKey _key;
        private readonly IEnumerable<TElement> _elements;

        #region IGrouping<TKey,TElement> Members

        public TKey Key
        {
            get
            {
                return _key;
            }
        }

        #endregion

        #region IEnumerable<TElement> Members

        public IEnumerator<TElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
