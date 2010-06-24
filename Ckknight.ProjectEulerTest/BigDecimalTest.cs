using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Ckknight.ProjectEuler;
using Ckknight.ProjectEuler.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace Ckknight.ProjectEulerTest
{
    [TestClass]
    public class BigDecimalTest
    {
        [TestMethod]
        public void Equality()
        {
            Assert.AreEqual(new BigDecimal(1234, -2), new BigDecimal(1234, -2));
        }

        [TestMethod]
        public void Inequality()
        {
            Assert.AreNotEqual(new BigDecimal(1234, -1), new BigDecimal(1234, -2));
            Assert.AreNotEqual(new BigDecimal(1234, -3), new BigDecimal(1234, -2));
            Assert.AreNotEqual(new BigDecimal(1233, -2), new BigDecimal(1234, -2));
        }

        [TestMethod]
        public void CompareTo()
        {
            BigInteger hugeInteger = BigInteger.Pow(long.MaxValue, 10);
            BigDecimal hugeNegative = new BigDecimal(-hugeInteger, hugeInteger);
            BigDecimal veryLargeNegative = new BigDecimal(long.MinValue, long.MaxValue);
            BigDecimal largeNegative = new BigDecimal(long.MinValue, int.MaxValue);
            BigDecimal mediumNegative = new BigDecimal(long.MinValue);
            BigDecimal mediumNegativePlusOne = new BigDecimal(long.MinValue + 1);
            BigDecimal zero = BigDecimal.Zero;
            BigDecimal nine = new BigDecimal(9);
            BigDecimal ten = new BigDecimal(10);
            BigDecimal tenPointOne = new BigDecimal(101, -1);
            BigDecimal eleven = new BigDecimal(11);
            BigDecimal nineteen = new BigDecimal(19);
            BigDecimal nineteenPointFive = new BigDecimal(195, -1);
            BigDecimal twenty = new BigDecimal(20);
            BigDecimal twentyFive = new BigDecimal(25);
            BigDecimal thirty = new BigDecimal(30);
            BigDecimal mediumNegativeMinusOne = new BigDecimal(long.MaxValue - 1);
            BigDecimal mediumPositive = new BigDecimal(long.MaxValue);
            BigDecimal largePositive = new BigDecimal(long.MaxValue, int.MaxValue);
            BigDecimal veryLargePositive = new BigDecimal(long.MaxValue, long.MaxValue);
            BigDecimal hugePositive = new BigDecimal(hugeInteger, hugeInteger);

            var expected = new[]
            {
                hugeNegative,
                veryLargeNegative,
                largeNegative,
                mediumNegative,
                mediumNegativePlusOne,
                zero,
                nine,
                ten,
                tenPointOne,
                eleven,
                nineteen,
                nineteenPointFive,
                twenty,
                twentyFive,
                thirty,
                mediumNegativeMinusOne,
                mediumPositive,
                largePositive,
                veryLargePositive,
                hugePositive
            };

            for (int i = 0; i < expected.Length; i++)
            {
                BigDecimal alpha = expected[i];
                Assert.AreEqual(alpha, alpha);
                Assert.AreEqual(0, alpha.CompareTo(alpha));
                for (int j = i + 1; j < expected.Length; j++)
                {
                    BigDecimal bravo = expected[j];
                    Assert.AreEqual(-1, alpha.CompareTo(bravo));
                    Assert.AreEqual(1, bravo.CompareTo(alpha));
                }
            }

            Assert.IsTrue(expected.Shuffle().OrderBy(x => x).SequenceEqual(expected));
        }

        [TestMethod]
        public void DecimalConversion()
        {
            BigDecimal value = 1234.5678m;
            Assert.AreEqual(new BigDecimal(12345678, -4), value);
        }

        [TestMethod]
        public void Addition()
        {
            decimal value = 1234.5678m;
            BigDecimal bigValue = value;
            Assert.AreEqual(bigValue, bigValue + BigDecimal.Zero);
            Assert.AreEqual(bigValue, BigDecimal.Zero + bigValue);
            Assert.AreEqual((BigDecimal)(value + 1), bigValue + BigDecimal.One);
            Assert.AreEqual((BigDecimal)(value + 1), BigDecimal.One + bigValue);
            Assert.AreEqual((BigDecimal)(value + 0.00000001m), bigValue + 0.00000001m);
            Assert.AreEqual((BigDecimal)(value + 0.00000001m), 0.00000001m + bigValue);
            Assert.AreEqual((BigDecimal)(value * 2), bigValue + bigValue);
        }

        [TestMethod]
        public void Subtraction()
        {
            decimal value = 1234.5678m;
            BigDecimal bigValue = value;
            Assert.AreEqual(bigValue, bigValue - BigDecimal.Zero);
            Assert.AreEqual(-bigValue, BigDecimal.Zero - bigValue);
            Assert.AreEqual((BigDecimal)(value - 1), bigValue - BigDecimal.One);
            Assert.AreEqual((BigDecimal)(1 - value), BigDecimal.One - bigValue);
            Assert.AreEqual((BigDecimal)(value - 0.00000001m), bigValue - 0.00000001m);
            Assert.AreEqual((BigDecimal)(0.00000001m - value), 0.00000001m - bigValue);
            Assert.AreEqual(BigDecimal.Zero, bigValue - bigValue);
        }

        [TestMethod]
        public void Multiplication()
        {
            decimal value = 1234.5678m;
            BigDecimal bigValue = value;
            Assert.AreEqual(BigDecimal.Zero, bigValue * BigDecimal.Zero);
            Assert.AreEqual(BigDecimal.Zero, BigDecimal.Zero * bigValue);
            Assert.AreEqual(bigValue, bigValue * BigDecimal.One);
            Assert.AreEqual(bigValue, BigDecimal.One * bigValue);
            Assert.AreEqual(bigValue + bigValue, bigValue * 2);
            Assert.AreEqual(bigValue + bigValue + bigValue, bigValue * 3);
            Assert.AreEqual(-bigValue, bigValue * -1);
            Assert.AreEqual((BigDecimal)(value * value), bigValue * bigValue);
        }

        [TestMethod]
        public void DivRem()
        {
            Func<BigDecimal, BigDecimal, Tuple<BigDecimal, BigDecimal>> divRem = (a, b) =>
            {
                BigDecimal mod;
                BigDecimal div = BigDecimal.DivRem(a, b, out mod);
                return new Tuple<BigDecimal, BigDecimal>(div, mod);
            };
            Assert.AreEqual(Tuple.Create(new BigDecimal(0), new BigDecimal(1)), divRem(1, 4));
            Assert.AreEqual(Tuple.Create(new BigDecimal(1), new BigDecimal(2)), divRem(5, 3));
            Assert.AreEqual(Tuple.Create(new BigDecimal(5), new BigDecimal(6, -1)), divRem(5.6m, 1));
            Assert.AreEqual(Tuple.Create(new BigDecimal(5), BigDecimal.Zero), divRem(5, 1));
        }

        [TestMethod]
        public void Division()
        {
            Assert.AreEqual(new BigDecimal(50), new BigDecimal(500) / new BigDecimal(10));
            Assert.AreEqual(new BigDecimal(5), new BigDecimal(5) / new BigDecimal(1));
            Assert.AreEqual(new BigDecimal(50), new BigDecimal(50) / new BigDecimal(1));
            Assert.AreEqual(new BigDecimal(25), new BigDecimal(50) / new BigDecimal(2));
            Assert.AreEqual(new BigDecimal(25, -1), new BigDecimal(5) / new BigDecimal(2));
            Assert.AreEqual(1.0 / 3.0, (double)(new BigDecimal(1) / new BigDecimal(3)), 0.000000001);
            Assert.AreEqual(2.0 / 3.0, (double)(new BigDecimal(2) / new BigDecimal(3)), 0.000000001);
            Assert.AreEqual(BigDecimal.One, (new BigDecimal(1) / new BigDecimal(3)) + (new BigDecimal(2) / new BigDecimal(3)));
            Assert.AreEqual(new BigDecimal(10), new BigDecimal(500) / new BigDecimal(50));
            Assert.AreEqual(BigDecimal.One, new BigDecimal(500) / new BigDecimal(500));
        }

        [TestMethod]
        public void ToString()
        {
            Assert.AreEqual("1", BigDecimal.One.ToString());
            Assert.AreEqual("10", new BigDecimal(1, 1).ToString());
            Assert.AreEqual("1000000000", new BigDecimal(1, 9).ToString());
            Assert.AreEqual("0.1", new BigDecimal(1, -1).ToString());
            Assert.AreEqual("0.01", new BigDecimal(1, -2).ToString());
            Assert.AreEqual("12340", new BigDecimal(1234, 1).ToString());
            Assert.AreEqual("1234", new BigDecimal(1234, 0).ToString());
            Assert.AreEqual("123.4", new BigDecimal(1234, -1).ToString());
            Assert.AreEqual("12.34", new BigDecimal(1234, -2).ToString());
            Assert.AreEqual("1.234", new BigDecimal(1234, -3).ToString());
            Assert.AreEqual("0.1234", new BigDecimal(1234, -4).ToString());
            Assert.AreEqual("0.01234", new BigDecimal(1234, -5).ToString());
            Assert.AreEqual("0.001234", new BigDecimal(1234, -6).ToString());
            Assert.AreEqual("0.0001234", new BigDecimal(1234, -7).ToString());
            Assert.AreEqual("0.000000001", new BigDecimal(1, -9).ToString());
        }

        [TestMethod]
        public void Serialization()
        {
            BigDecimal dec = 1234.5678m;

            IFormatter formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, dec);
            stream.Seek(0, SeekOrigin.Begin);

            object obj = formatter.Deserialize(stream);
            stream.Close();
            Assert.IsInstanceOfType(obj, typeof(BigDecimal));
            BigDecimal dec2 = (BigDecimal)obj;
            Assert.AreEqual(dec, dec2);
        }
    }
}
