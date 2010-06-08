using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(50,
        @"The prime 41, can be written as the sum of six consecutive primes:

                41 = 2 + 3 + 5 + 7 + 11 + 13

        This is the longest sum of consecutive primes that adds to a prime
        below one-hundred.

        The longest sum of consecutive primes below one-thousand that adds to a
        prime, contains 21 terms, and is equal to 953.

        Which prime, below one-million, can be written as the sum of the most
        consecutive primes?")]
    public class Problem050 : BaseProblem
    {
        public override object CalculateResult()
        {
            int maximum = 1000000;

            var primes = PrimeGenerator.Instance
                .TakeWhile(p => p < maximum)
                .ToArray();

            return primes
                .SelectMany((p, i) => primes
                    .Skip(i)
                    .SelectWithAggregate(new { Count = 0, Sum = 0L }, (x, v) => new { Count = x.Count + 1, Sum = x.Sum + v })
                    .TakeWhile(x => x.Sum < maximum))
                .Where(x => PrimeGenerator.Instance.IsPrime(x.Sum))
                .Aggregate((a, b) => a.Count > b.Count ? a : b)
                .Sum;
        }
    }
}
