using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Sanitizers
{
    public interface ISanitizer<T>
    {
        T Sanitize(T value);
        bool EnsureUniqueValues { get; }
        int RetryLimit { get; }
        Func<T, bool> ShouldSanitize { get; set; }
        void Reset();
    }
}
