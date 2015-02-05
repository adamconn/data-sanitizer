using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Sanitizers
{
    public interface IKeyedSanitizer<TInput, TOutput> : ISanitizer
    {
        TOutput Sanitize(TInput input, TOutput defaultValue);
        Func<TInput, bool> ShouldSanitize { get; set; }
    }
}
