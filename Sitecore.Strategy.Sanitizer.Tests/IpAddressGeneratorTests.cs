using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.Strategy.Sanitizer.Generators.Net;

namespace Sitecore.Strategy.Sanitizer.Tests
{
    [TestClass]
    public class IpAddressGeneratorTests
    {
        [TestMethod]
        public void DefaultRangeTest()
        {
            var generator = new IpAddressGenerator();
            var ranges = generator.DefaultIpAddressRanges;
            for (var i = 0; i < 10; i++)
            {
                var value = generator.NextValue();
                Assert.IsNotNull(value);
                var bytes = value.GetAddressBytes();
                IpAddressRange selectedRange = null;
                foreach (var range in ranges)
                {
                    var start = range.Start.GetAddressBytes();
                    var end = range.End.GetAddressBytes();
                    for (var b = 0; b < bytes.Length; b++)
                    {
                        if ((bytes[b] >= start[b]) && (bytes[b] <= end[b]))
                        {
                            selectedRange = range;
                            break;
                        }
                    }
                    if (selectedRange != null)
                    {
                        break;
                    }
                }
                Assert.IsNotNull(selectedRange);
            }
        }
        [TestMethod]
        public void NoRangeTest()
        {
            var generator = new IpAddressGenerator()
            {
                DefaultIpAddressRanges = new[] {new IpAddressRange(
                    new IPAddress(new byte[] { 0, 0, 0, 0 }),
                    new IPAddress(new byte[] { 0, 0, 0, 0 })
                    )}
            };
            var value = generator.NextValue();
            Assert.AreEqual(new IPAddress(new byte[] { 0, 0, 0, 0 }), value);
            value = generator.NextValue();
            Assert.AreEqual(new IPAddress(new byte[] { 0, 0, 0, 0 }), value);
        }
    }
}
