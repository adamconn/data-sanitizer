using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Strategy.Sanitizer.Generators;

namespace Sitecore.Strategy.Sanitizer.Sanitizers
{
    public class UniqueValueSanitizer<TInput, TOutput> : IKeyedSanitizer<TInput, TOutput>
    {
        public UniqueValueSanitizer(IGenerator<TInput, TOutput> generator)
        {
            this.Generator = generator;
            this.UsedValues = new Dictionary<TInput, TOutput>();
            this.RetryLimit = 100;
        }
        protected IGenerator<TInput, TOutput> Generator { get; private set; }
        protected IDictionary<TInput, TOutput> UsedValues { get; private set; }

        public Func<TInput, bool> ShouldSanitize { get; set; }

        public virtual TOutput Sanitize(TInput inputValue, TOutput defaultValue)
        {
            if (this.ShouldSanitize != null && !this.ShouldSanitize(inputValue))
            {
                return defaultValue;
            }
            if (this.UsedValues.ContainsKey(inputValue))
            {
                return this.UsedValues[inputValue];
            }
            var limit = this.RetryLimit;
            while (limit > 0)
            {
                limit--;
                var newValue = this.Generator.NextValue();
                if (!this.UsedValues.Any(p => p.Value.Equals(newValue)))
                {
                    this.UsedValues.Add(inputValue, newValue);
                    return newValue;
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
