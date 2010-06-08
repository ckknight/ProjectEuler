using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ckknight.ProjectEuler;
using Ckknight.ProjectEuler.Collections;
using System.Numerics;

namespace Ckknight.ProjectEulerTest
{
    [TestClass]
    public class ContinuedFractionTest
    {
        [TestMethod]
        public void ToContinuedFraction()
        {
            var result = ContinuedFraction.FromFraction(new Fraction(3245, 1000));
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Floor);
            Assert.IsTrue(result.Quotients.SequenceEqual(new long[] { 4, 12, 4 }));
            Assert.AreEqual("[3; 4, 12, 4]", result.ToString());
        }

        [TestMethod]
        public void BigToContinuedFraction()
        {
            var result = BigContinuedFraction.FromBigFraction(new BigFraction(3245, 1000));
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Floor);
            Assert.IsTrue(result.Quotients.SequenceEqual(new BigInteger[] { 4, 12, 4 }));
            Assert.AreEqual("[3; 4, 12, 4]", result.ToString());
        }

        [TestMethod]
        public void ContinuedFractionToFractions()
        {
            var cont = new ContinuedFraction(3, new long[] { 4, 12, 4 });

            var result = cont.GetFractions();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.SequenceEqual(new[] { new Fraction(3), new Fraction(13, 4), new Fraction(159, 49), new Fraction(649, 200) }));
        }

        [TestMethod]
        public void ContinuedFractionToBigFractions()
        {
            var cont = new BigContinuedFraction(3, new BigInteger[] { 4, 12, 4 });

            var result = cont.GetBigFractions();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.SequenceEqual(new[] { new BigFraction(3), new BigFraction(13, 4), new BigFraction(159, 49), new BigFraction(649, 200) }));
        }

        [TestMethod]
        public void ContinuedFractionToBigFractions_Sqrt2()
        {
            var cont = new BigContinuedFraction(1, CollectionUtilities.Repeat(new BigInteger(2)));
            Assert.AreEqual("[1; 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, ...]", cont.ToString());

            var result = cont.GetBigFractions();
            Assert.IsNotNull(result);
            Assert.AreEqual(0.5, BigFraction.Log(result.ElementAt(200), 2.0));
        }

        [TestMethod]
        public void ContinuedFractionToBigFractions_Sqrt2_Alternate()
        {
            var cont = new BigContinuedFraction(1, Enumerable.Empty<BigInteger>(), new BigInteger[] { 2 });
            Assert.AreEqual("[1; (2)]", cont.ToString());

            var result = cont.GetBigFractions();
            Assert.IsNotNull(result);
            Assert.AreEqual(0.5, BigFraction.Log(result.ElementAt(200), 2.0));
        }

        [TestMethod]
        public void Sqrt()
        {
            for (int i = 2; i < 100; i++)
            {
                var cont = BigContinuedFraction.Sqrt(new BigInteger(i));

                var result = cont.GetBigFractions()
                    .Take(200)
                    .ToArray();
                Assert.IsNotNull(result);
                double sqrt = (double)result.Last();
                double value = Math.Round(sqrt * sqrt, 10);
                Assert.AreEqual((double)i, value);
            }
        }
    }
}
