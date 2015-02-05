using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Generators.Text
{
    public class CompoundValuesGeneratorContext
    {
        public CompoundValuesGeneratorContext()
        {
            this.Formats = new Dictionary<string, Func<IDictionary<string, string>, string>>();
        }
        public IDictionary<string, Func<IDictionary<string, string>, string>> Formats { get; private set; }
    }
}
