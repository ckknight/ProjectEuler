using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ckknight.ProjectEuler;

namespace Ckknight.ProjectEulerTest
{
    [TestClass]
    public class FractionTest
    {
        [TestMethod]
        public void Zero_ToString_is_0()
        {
            Assert.AreEqual("0", Fraction.Zero.ToString());
        }

        [TestMethod]
        public void One_ToString_is_1()
        {
            Assert.AreEqual("1", Fraction.One.ToString());
        }

        [TestMethod]
        public void Negative_One_ToString_is_neg_1()
        {
            Assert.AreEqual("-1", (-Fraction.One).ToString());
        }

        [TestMethod]
        public void NaN_ToString_is_NaN()
        {
            Assert.AreEqual("NaN", Fraction.NaN.ToString());
        }

        [TestMethod]
        public void PositiveInfinity_ToString_is_Infinity()
        {
            Assert.AreEqual("Infinity", Fraction.PositiveInfinity.ToString());
        }

        [TestMethod]
        public void NegativeInfinity_ToString_is_negInfinity()
        {
            Assert.AreEqual("-Infinity", Fraction.NegativeInfinity.ToString());
        }

        [TestMethod]
        public void Half_ToString_is_1_2()
        {
            Assert.AreEqual("1/2", new Fraction(1, 2).ToString());
        }

        [TestMethod]
        public void Two_Thirds_ToString_is_2_3()
        {
            Assert.AreEqual("2/3", new Fraction(2, 3).ToString());
        }

        [TestMethod]
        public void AutoReduction_ToString()
        {
            Assert.AreEqual("1/2", new Fraction(5, 10).ToString());
        }

        [TestMethod]
        public void AutoReduction_Equality()
        {
            Assert.AreEqual(new Fraction(2, 4), new Fraction(5, 10));
        }

        [TestMethod]
        public void Addition()
        {
            Assert.AreEqual(new Fraction(22, 15), new Fraction(2, 3) + new Fraction(4, 5));
        }

        [TestMethod]
        public void Addition_NaN()
        {
            Assert.AreEqual(Fraction.NaN, new Fraction(2, 3) + Fraction.NaN);
        }

        [TestMethod]
        public void Addition_PositiveInfinity()
        {
            Assert.AreEqual(Fraction.PositiveInfinity, new Fraction(2, 3) + Fraction.PositiveInfinity);
        }

        [TestMethod]
        public void Addition_NegativeInfinity()
        {
            Assert.AreEqual(Fraction.NegativeInfinity, new Fraction(2, 3) + Fraction.NegativeInfinity);
        }

        [TestMethod]
        public void Addition_NaN_NaN()
        {
            Assert.AreEqual(Fraction.NaN, Fraction.NaN + Fraction.NaN);
        }

        [TestMethod]
        public void Addition_NaN_PositiveInfinity()
        {
            Assert.AreEqual(Fraction.NaN, Fraction.NaN + Fraction.PositiveInfinity);
        }

        [TestMethod]
        public void Addition_NaN_NegativeInfinity()
        {
            Assert.AreEqual(Fraction.NaN, Fraction.NaN + Fraction.NegativeInfinity);
        }

        [TestMethod]
        public void Addition_PositiveInfinity_PositiveInfinity()
        {
            Assert.AreEqual(Fraction.PositiveInfinity, Fraction.PositiveInfinity + Fraction.PositiveInfinity);
        }

        [TestMethod]
        public void Addition_PositiveInfinity_NegativeInfinity()
        {
            Assert.AreEqual(Fraction.NaN, Fraction.PositiveInfinity + Fraction.NegativeInfinity);
        }

        [TestMethod]
        public void Addition_NegativeInfinity_NegativeInfinity()
        {
            Assert.AreEqual(Fraction.NegativeInfinity, Fraction.NegativeInfinity + Fraction.NegativeInfinity);
        }

        [TestMethod]
        public void Subtraction()
        {
            Assert.AreEqual(new Fraction(-2, 15), new Fraction(2, 3) - new Fraction(4, 5));
        }

        [TestMethod]
        public void Multiplication()
        {
            Assert.AreEqual(new Fraction(8, 15), new Fraction(2, 3) * new Fraction(4, 5));
        }

        [TestMethod]
        public void Multiplication_NaN()
        {
            Assert.AreEqual(Fraction.NaN, new Fraction(2, 3) * Fraction.NaN);
        }

        [TestMethod]
        public void Multiplication_PositiveInfinity()
        {
            Assert.AreEqual(Fraction.PositiveInfinity, new Fraction(2, 3) * Fraction.PositiveInfinity);
        }

        [TestMethod]
        public void Multiplication_NegativeInfinity()
        {
            Assert.AreEqual(Fraction.NegativeInfinity, new Fraction(2, 3) * Fraction.NegativeInfinity);
        }

        [TestMethod]
        public void Multiplication_Zero_NaN()
        {
            Assert.AreEqual(Fraction.NaN, Fraction.Zero * Fraction.NaN);
        }

        [TestMethod]
        public void Multiplication_Zero_PositiveInfinity()
        {
            Assert.AreEqual(Fraction.NaN, Fraction.Zero * Fraction.PositiveInfinity);
        }

        [TestMethod]
        public void Multiplication_Zero_NegativeInfinity()
        {
            Assert.AreEqual(Fraction.NaN, Fraction.Zero * Fraction.NegativeInfinity);
        }

        [TestMethod]
        public void Multiplication_NaN_NaN()
        {
            Assert.AreEqual(Fraction.NaN, Fraction.NaN * Fraction.NaN);
        }

        [TestMethod]
        public void Multiplication_NaN_PositiveInfinity()
        {
            Assert.AreEqual(Fraction.NaN, Fraction.NaN * Fraction.PositiveInfinity);
        }

        [TestMethod]
        public void Multiplication_NaN_NegativeInfinity()
        {
            Assert.AreEqual(Fraction.NaN, Fraction.NaN * Fraction.NegativeInfinity);
        }

        [TestMethod]
        public void Multiplication_PositiveInfinity_PositiveInfinity()
        {
            Assert.AreEqual(Fraction.PositiveInfinity, Fraction.PositiveInfinity * Fraction.PositiveInfinity);
        }

        [TestMethod]
        public void Multiplication_PositiveInfinity_NegativeInfinity()
        {
            Assert.AreEqual(Fraction.NegativeInfinity, Fraction.PositiveInfinity * Fraction.NegativeInfinity);
        }

        [TestMethod]
        public void Multiplication_NegativeInfinity_NegativeInfinity()
        {
            Assert.AreEqual(Fraction.PositiveInfinity, Fraction.NegativeInfinity * Fraction.NegativeInfinity);
        }

        [TestMethod]
        public void Division()
        {
            Assert.AreEqual(new Fraction(5, 6), new Fraction(2, 3) / new Fraction(4, 5));
        }

        [TestMethod]
        public void Modulo_1()
        {
            Assert.AreEqual(new Fraction(2, 3), new Fraction(2, 3) % new Fraction(4, 5));
        }

        [TestMethod]
        public void Modulo_2()
        {
            Assert.AreEqual(new Fraction(1, 6), new Fraction(2, 3) % new Fraction(1, 2));
        }

        [TestMethod]
        public void DivRem_Zero_Zero()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NaN, Fraction.DivRem(Fraction.Zero, Fraction.Zero, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_Zero_One()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.Zero, Fraction.DivRem(Fraction.Zero, Fraction.One, out remainder));
            Assert.AreEqual(Fraction.Zero, remainder);
        }

        [TestMethod]
        public void DivRem_Zero_PositiveInfinity()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.Zero, Fraction.DivRem(Fraction.Zero, Fraction.PositiveInfinity, out remainder));
            Assert.AreEqual(Fraction.Zero, remainder);
        }

        [TestMethod]
        public void DivRem_Zero_NegativeInfinity()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.Zero, Fraction.DivRem(Fraction.Zero, Fraction.NegativeInfinity, out remainder));
            Assert.AreEqual(Fraction.Zero, remainder);
        }

        [TestMethod]
        public void DivRem_Zero_NaN()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NaN, Fraction.DivRem(Fraction.Zero, Fraction.NaN, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_One_Zero()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.PositiveInfinity, Fraction.DivRem(Fraction.One, Fraction.Zero, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_One_One()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.One, Fraction.DivRem(Fraction.One, Fraction.One, out remainder));
            Assert.AreEqual(Fraction.Zero, remainder);
        }

        [TestMethod]
        public void DivRem_One_Two()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.Zero, Fraction.DivRem(Fraction.One, new Fraction(2), out remainder));
            Assert.AreEqual(Fraction.One, remainder);
        }

        [TestMethod]
        public void DivRem_One_PositiveInfinity()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.Zero, Fraction.DivRem(Fraction.One, Fraction.PositiveInfinity, out remainder));
            Assert.AreEqual(Fraction.One, remainder);
        }

        [TestMethod]
        public void DivRem_One_NegativeInfinity()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.Zero, Fraction.DivRem(Fraction.One, Fraction.NegativeInfinity, out remainder));
            Assert.AreEqual(Fraction.One, remainder);
        }

        [TestMethod]
        public void DivRem_One_NaN()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NaN, Fraction.DivRem(Fraction.One, Fraction.NaN, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_PositiveInfinity_Zero()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.PositiveInfinity, Fraction.DivRem(Fraction.PositiveInfinity, Fraction.Zero, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_PositiveInfinity_One()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.PositiveInfinity, Fraction.DivRem(Fraction.PositiveInfinity, Fraction.One, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_PositiveInfinity_PositiveInfinity()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NaN, Fraction.DivRem(Fraction.PositiveInfinity, Fraction.PositiveInfinity, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_PositiveInfinity_NegativeInfinity()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NaN, Fraction.DivRem(Fraction.PositiveInfinity, Fraction.NegativeInfinity, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_PositiveInfinity_NaN()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NaN, Fraction.DivRem(Fraction.PositiveInfinity, Fraction.NaN, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_NegativeInfinity_Zero()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NegativeInfinity, Fraction.DivRem(Fraction.NegativeInfinity, Fraction.Zero, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_NegativeInfinity_One()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NegativeInfinity, Fraction.DivRem(Fraction.NegativeInfinity, Fraction.One, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_NegativeInfinity_PositiveInfinity()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NaN, Fraction.DivRem(Fraction.NegativeInfinity, Fraction.PositiveInfinity, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_NegativeInfinity_NegativeInfinity()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NaN, Fraction.DivRem(Fraction.NegativeInfinity, Fraction.NegativeInfinity, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_NegativeInfinity_NaN()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NaN, Fraction.DivRem(Fraction.NegativeInfinity, Fraction.NaN, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_NaN_Zero()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NaN, Fraction.DivRem(Fraction.NaN, Fraction.Zero, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_NaN_One()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NaN, Fraction.DivRem(Fraction.NaN, Fraction.One, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_NaN_PositiveInfinity()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NaN, Fraction.DivRem(Fraction.NaN, Fraction.PositiveInfinity, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_NaN_NegativeInfinity()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NaN, Fraction.DivRem(Fraction.NaN, Fraction.NegativeInfinity, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void DivRem_NaN_NaN()
        {
            Fraction remainder;
            Assert.AreEqual(Fraction.NaN, Fraction.DivRem(Fraction.NaN, Fraction.NaN, out remainder));
            Assert.AreEqual(Fraction.NaN, remainder);
        }

        [TestMethod]
        public void Floor_1()
        {
            Assert.AreEqual(new Fraction(5), Fraction.Floor(new Fraction(26, 5)));
        }

        [TestMethod]
        public void Floor_2()
        {
            Assert.AreEqual(new Fraction(5), Fraction.Floor(new Fraction(25, 5)));
        }

        [TestMethod]
        public void Floor_3()
        {
            Assert.AreEqual(Fraction.Zero, Fraction.Floor(new Fraction(5, 6)));
        }

        [TestMethod]
        public void Floor_NaN()
        {
            Assert.AreEqual(Fraction.NaN, Fraction.Floor(Fraction.NaN));
        }

        [TestMethod]
        public void Floor_PositiveInfinity()
        {
            Assert.AreEqual(Fraction.PositiveInfinity, Fraction.Floor(Fraction.PositiveInfinity));
        }

        [TestMethod]
        public void Floor_NegativeInfinity()
        {
            Assert.AreEqual(Fraction.NegativeInfinity, Fraction.Floor(Fraction.NegativeInfinity));
        }

        [TestMethod]
        public void Ceiling_1()
        {
            Assert.AreEqual(new Fraction(6), Fraction.Ceiling(new Fraction(26, 5)));
        }

        [TestMethod]
        public void Ceiling_2()
        {
            Assert.AreEqual(new Fraction(5), Fraction.Ceiling(new Fraction(25, 5)));
        }

        [TestMethod]
        public void Ceiling_3()
        {
            Assert.AreEqual(new Fraction(1), Fraction.Ceiling(new Fraction(5, 6)));
        }

        [TestMethod]
        public void Ceiling_NaN()
        {
            Assert.AreEqual(Fraction.NaN, Fraction.Ceiling(Fraction.NaN));
        }

        [TestMethod]
        public void Ceiling_PositiveInfinity()
        {
            Assert.AreEqual(Fraction.PositiveInfinity, Fraction.Ceiling(Fraction.PositiveInfinity));
        }

        [TestMethod]
        public void Ceiling_NegativeInfinity()
        {
            Assert.AreEqual(Fraction.NegativeInfinity, Fraction.Ceiling(Fraction.NegativeInfinity));
        }

        [TestMethod]
        public void Pow_0()
        {
            Assert.AreEqual(new Fraction(1), Fraction.Pow(new Fraction(2, 3), 0));
        }

        [TestMethod]
        public void Pow_1()
        {
            Assert.AreEqual(new Fraction(2, 3), Fraction.Pow(new Fraction(2, 3), 1));
        }

        [TestMethod]
        public void Pow_2()
        {
            Assert.AreEqual(new Fraction(4, 9), Fraction.Pow(new Fraction(2, 3), 2));
        }

        [TestMethod]
        public void Pow_3()
        {
            Assert.AreEqual(new Fraction(8, 27), Fraction.Pow(new Fraction(2, 3), 3));
        }

        [TestMethod]
        public void Abs_Positive()
        {
            Assert.AreEqual(new Fraction(8, 27), Fraction.Abs(new Fraction(8, 27)));
        }

        [TestMethod]
        public void Abs_Negative()
        {
            Assert.AreEqual(new Fraction(8, 27), Fraction.Abs(new Fraction(-8, 27)));
        }

        [TestMethod]
        public void Abs_PositiveInfinity()
        {
            Assert.AreEqual(Fraction.PositiveInfinity, Fraction.Abs(Fraction.PositiveInfinity));
        }

        [TestMethod]
        public void Abs_NegativeInfinity()
        {
            Assert.AreEqual(Fraction.PositiveInfinity, Fraction.Abs(Fraction.NegativeInfinity));
        }
    }
}
