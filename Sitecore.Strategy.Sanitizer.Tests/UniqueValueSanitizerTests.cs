using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sitecore.Strategy.Sanitizer.Generators;
using Sitecore.Strategy.Sanitizer.Sanitizers;

namespace Sitecore.Strategy.Sanitizer.Tests
{
    [TestClass]
    public class UniqueValueSanitizerTests
    {
        [TestMethod]
        public void SanitizeTest()
        {
            var mock = new Mock<IGenerator<string, string>>();
            mock.Setup(generator => generator.NextValue()).Returns("aaa");
            var sanitizer = new UniqueValueSanitizer<string, string>(mock.Object);
            var val = sanitizer.Sanitize("bbb");
            Assert.AreEqual("aaa", val);
            val = sanitizer.Sanitize("bbb");
            Assert.AreEqual("aaa", val);
        }

        [TestMethod]
        public void ResetTest()
        {
            var mock = new Mock<IGenerator<string, string>>();
            mock.Setup(generator => generator.NextValue()).Returns("aaa");
            var sanitizer = new UniqueValueSanitizer<string, string>(mock.Object);
            var val1 = sanitizer.Sanitize("bbb");
            sanitizer.Reset();
            var val2 = sanitizer.Sanitize("ccc");
            Assert.AreEqual("aaa", val1);
            Assert.AreEqual("aaa", val2);
        }

        [TestMethod]
        [ExpectedException(typeof(SanitizerRetryException))]
        public void RetryFailTest()
        {
            var mock = new Mock<IGenerator<string, string>>();
            mock.Setup(generator => generator.NextValue()).Returns("aaa");
            var sanitizer = new UniqueValueSanitizer<string, string>(mock.Object);
            sanitizer.RetryLimit = 1;
            var val1 = sanitizer.Sanitize("bbb");
            var val2 = sanitizer.Sanitize("ccc");
        }
    }
}
