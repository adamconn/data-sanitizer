using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Sanitizers
{
    public interface ISimpleSanitizer<T> : ISanitizer
    {
        T Sanitize(T input);
        Func<T, bool> ShouldSanitize { get; set; }
    }
}
