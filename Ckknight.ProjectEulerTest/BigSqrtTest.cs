using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ckknight.ProjectEuler;
using System.Numerics;

namespace Ckknight.ProjectEulerTest
{
    [TestClass]
    public class BigSqrtTest
    {
        [TestMethod]
        public void Zero()
        {
            Assert.AreEqual(BigInteger.Zero, MathUtilities.BigSqrt(BigInteger.Zero));
        }

        [TestMethod]
        public void One()
        {
            Assert.AreEqual(BigInteger.One, MathUtilities.BigSqrt(BigInteger.One));
            Assert.AreEqual(BigInteger.One, MathUtilities.BigSqrt(2));
            Assert.AreEqual(BigInteger.One, MathUtilities.BigSqrt(3));
        }

        [TestMethod]
        public void Two()
        {
            Assert.AreEqual(new BigInteger(2), MathUtilities.BigSqrt(4));
            Assert.AreEqual(new BigInteger(2), MathUtilities.BigSqrt(5));
            Assert.AreEqual(new BigInteger(2), MathUtilities.BigSqrt(6));
            Assert.AreEqual(new BigInteger(2), MathUtilities.BigSqrt(7));
            Assert.AreEqual(new BigInteger(2), MathUtilities.BigSqrt(8));
        }

        [TestMethod]
        public void RandomValues()
        {
            Random random = new Random();
            for (int i = 1; i <= 100; i++)
            {
                byte[] data = new byte[i];
                random.NextBytes(data);
                BigInteger value = new BigInteger(data);
                if (value.Sign < 0)
                {
                    value = BigInteger.Negate(value);
                }
                BigInteger sqrt = MathUtilities.BigSqrt(value);
                
                BigInteger sqrtSquared = BigInteger.Pow(sqrt, 2);
                Assert.IsTrue(sqrtSquared <= value);

                BigInteger sqrtPlusOne = sqrt + BigInteger.One;
                BigInteger sqrtPlusOneSquared = BigInteger.Pow(sqrtPlusOne, 2);
                Assert.IsTrue(sqrtPlusOneSquared > value);
            }
        }
    }
}
