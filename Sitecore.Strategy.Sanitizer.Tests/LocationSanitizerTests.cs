using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sitecore.Strategy.Sanitizer.Matchers;
using Sitecore.Strategy.Sanitizer.Models;
using Sitecore.Strategy.Sanitizer.Sanitizers;

namespace Sitecore.Strategy.Sanitizer.Tests
{
    [TestClass]
    public class LocationSanitizerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullLocation()
        {
            var sanitizer = new LocationSanitizer();
            sanitizer.Sanitize(null);
        }
        
        [TestMethod]
        public void DefaultLocation()
        {
            var locBefore = new Location();
            var sanitizer = new LocationSanitizer();
            var locAfter = sanitizer.Sanitize(locBefore);
            Assert.IsNull(locAfter);
        }

        [TestMethod]
        public void OneDataSource()
        {
            var mock = new Mock<IMatcher<Location>>();
            mock.Setup(loc => loc.Condition).Returns(loc => true);
            var locSanitized = new Location();
            mock.Setup(loc => loc.Action).Returns((loc, locs) => locs.FirstOrDefault());

            var sanitizer = new LocationSanitizer();
            sanitizer.DataSources.Add(mock.Object, new Location[] { locSanitized });

            var locBefore = new Location();
            var locAfter = sanitizer.Sanitize(locBefore);
            Assert.AreSame(locSanitized, locAfter);
        }

        [TestMethod]
        public void NoMatcher()
        {
            var mock = new Mock<IMatcher<Location>>();
            mock.Setup(loc => loc.Condition).Returns(loc => false);
            var sanitizer = new LocationSanitizer();
            sanitizer.DataSources.Add(mock.Object, new Location[] {});
            var locAfter = sanitizer.Sanitize(new Location());
            Assert.IsNull(locAfter);
        }
        [TestMethod]
        public void TwoMatchers()
        {
            var mockUs = new Mock<IMatcher<Location>>();
            mockUs.Setup(loc => loc.Condition).Returns(loc => loc.Country == "US");
            mockUs.Setup(loc => loc.Action).Returns((loc, locs) => locs.FirstOrDefault());

            var mockCa = new Mock<IMatcher<Location>>();
            mockCa.Setup(loc => loc.Condition).Returns(loc => loc.Country == "CA");
            mockCa.Setup(loc => loc.Action).Returns((loc, locs) => locs.FirstOrDefault());

            var sanitizer = new LocationSanitizer();
            var cityUs = new Location() { Country = "US", City = "New York" };
            var cityCa = new Location() { Country = "CA", City = "Toronto" };
            sanitizer.DataSources.Add(mockUs.Object, new Location[] { cityUs });
            sanitizer.DataSources.Add(mockCa.Object, new Location[] { cityCa });
            var locAfter1 = sanitizer.Sanitize(new Location());
            var locAfter2 = sanitizer.Sanitize(new Location() { Country = "US" });
            var locAfter3 = sanitizer.Sanitize(new Location() { Country = "CA" });
            Assert.IsNull(locAfter1);
            Assert.AreSame(cityUs, locAfter2);
            Assert.AreSame(cityCa, locAfter3);
        }
    }
}
