using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Generators.Text
{
    public class CompoundStringGenerator : BaseGenerator<KeyValuePair<string, IEnumerable<string>>, string>
    {
        public CompoundStringGenerator(CompoundStringGeneratorContext context)
        {
            this.Context = context;
        }
        protected CompoundStringGeneratorContext Context { get; private set; }

        protected virtual string GetValue(IEnumerable<string> values)
        {
            var position = this.Random.Next(0, values.Count());
            return values.Skip(position).Take(1).FirstOrDefault();
        }
        public override string NextValue()
        {
            if (this.Context == null || this.Context.Formats == null || !this.Context.Formats.Any())
            {
                throw new Exception("No formatting has been specified in the context.");
            }
            var value = GetValue(this.Context.Formats);
            //
            var matches = Regex.Matches(value, @"\{([\w\-]*)\}");
            var keys = new List<string>();
            foreach (Match match in matches)
            {
                keys.Add(match.Groups[1].Value);
            }
            //
            foreach (var key in keys.Distinct())
            {
                var pair = this.Values.FirstOrDefault(p => p.Key == key);
                if (pair.Key == null)
                {
                    continue;
                }
                var value2 = GetValue(pair.Value);
                //apply formatter if applicable
                if (this.Context.ValueFormatters.ContainsKey(key))
                {
                    var formatter = this.Context.ValueFormatters[key];
                    if (formatter != null)
                    {
                        value2 = formatter(value2);
                    }
                }
                var pattern = string.Format("{{{0}}}", key);
                value = value.Replace(pattern, value2);
            }
            return value;
        }
    }
}
