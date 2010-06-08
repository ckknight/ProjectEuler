using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEulerTest
{
    [TestClass]
    public class CombinationGeneratorTest
    {
        [TestMethod]
        public void Combination()
        {
            var generator = new CombinationGenerator<string>(new[] { "a", "b", "c", "d", "e" }, 2);
            var results = generator.ToArray();
            string[][] expected = {
                                      new[] { "a", "b" },
                                      new[] { "a", "c" },
                                      new[] { "a", "d" },
                                      new[] { "a", "e" },
                                      new[] { "b", "c" },
                                      new[] { "b", "d" },
                                      new[] { "b", "e" },
                                      new[] { "c", "d" },
                                      new[] { "c", "e" },
                                      new[] { "d", "e" }
                                  };

            Assert.AreEqual(expected.Length, results.Length);
            Assert.IsTrue(expected.Select((x, i) => x.SequenceEqual(results[i])).All(x => x));
        }
    }
}
