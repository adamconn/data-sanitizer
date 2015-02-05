using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.Strategy.Sanitizer.Generators.Text;

namespace Sitecore.Strategy.Sanitizer.Tests
{
    [TestClass]
    public class CompoundValuesGeneratorTests
    {
        [TestMethod]
        public void MissingKeyTest()
        {
            var context = new CompoundValuesGeneratorContext();
            context.Formats.Add("name", values =>
            {
                var first = values.ContainsKey("first") ? values["first"] : string.Empty;
                var last = values.ContainsKey("last") ? values["last"] : string.Empty;
                return string.Format("{0} {1}", first, last);
            });
            var sourcesFirst = new KeyValuePair<string, IEnumerable<string>>("first", new string[] { "Ann" });

            var generator = new CompoundValuesGenerator(context);
            generator.AddValues(sourcesFirst);
            var formattedValues = generator.NextValue();
            Assert.AreEqual(1, formattedValues.Values.Count);
            Assert.AreEqual("Ann ", formattedValues["name"]);
        }

        [TestMethod]
        public void TwoFormatsTest()
        {
            var context = new CompoundValuesGeneratorContext();
            context.Formats.Add("name1", values => string.Format("{0} {1}", values["first"], values["last"]));
            context.Formats.Add("name2", values => string.Format("{0}, {1}", values["last"], values["first"]));

            var sourcesFirst = new KeyValuePair<string, IEnumerable<string>>("first", new string[] { "Ann" });
            var sourcesLast = new KeyValuePair<string, IEnumerable<string>>("last", new string[] { "Johnson" });

            var generator = new CompoundValuesGenerator(context);
            generator.AddValues(sourcesFirst, sourcesLast);
            var formattedValues = generator.NextValue();
            Assert.AreEqual(2, formattedValues.Values.Count);
            Assert.AreEqual("Ann Johnson", formattedValues["name1"]);
            Assert.AreEqual("Johnson, Ann", formattedValues["name2"]);
        }

        [TestMethod]
        public void EmailTest()
        {
            var context = new CompoundValuesGeneratorContext();
            context.Formats.Add("email", values =>
            {
                return string.Format("{0}.{1}@{2}", values["first"].ToLower(), values["last"].ToLower(), values["domain"]);
            });

            var sourcesFirst = new KeyValuePair<string, IEnumerable<string>>("first", new string[] { "Ann" });
            var sourcesLast = new KeyValuePair<string, IEnumerable<string>>("last", new string[] { "Johnson" });
            var sourcesDomain = new KeyValuePair<string, IEnumerable<string>>("domain", new string[] { "gmail.com" });

            var generator = new CompoundValuesGenerator(context);
            generator.AddValues(sourcesFirst, sourcesLast, sourcesDomain);
            var formattedValues = generator.NextValue();
            Assert.AreEqual(1, formattedValues.Values.Count);
            Assert.AreEqual("ann.johnson@gmail.com", formattedValues["email"]);
        }
    }
}
