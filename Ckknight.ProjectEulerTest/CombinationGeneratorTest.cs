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
            var generator = new BetterCombinationGenerator<string>(new[] { "a", "b", "c", "d", "e" }, 2);
            var results = generator.ToArray();
        }
    }
}
