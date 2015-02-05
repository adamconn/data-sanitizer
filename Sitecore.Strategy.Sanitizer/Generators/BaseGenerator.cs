using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Generators
{
    public abstract class BaseGenerator<TSource, TTarget> : IGenerator<TSource, TTarget>
    {
        protected BaseGenerator()
        {
            this.Random = new Random(Guid.NewGuid().GetHashCode());
        }
        public abstract TTarget NextValue();

        protected Random Random { get; private set; }
        protected IEnumerable<TSource> Values { get; set; }

        public virtual void AddValues(params TSource[] values)
        {
            if (values == null || values.Length == 0)
            {
                return;
            }
            if (this.Values == null)
            {
                this.Values = values;
            }
            else
            {
                this.Values = this.Values.Concat(values);
            }
        }

        public virtual void RemoveValues(params TSource[] values)
        {
            if (values == null || values.Length == 0 || this.Values == null || !this.Values.Any())
            {
                return;
            }
            this.Values = this.Values.Except(values);
        }
    }
}
