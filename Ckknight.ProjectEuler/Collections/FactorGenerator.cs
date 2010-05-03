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
        {
            if (number < 1)
            {
                throw new ArgumentOutOfRangeException("number", number, "Must be at least 1");
            }

            _number = number;
        }

        private readonly long _number;

        #region IEnumerable<long> Members

        public IEnumerator<long> GetEnumerator()
        {
            long[] primeFactors = new PrimeFactorGenerator(_number).ToArray();

            HashSet<long> divisors = new HashSet<long> { 1L };
            for (int i = 1; i <= primeFactors.Length; i++)
            {
                divisors.UnionWith(primeFactors.GetCombinations(i).Select(x => x.Aggregate((a, b) => a * b)));
            }
            return divisors.OrderBy(x => x).GetEnumerator();
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
