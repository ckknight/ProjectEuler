using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(60,
        @"The primes 3, 7, 109, and 673, are quite remarkable. By taking any
        two primes and concatenating them in any order the result will always
        be prime. For example, taking 7 and 109, both 7109 and 1097 are prime.
        The sum of these four primes, 792, represents the lowest sum for a set
        of four primes with this property.

        Find the lowest sum for a set of five primes for which any two primes
        concatenate to produce another prime.")]
    public class Problem060 : BaseProblem
    {
        public override object CalculateResult()
        {
            int maximum = 10000;

            long[] primes = PrimeGenerator.Instance
                .TakeWhile(p => p < maximum)
                .ToArray();

            int length = primes.Length;

            var pairs = new BooleanMatrix(length, length);

            for (int i = 0; i < length; i++)
            {
                long alpha = primes[i];
                long alphaMultiplier = MathUtilities.Pow(10, (long)Math.Log10(alpha) + 1);
                for (int j = i + 1; j < length; j++)
                {
                    long bravo = primes[j];
                    if (PrimeGenerator.Instance.IsPrime(bravo * alphaMultiplier + alpha) &&
                        PrimeGenerator.Instance.IsPrime(alpha * MathUtilities.Pow(10, (long)Math.Log10(bravo) + 1) + bravo))
                    {
                        pairs[i, j] = true;
                        pairs[j, i] = true;
                    }
                }
            }

            return GetResults(pairs, 5, ImmutableSequence<int>.Empty)
                .Select(g => g
                    .Select(p => primes[p])
                    .Sum())
                .Min();
        }

        public IEnumerable<IEnumerable<int>> GetResults(BooleanMatrix pairs, int count, ImmutableSequence<int> current)
        {
            int length = pairs.Width;

            int start;
            if (!current.HasValue)
            {
                start = 0;
            }
            else
            {
                start = current.First();
            }

            var results = new Range(start, length)
                .Where(i => current
                    .All(x => pairs[x, i]));

            if (count == 1)
            {
                return results
                    .Select(i => new ImmutableSequence<int>(i, current));
            }
            else
            {
                return results
                    .SelectMany(i => GetResults(pairs, count - 1, new ImmutableSequence<int>(i, current)));
            }
        }
    }
}
