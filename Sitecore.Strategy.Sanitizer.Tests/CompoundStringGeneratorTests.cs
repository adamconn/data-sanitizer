using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.Strategy.Sanitizer.Generators.Text;

namespace Sitecore.Strategy.Sanitizer.Tests
{
    [TestClass]
    public class CompoundStringGeneratorTests
    {
        public static string[] FIRST_NAMES_MEN_1 = new string[] { "Adam", "Brian", "Charles", "David", "Eric", "Fred", "George", "Henry", "Ian", "Jeff", "Keith" };
        public static string[] FIRST_NAMES_MEN_2 = new string[] { "Aaron", "Bruce", "Chip", "Donald", "Errol", "Finn", "Gregory", "Horace", "Ivan", "Joseph", "Konrad" };
        public static string[] FIRST_NAMES_WOMEN_1 = new string[] { "Ann", "Beth", "Christine", "Danielle", "Erin", "Fanny", "Gina", "Henrietta", "Iris", "Jennnifer", "Kelley" };
        public static string[] LAST_NAMES_1 = new string[] { "Abrams", "Beverly", "Chesterton", "Davison", "Englund", "Franks", "Geary", "Harris", "Ickles", "Jamison", "Kerry" };
        public static string[] LAST_NAMES_2 = new string[] { "Andersen", "Brown", "Clark", "Davis", "Edwards", "Flores", "Green", "Heller", "Ingram", "Juarez", "Kramer" };

        [TestMethod]
        public void OneFormatTest()
        {
            var context = new CompoundStringGeneratorContext();
            context.Formats.Add("{first} {last1}-{last2}");
            var generator = new CompoundStringGenerator(context);
            var values = new Dictionary<string, IEnumerable<string>>();
            values["first"] = FIRST_NAMES_MEN_1;
            values["last1"] = LAST_NAMES_1;
            values["last2"] = LAST_NAMES_1;
            generator.AddValues(values.ToArray());
            for (var i = 0; i < 10; i++)
            {
                var value = generator.NextValue();
                Assert.IsFalse(string.IsNullOrEmpty(value));
                Assert.IsFalse(FIRST_NAMES_MEN_1.Contains(value));
                Assert.IsFalse(LAST_NAMES_1.Contains(value));
                var fullName = value.Split(' ');
                Assert.AreEqual(2, fullName.Length);
                Assert.IsTrue(FIRST_NAMES_MEN_1.Contains(fullName[0]));
                var lastName = fullName[1].Split('-');
                Assert.IsTrue(LAST_NAMES_1.Contains(lastName[0]));
                Assert.IsTrue(LAST_NAMES_1.Contains(lastName[1]));
            }
        }

        [TestMethod]
        public void FormatterTest()
        {
            var context = new CompoundStringGeneratorContext();
            context.Formats.Add("{first} {last}");
            context.ValueFormatters.Add("first", s => s.ToUpper());
            var generator = new CompoundStringGenerator(context);
            var values = new Dictionary<string, IEnumerable<string>>();
            values["first"] = FIRST_NAMES_MEN_1;
            values["last"] = LAST_NAMES_1;
            generator.AddValues(values.ToArray());
            for (var i = 0; i < 10; i++)
            {
                var value = generator.NextValue();
                Assert.IsFalse(string.IsNullOrEmpty(value));
                Assert.IsFalse(FIRST_NAMES_MEN_1.Contains(value));
                Assert.IsFalse(LAST_NAMES_1.Contains(value));
                var fullName = value.Split(' ');
                Assert.AreEqual(2, fullName.Length);
                Assert.IsTrue(FIRST_NAMES_MEN_1.Any(name => name.ToUpper() == fullName[0]));
            }
        }

        [TestMethod]
        public void TwoFormatsTest()
        {
            var context = new CompoundStringGeneratorContext();
            context.Formats.Add("{a}");
            context.Formats.Add("{a}-{b}");
            var generator = new CompoundStringGenerator(context);
            var values = new Dictionary<string, IEnumerable<string>>();
            values["a"] = LAST_NAMES_1;
            values["b"] = LAST_NAMES_1;
            generator.AddValues(values.ToArray());
            for (var i = 0; i < 10; i++)
            {
                var value = generator.NextValue();
                Assert.IsFalse(string.IsNullOrEmpty(value));
                var split = value.Split('-');
                Assert.IsTrue(split.Length == 1 || split.Length == 2);
                if (split.Length == 1)
                {
                    Assert.IsTrue(LAST_NAMES_1.Contains(value));
                }
                else
                {
                    Assert.IsFalse(LAST_NAMES_1.Contains(value));
                    Assert.IsTrue(LAST_NAMES_1.Contains(split[0]));
                    Assert.IsTrue(LAST_NAMES_1.Contains(split[1]));
                }
            }
        }
    }
}
