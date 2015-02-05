using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Strategy.Sanitizer.Generators;

namespace Sitecore.Strategy.Sanitizer.Sanitizers
{
    public class UniqueValueSanitizer<TSource, TValue> : ISanitizer<TValue>
    {
        public UniqueValueSanitizer(IGenerator<TSource, TValue> generator)
        {
            this.Generator = generator;
            this.UsedValues = new Dictionary<TValue, TValue>();
            this.RetryLimit = 100;
        }
        protected IGenerator<TSource, TValue> Generator { get; private set; }
        protected IDictionary<TValue, TValue> UsedValues { get; private set; }
        public Func<TValue, bool> ShouldSanitize { get; set; }

        public virtual TValue Sanitize(TValue value)
        {
            if (this.ShouldSanitize != null && !this.ShouldSanitize(value))
            {
                return value;
            }
            if (this.UsedValues.ContainsKey(value))
            {
                return this.UsedValues[value];
            }
            var limit = this.RetryLimit;
            while (limit > 0)
            {
                limit--;
                var address = this.Generator.NextValue();
                if (!this.UsedValues.Any(p => p.Value.Equals(address)))
                {
                    this.UsedValues.Add(value, address);
                    return address;
                }
            }
            throw new SanitizerRetryException(this.RetryLimit);
        }

        public bool EnsureUniqueValues
        {
            get { return true; }
        }
        public int RetryLimit { get; set; }

        public virtual void Reset()
        {
            this.UsedValues.Clear();
        }
    }
}
