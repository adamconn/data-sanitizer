using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.Strategy.Sanitizer.Generators.Text;

namespace Sitecore.Strategy.Sanitizer.Tests
{
    [TestClass]
    public class StringGeneratorTests
    {
        public static string[] FIRST_NAMES_1 = new string[] { "Adam", "Brian", "Charles", "David", "Eric", "Fred", "George", "Henry", "Ian", "Jeff", "Keith" };
        public static string[] FIRST_NAMES_2 = new string[] { "Ann", "Beth", "Christine", "Danielle", "Erin", "Fanny", "Gina", "Henrietta", "Iris", "Jennnifer", "Kelley" };

        public void Test()
        {
            var places = new string[] {"Seattle", "Portland", "San Francisco", "Los Angeles", "San Diego"};
            var generator = new StringGenerator();
            var randomPlace = generator.NextValue();
        }
        [TestMethod]
        public void StringGeneratorTest()
        {
            var generator = new StringGenerator();
            generator.AddValues(FIRST_NAMES_1);
            for (var i = 0; i < 10; i++)
            {
                var value = generator.NextValue();
                Assert.IsFalse(string.IsNullOrEmpty(value));
                Assert.IsTrue(FIRST_NAMES_1.Contains(value));
            }
        }
        [TestMethod]
        public void StringGeneratorAddValuesTest()
        {
            var generator = new StringGenerator();
            generator.AddValues(FIRST_NAMES_1);
            generator.AddValues(FIRST_NAMES_2);
            for (var i = 0; i < 10; i++)
            {
                var value = generator.NextValue();
                Assert.IsFalse(string.IsNullOrEmpty(value));
                Assert.IsTrue(FIRST_NAMES_1.Contains(value) || FIRST_NAMES_2.Contains(value));
            }
        }
        [TestMethod]
        public void StringGeneratorRemoveValuesTest()
        {
            var generator = new StringGenerator();
            generator.AddValues(FIRST_NAMES_1);
            generator.AddValues(FIRST_NAMES_2);
            generator.RemoveValues(FIRST_NAMES_2);
            for (var i = 0; i < 10; i++)
            {
                var value = generator.NextValue();
                Assert.IsFalse(string.IsNullOrEmpty(value));
                Assert.IsTrue(FIRST_NAMES_1.Contains(value));
                Assert.IsFalse(FIRST_NAMES_2.Contains(value));
            }
        }
    }
}
