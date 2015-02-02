using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Generators.Text
{
    public class CompoundStringGeneratorContext
    {
        public CompoundStringGeneratorContext()
        {
            this.Formats = new List<string>();
            this.ValueFormatters = new Dictionary<string, Func<string, string>>();
        }
        public ICollection<string> Formats { get; private set; }
        public IDictionary<string, Func<string, string>> ValueFormatters { get; private set; }
    }
}
