using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(46,
        @"It was proposed by Christian Goldbach that every odd composite number
        can be written as the sum of a prime and twice a square.

        9 = 7 + 2*1^2
        15 = 7 + 2*2^2
        21 = 3 + 2*3^2
        25 = 7 + 2*3^2
        27 = 19 + 2*2^2
        33 = 31 + 2*1^2

        It turns out that the conjecture was false.

        What is the smallest odd composite that cannot be written as the sum of a prime and twice a square?")]
    public class Problem046 : BaseProblem
    {
        public override object CalculateResult()
        {
            return GetComposites()
                .Where(n => (n % 2) == 1)
                .First(n =>
                {
                    long sqrt = (long)Math.Sqrt(n / 2);
                    for (int i = 1; i <= sqrt; i++)
                    {
                        if (PrimeGenerator.Instance.IsPrime(n - (2 * i * i)))
                        {
                            return false;
                        }
                    }
                    return true;
                });
        }

        public IEnumerable<long> GetComposites()
        {
            long n = 1;
            foreach (long prime in PrimeGenerator.Instance)
            {
                for (long i = n + 1; i < prime; i++)
                {
                    yield return i;
                }
                n = prime;
            }
        }
    }
}
