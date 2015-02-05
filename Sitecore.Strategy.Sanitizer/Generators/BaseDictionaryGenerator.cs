using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Generators
{
    public abstract class BaseDictionaryGenerator<TSource, TTarget> : BaseGenerator<TSource, IDictionary<string, TTarget>>
    {
    }
}
