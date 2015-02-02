using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Generators
{
    public interface IGenerator<TSource, TValue>
    {
        void AddValues(params TSource[] values);
        void RemoveValues(params TSource[] values);
        TValue NextValue();
    }
}
