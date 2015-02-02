using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Matchers
{
    public interface IMatcher<T>
    {
        Func<T, bool> Condition { get; }
        Func<T, IEnumerable<T>, T> Action { get; }
    }
}
