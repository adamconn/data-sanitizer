using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Matchers
{
    public class SanitizerMatcher<T> : IMatcher<T>
    {
        public SanitizerMatcher(Func<T, bool> condition, Func<T, IEnumerable<T>, T> action)
        {
            this.Condition = condition;
            this.Action = action;
        }
        public Func<T, bool> Condition { get; private set; }
        public Func<T, IEnumerable<T>, T> Action { get; private set; }
    }
}
