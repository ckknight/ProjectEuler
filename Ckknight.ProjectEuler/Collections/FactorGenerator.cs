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

            var divisors = new Range(primeFactors.Length)
                .Aggregate(new HashSet<long> { 1L }, (x, i) =>
                {
                    long primeFactor = primeFactors[i];
                    x.UnionWith(x.Select(v => v * primeFactor).ToArray());
                    return x;
                });
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
