using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Strategy.Sanitizer.Matchers;
using Sitecore.Strategy.Sanitizer.Models;

namespace Sitecore.Strategy.Sanitizer.Sanitizers
{
    public class LocationSanitizer : ISimpleSanitizer<Location>
    {
        public LocationSanitizer()
        {
            this.DataSources = new Dictionary<IMatcher<Location>, IEnumerable<Location>>();
        }
        public Func<Location, bool> ShouldSanitize { get; set; }
        public IDictionary<IMatcher<Location>, IEnumerable<Location>> DataSources { get; private set; }
        public Location Sanitize(Location location)
        {
            if (location == null)
            {
                throw new ArgumentNullException("value");
            }
            if (this.ShouldSanitize == null || this.ShouldSanitize(location))
            {
                foreach (var matcher in this.DataSources.Keys)
                {
                    if (matcher.Condition(location))
                    {
                        var locations = this.DataSources[matcher];
                        return matcher.Action(location, locations);
                    }
                }
            }
            return location;
        }

        public bool EnsureUniqueValues
        {
            get { return false; }
        }
        public int RetryLimit
        {
            get { return -1; }
        }
        public void Reset()
        {
            throw new NotSupportedException();
        }
    }
}
