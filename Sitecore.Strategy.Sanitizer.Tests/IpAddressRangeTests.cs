using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.Strategy.Sanitizer.Generators.Net;

namespace Sitecore.Strategy.Sanitizer.Tests
{
    [TestClass]
    public class IpAddressRangeTests
    {
        [TestMethod]
        public void EqualsTest()
        {
            var r1 = new IpAddressRange(IPAddress.Parse("1.2.3.4"), IPAddress.Parse("4.3.2.1"));
            var r2 = new IpAddressRange(IPAddress.Parse("4.3.2.1"), IPAddress.Parse("1.2.3.4"));
            Assert.IsTrue(r1.Equals(r2));
            Assert.IsTrue(r2.Equals(r1));
        }
        [TestMethod]
        public void NotEqualsTest()
        {
            var r1 = new IpAddressRange(IPAddress.Parse("1.2.3.4"), IPAddress.Parse("4.3.2.1"));
            var r2 = new IpAddressRange(IPAddress.Parse("5.6.7.8"), IPAddress.Parse("8.7.6.5"));
            Assert.IsFalse(r1.Equals(r2));
            Assert.IsFalse(r2.Equals(r1));
        }
    }
}
