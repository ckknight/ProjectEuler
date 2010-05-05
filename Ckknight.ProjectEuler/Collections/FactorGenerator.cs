using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ckknight.ProjectEuler.Collections
{
    public class FactorGenerator : IEnumerable<long>
    {
        public FactorGenerator(long number)
            : this(number, true) { }

        public FactorGenerator(long number, bool includeNumber)
        {
            if (number < 1)
            {
                throw new ArgumentOutOfRangeException("number", number, "Must be at least 1");
            }

            _number = number;
            _includeNumber = includeNumber;
        }

        private readonly long _number;
        private readonly bool _includeNumber;

        #region IEnumerable<long> Members

        public IEnumerator<long> GetEnumerator()
        {
            long[] primeFactors = new PrimeFactorGenerator(_number).ToArray();

            HashSet<long> divisors = new HashSet<long> { 1L };
            int max = primeFactors.Length;
            if (_includeNumber)
            {
                max++;
            }
            for (int i = 1; i < max; i++)
            {
                divisors.UnionWith(primeFactors.GetCombinations(i).Select(x => x.Product()));
            }
            return divisors.GetEnumerator();
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
